import sys
import shutil
from pathlib import Path

APP_DIR = Path(__file__).resolve().parent
TOOLKIT_DIR = APP_DIR.parents[1]
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


def fail(message):
    print()
    print("ERROR:")
    print(message)
    print()
    sys.exit(1)


def luminance_bgr(img):
    rgb = img[:, :, :3].astype(np.float32)
    # OpenCV is BGR, so use B,G,R order here.
    return 0.0722 * rgb[..., 0] + 0.7152 * rgb[..., 1] + 0.2126 * rgb[..., 2]


def blur_3x3(img):
    kernel = np.array([[1, 2, 1], [2, 4, 2], [1, 2, 1]], dtype=np.float32)
    kernel /= kernel.sum()
    padded = np.pad(img, ((1, 1), (1, 1)), mode="edge")
    out = np.zeros_like(img)
    for y in range(3):
        for x in range(3):
            out += kernel[y, x] * padded[y:y + img.shape[0], x:x + img.shape[1]]
    return out


def build_rmaos(diffuse, normal, complex_m):
    h, w = diffuse.shape[:2]
    if normal.shape[:2] != (h, w):
        normal = cv2.resize(normal, (w, h), interpolation=cv2.INTER_LINEAR)
    if complex_m.shape[:2] != (h, w):
        complex_m = cv2.resize(complex_m, (w, h), interpolation=cv2.INTER_LINEAR)

    diffuse_rgb = diffuse[:, :, :3] if diffuse.ndim == 3 else cv2.cvtColor(diffuse, cv2.COLOR_GRAY2BGR)
    normal_rgb = normal[:, :, :3] if normal.ndim == 3 else cv2.cvtColor(normal, cv2.COLOR_GRAY2BGR)

    rough = 1.0 - (luminance_bgr(diffuse_rgb) / 255.0)
    rough = np.clip(rough, 0.05, 0.95)

    # Existing complex parallax _m convention: blue channel often carries metalness-ish data.
    if complex_m.ndim == 3 and complex_m.shape[2] >= 3:
        metal = complex_m[..., 0].astype(np.float32) / 255.0
    else:
        metal = np.zeros((h, w), dtype=np.float32)

    n = normal_rgb.astype(np.float32) / 255.0
    n = n * 2.0 - 1.0
    dx = np.gradient(n, axis=1)
    dy = np.gradient(n, axis=0)
    curvature = np.sqrt(np.sum(dx * dx + dy * dy, axis=2))
    curvature = curvature / (np.max(curvature) + 1e-5)
    ao = 1.0 - curvature
    ao = blur_3x3(ao)
    ao = np.clip(ao, 0.45, 1.0)

    specular = 1.0 - rough

    out = np.zeros((h, w, 4), dtype=np.uint8)
    out[:, :, 0] = (ao * 255).astype(np.uint8)
    out[:, :, 1] = (metal * 255).astype(np.uint8)
    out[:, :, 2] = (rough * 255).astype(np.uint8)
    out[:, :, 3] = (specular * 255).astype(np.uint8)
    return out


def extract_height_from_m(complex_m):
    if complex_m.ndim == 3 and complex_m.shape[2] >= 4:
        return complex_m[:, :, 3].astype(np.uint8)
    if complex_m.ndim == 2:
        return complex_m.astype(np.uint8)
    return cv2.cvtColor(complex_m[:, :, :3], cv2.COLOR_BGR2GRAY)


def process_set(parts, src_root, out_root, tmp):
    diffuse_path = parts.get("albedo")
    normal_path = parts.get("normal")
    m_path = parts.get("complex_m")
    if not diffuse_path or not normal_path or not m_path:
        return False

    rel_dir = diffuse_path.parent.relative_to(src_root)

    # Community Shaders PBR textures belong in textures\PBR\
    if rel_dir.parts and rel_dir.parts[0].lower() == "textures":
        rel_dir = Path("textures") / "PBR" / Path(*rel_dir.parts[1:])
    else:
        rel_dir = Path("textures") / "PBR" / rel_dir

    out_dir = out_root / rel_dir
    out_dir.mkdir(parents=True, exist_ok=True)
    name = diffuse_path.stem
    if name.lower().endswith("_d"):
        name = name[:-2]

    print(f"Processing {rel_dir / name}")

    # Preserve diffuse alpha and original dimensions. Existing DDS files are copied exactly.
    copy_or_convert_to_dds(diffuse_path, out_dir / f"{name}.dds", "BC7_UNORM", True, tmp)
    copy_or_convert_to_dds(normal_path, out_dir / f"{name}_n.dds", "BC7_UNORM", False, tmp)

    diffuse, _ = read_image(diffuse_path, tmp, cv2.IMREAD_UNCHANGED)
    normal, _ = read_image(normal_path, tmp, cv2.IMREAD_UNCHANGED)
    complex_m, _ = read_image(m_path, tmp, cv2.IMREAD_UNCHANGED)
    if diffuse is None or normal is None or complex_m is None:
        print("  skipped - could not read one or more source textures")
        return False

    height = extract_height_from_m(complex_m)
    p_png = tmp / f"{name}_p.png"
    write_png(p_png, height)
    png_to_dds(p_png, out_dir / f"{name}_p.dds", "BC4_UNORM", srgb=False)

    rmaos = build_rmaos(diffuse, normal, complex_m)
    r_png = tmp / f"{name}_rmaos.png"
    write_png(r_png, rmaos)
    png_to_dds(r_png, out_dir / f"{name}_rmaos.dds", "BC7_UNORM", srgb=False)

    return True


def print_usage():
    print("Skyking Complex Parallax to PBR")
    print("Usage:")
    print("  python convert.py <source_folder> <output_folder>")


def main():
    if len(sys.argv) < 3:
        print_usage()
        fail("Missing arguments.")

    check_runtime(needs_magick=True, needs_texconv=True)

    src = Path(sys.argv[1]).resolve()
    out = Path(sys.argv[2]).resolve()
    if not src.exists():
        fail(f"Source folder does not exist: {src}")

    out.mkdir(parents=True, exist_ok=True)
    tmp = out / "_tmp"
    tmp.mkdir(parents=True, exist_ok=True)

    print()
    print("=== Skyking Complex Parallax to PBR ===")
    print(f"Source: {src}")
    print(f"Output: {out}")
    print()

    sets = scan_texture_sets(src, SUPPORTED_TEXTURE_EXTS)
    count = 0
    for _, parts in sets.items():
        if process_set(parts, src, out, tmp):
            count += 1

    shutil.rmtree(tmp, ignore_errors=True)
    print()
    print(f"done - processed {count} texture sets")


if __name__ == "__main__":
    main()
