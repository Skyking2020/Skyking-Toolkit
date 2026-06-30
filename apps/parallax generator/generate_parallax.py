import copy
import json
import sys
import shutil
from pathlib import Path

APP_DIR = Path(__file__).resolve().parent
TOOLKIT_DIR = APP_DIR.parents[1]
CONFIG_PATH = APP_DIR / "config.json"
sys.path.insert(0, str(TOOLKIT_DIR))

import cv2
import numpy as np

from shared.runtime import check_runtime
from shared.textures import (
    SUPPORTED_TEXTURE_EXTS,
    read_image,
    png_to_dds,
    write_png,
    copy_or_convert_to_dds,
)
from shared.naming import scan_texture_sets


DEFAULT_CONFIG = {
    "default": {
        "contrast_factor": 0.5,
        "clamp_low": 60,
        "clamp_high": 200,
        "blur_radius": 4,
        "red_value": 75,
        "green_value": 65,
        "blue_value": 75,
        "red_brightness": 1.0,
        "green_brightness": 1.0,
        "blue_brightness": 1.0,
    },
    "categories": {},
    "overrides": {},
    "exclude": [],
}


def fail(message):
    print()
    print("ERROR:")
    print(message)
    print()
    sys.exit(1)


def deep_update(target, source):
    for key, value in source.items():
        if isinstance(value, dict) and isinstance(target.get(key), dict):
            deep_update(target[key], value)
        else:
            target[key] = value


def load_config():
    if not CONFIG_PATH.exists():
        print(f"WARNING: Missing config.json at {CONFIG_PATH}; using built-in defaults.", flush=True)
        return copy.deepcopy(DEFAULT_CONFIG)

    with CONFIG_PATH.open("r", encoding="utf-8") as f:
        user_config = json.load(f)

    config = copy.deepcopy(DEFAULT_CONFIG)
    deep_update(config, user_config)
    return config


def output_name(diffuse_path):
    name = diffuse_path.stem
    if name.lower().endswith("_d"):
        name = name[:-2]
    return name


def texture_identity(parts, src_root):
    diffuse_path = parts.get("albedo")
    if not diffuse_path:
        return "", ""

    name = output_name(diffuse_path)
    try:
        rel_dir = diffuse_path.parent.relative_to(src_root)
        rel_key = str(rel_dir / name).replace("/", "\\").lower()
    except ValueError:
        rel_key = name.lower()

    return name.lower(), rel_key


def settings_for_texture(name, rel_key, config):
    settings = copy.deepcopy(config.get("default", {}))

    for category, values in config.get("categories", {}).items():
        category_text = str(category).lower()
        if category_text in rel_key or category_text in name:
            if isinstance(values, dict):
                deep_update(settings, values)

    for override_name, values in config.get("overrides", {}).items():
        override_text = str(override_name).lower()
        if override_text == name or override_text == rel_key:
            if isinstance(values, dict):
                deep_update(settings, values)

    return settings


def should_exclude(name, rel_key, config):
    for item in config.get("exclude", []):
        needle = str(item).lower()
        if needle and (needle in name or needle in rel_key):
            return True
    return False


def to_gray_uint8(img):
    if img is None:
        return None
    if img.ndim == 2:
        gray = img
    else:
        gray = cv2.cvtColor(img[:, :, :3], cv2.COLOR_BGR2GRAY)
    if gray.dtype != np.uint8:
        gray = cv2.normalize(gray, None, 0, 255, cv2.NORM_MINMAX).astype(np.uint8)
    return gray


def raw_normal_to_height(normal):
    """FFT-based normal-map-to-height conversion from the original parallax tool."""
    if normal is None:
        return None

    if normal.ndim == 2:
        normal = cv2.cvtColor(normal, cv2.COLOR_GRAY2BGR)

    img = normal.astype(np.float32) / 255.0
    if img.shape[2] == 4:
        img = img[:, :, :3]

    # OpenCV reads BGR. The original tool uses B and G as the normal X/Y channels.
    nx = img[:, :, 2] * 2.0 - 1.0
    ny = img[:, :, 1] * 2.0 - 1.0

    gx = -nx
    gy = -ny

    h, w = gx.shape
    fx = np.fft.fft2(gx)
    fy = np.fft.fft2(gy)

    wx = 2.0 * np.pi * np.fft.fftfreq(w).reshape(1, w)
    wy = 2.0 * np.pi * np.fft.fftfreq(h).reshape(h, 1)

    denom = wx ** 2 + wy ** 2
    denom[0, 0] = 1.0

    fz = (-1j * wx * fx - 1j * wy * fy) / denom
    fz[0, 0] = 0.0

    hmap = np.real(np.fft.ifft2(fz))
    hmap -= hmap.min()
    if hmap.max() > 0:
        hmap /= hmap.max()

    return hmap


def apply_height_settings(height, settings):
    """Apply contrast, clamp, and config-controlled blur to a height map."""
    if height is None:
        return None

    h = height.astype(np.float32)
    if h.max() <= 1.0:
        h *= 255.0

    mean = h.mean()
    contrast = float(settings.get("contrast_factor", 0.5))
    h = 127.0 + (h - mean) * contrast

    clamp_low = int(settings.get("clamp_low", 60))
    clamp_high = int(settings.get("clamp_high", 200))
    if clamp_high <= clamp_low:
        clamp_low, clamp_high = 0, 255

    h = np.clip(h, clamp_low, clamp_high)

    blur = float(settings.get("blur_radius", 4))
    if blur > 0:
        sigma = (blur ** 1.6) * 0.9
        h = cv2.GaussianBlur(
            h,
            (0, 0),
            sigmaX=sigma,
            sigmaY=sigma,
            borderType=cv2.BORDER_REFLECT_101,
        )

    return np.ascontiguousarray(np.clip(h, 0, 255).astype(np.uint8))


def normal_to_height(normal, settings):
    """Generate a blurred displacement/height map from a normal map."""
    return apply_height_settings(raw_normal_to_height(normal), settings)


def luminance_bgr(img):
    if img.ndim == 2:
        return img.astype(np.float32)
    rgb = img[:, :, :3].astype(np.float32)
    return 0.0722 * rgb[..., 0] + 0.7152 * rgb[..., 1] + 0.2126 * rgb[..., 2]


def resize_like(img, reference):
    h, w = reference.shape[:2]
    if img.shape[:2] == (h, w):
        return img
    return cv2.resize(img, (w, h), interpolation=cv2.INTER_LINEAR)


def channel_value(settings, value_key, brightness_key):
    value = float(settings.get(value_key, 0))
    brightness = float(settings.get(brightness_key, 1.0))
    return int(np.clip(value * brightness, 0, 255))


def build_complex_m(diffuse, height, settings):
    h, w = diffuse.shape[:2]
    if height.shape[:2] != (h, w):
        height = cv2.resize(height, (w, h), interpolation=cv2.INTER_LINEAR)

    out = np.zeros((h, w, 4), dtype=np.uint8)
    out[:, :, 0] = channel_value(settings, "blue_value", "blue_brightness")
    out[:, :, 1] = channel_value(settings, "green_value", "green_brightness")
    out[:, :, 2] = channel_value(settings, "red_value", "red_brightness")
    out[:, :, 3] = height
    return out


def build_rmaos(diffuse, normal):
    h, w = diffuse.shape[:2]
    if normal.shape[:2] != (h, w):
        normal = cv2.resize(normal, (w, h), interpolation=cv2.INTER_LINEAR)

    diff_luma = luminance_bgr(diffuse) / 255.0
    rough = np.clip(1.0 - diff_luma, 0.05, 0.95)
    metal = np.zeros((h, w), dtype=np.float32)

    normal_rgb = normal[:, :, :3] if normal.ndim == 3 else cv2.cvtColor(normal, cv2.COLOR_GRAY2BGR)
    n = normal_rgb.astype(np.float32) / 255.0
    n = n * 2.0 - 1.0
    dx = np.gradient(n, axis=1)
    dy = np.gradient(n, axis=0)
    curvature = np.sqrt(np.sum(dx * dx + dy * dy, axis=2))
    curvature = curvature / (np.max(curvature) + 1e-5)
    ao = np.clip(1.0 - cv2.GaussianBlur(curvature, (3, 3), 0), 0.45, 1.0)
    spec = 1.0 - rough

    out = np.zeros((h, w, 4), dtype=np.uint8)
    out[:, :, 0] = (ao * 255).astype(np.uint8)
    out[:, :, 1] = (metal * 255).astype(np.uint8)
    out[:, :, 2] = (rough * 255).astype(np.uint8)
    out[:, :, 3] = (spec * 255).astype(np.uint8)
    return out



def pbr_relative_dir(rel_dir):
    """Return relative output folder for Community Shaders PBR textures."""
    if rel_dir.parts and rel_dir.parts[0].lower() == "textures":
        return Path("textures") / "PBR" / Path(*rel_dir.parts[1:])
    return Path("textures") / "PBR" / rel_dir


def ensure_dir(path):
    path.mkdir(parents=True, exist_ok=True)
    return path

def process_set(parts, src_root, out_root, tmp, mode, config):
    diffuse_path = parts.get("albedo")
    normal_path = parts.get("normal")
    if not diffuse_path or not normal_path:
        return False

    rel_dir = diffuse_path.parent.relative_to(src_root)
    complex_out_dir = ensure_dir(out_root / rel_dir)
    pbr_out_dir = ensure_dir(out_root / pbr_relative_dir(rel_dir))
    name = output_name(diffuse_path)
    name_key, rel_key = texture_identity(parts, src_root)
    settings = settings_for_texture(name_key, rel_key, config)

    print(f"Processing {rel_dir / name}", flush=True)

    diffuse, _ = read_image(diffuse_path, tmp, cv2.IMREAD_UNCHANGED)
    normal, _ = read_image(normal_path, tmp, cv2.IMREAD_UNCHANGED)
    if diffuse is None or normal is None:
        print("  skipped - could not read diffuse or normal", flush=True)
        return False

    if mode in ("complex", "both"):
        copy_or_convert_to_dds(diffuse_path, complex_out_dir / f"{name}.dds", "BC7_UNORM", True, tmp)
        copy_or_convert_to_dds(normal_path, complex_out_dir / f"{name}_n.dds", "BC7_UNORM", False, tmp)

    if mode in ("pbr", "both"):
        copy_or_convert_to_dds(diffuse_path, pbr_out_dir / f"{name}.dds", "BC7_UNORM", True, tmp)
        copy_or_convert_to_dds(normal_path, pbr_out_dir / f"{name}_n.dds", "BC7_UNORM", False, tmp)

    height_path = parts.get("height")
    if height_path:
        height, _ = read_image(height_path, tmp, cv2.IMREAD_GRAYSCALE)
        height = apply_height_settings(height, settings)
    else:
        height = normal_to_height(normal, settings)

    if height is None:
        print("  skipped generated maps - could not create/read height", flush=True)
        return False

    p_png = tmp / f"{name}_p.png"
    write_png(p_png, height)

    if mode in ("complex", "both"):
        png_to_dds(p_png, complex_out_dir / f"{name}_p.dds", "BC4_UNORM", srgb=False)

    if mode in ("pbr", "both"):
        png_to_dds(p_png, pbr_out_dir / f"{name}_p.dds", "BC4_UNORM", srgb=False)

    if mode in ("complex", "both"):
        existing_m = parts.get("complex_m")
        if existing_m:
            copy_or_convert_to_dds(existing_m, complex_out_dir / f"{name}_m.dds", "BC7_UNORM", False, tmp)
        else:
            m_map = build_complex_m(diffuse, height, settings)
            m_png = tmp / f"{name}_m.png"
            write_png(m_png, m_map)
            png_to_dds(m_png, complex_out_dir / f"{name}_m.dds", "BC7_UNORM", srgb=False)

    if mode in ("pbr", "both"):
        existing_rmaos = parts.get("rmaos")
        if existing_rmaos:
            copy_or_convert_to_dds(existing_rmaos, pbr_out_dir / f"{name}_rmaos.dds", "BC7_UNORM", False, tmp)
        else:
            rmaos = build_rmaos(diffuse, normal)
            r_png = tmp / f"{name}_rmaos.png"
            write_png(r_png, rmaos)
            png_to_dds(r_png, pbr_out_dir / f"{name}_rmaos.dds", "BC7_UNORM", srgb=False)

    return True


def print_usage():
    print()
    print("Skyking Parallax Generator")
    print("Usage:")
    print("  python generate_parallax.py <source_folder> <output_folder> <complex|pbr|both>")
    print()


def main():
    if len(sys.argv) < 4:
        print_usage()
        fail("Missing arguments.")

    check_runtime(needs_magick=True, needs_texconv=True)

    src = Path(sys.argv[1]).resolve()
    out = Path(sys.argv[2]).resolve()
    mode = sys.argv[3].lower().strip()
    if mode not in ("complex", "pbr", "both"):
        mode = "both"

    if not src.exists():
        fail(f"Source folder does not exist: {src}")

    config = load_config()

    out.mkdir(parents=True, exist_ok=True)
    tmp = out / "_tmp"
    tmp.mkdir(parents=True, exist_ok=True)

    print()
    print("=== Skyking Parallax Generator ===")
    print(f"Source: {src}")
    print(f"Output: {out}")
    print(f"Mode: {mode}")
    print(f"Config: {CONFIG_PATH}")
    print()

    raw_sets = scan_texture_sets(src, SUPPORTED_TEXTURE_EXTS)
    sets = {}
    for key, parts in raw_sets.items():
        name_key, rel_key = texture_identity(parts, src)
        if should_exclude(name_key, rel_key, config):
            print(f"Excluded {rel_key}", flush=True)
            continue
        sets[key] = parts

    total = len(sets)
    print(f"__SKYKING_TOTAL__={total}", flush=True)

    count = 0
    done = 0
    try:
        for _, parts in sets.items():
            done += 1
            if process_set(parts, src, out, tmp, mode, config):
                count += 1
            print(f"__SKYKING_PROGRESS__={done}/{total}", flush=True)
    finally:
        shutil.rmtree(tmp, ignore_errors=True)

    print()
    print(f"done - processed {count} texture sets", flush=True)


if __name__ == "__main__":
    main()
