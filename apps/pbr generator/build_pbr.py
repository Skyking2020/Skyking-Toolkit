import sys
import json
import shutil
from pathlib import Path

# Allow this app to import /shared when launched directly from a .bat or future GUI.
APP_DIR = Path(__file__).resolve().parent
TOOLKIT_DIR = APP_DIR.parents[1]
sys.path.insert(0, str(TOOLKIT_DIR))

import cv2
import numpy as np

from shared.runtime import check_runtime
from shared.textures import SUPPORTED_TEXTURE_EXTS, image_to_png, read_gray, png_to_dds, write_png

CONFIG_PATH = APP_DIR / "config.json"


def fail(message):
    print()
    print("ERROR:")
    print(message)
    print()
    sys.exit(1)


def load_config():
    if not CONFIG_PATH.exists():
        fail(f"Missing config.json: {CONFIG_PATH}")
    with CONFIG_PATH.open("r", encoding="utf-8") as f:
        return json.load(f)["aliases"]


def norm(s):
    return s.lower().replace("_", "").replace("-", "").replace(" ", "")


def group(files):
    out = {}
    for f in files:
        base_name = f.stem.split("_")[0]
        out.setdefault(base_name, []).append(f)
    return out


def match(files, aliases):
    res = {}
    for f in files:
        n = norm(f.stem)
        for key, vals in aliases.items():
            if key in res:
                continue
            if any(norm(v) in n for v in vals):
                res[key] = f
    return res


def texture_to_png(src_path, tmp):
    return image_to_png(src_path, tmp)


def parallax(img):
    if img is None:
        return None
    h = img.astype(np.float32)
    if len(h.shape) == 3:
        h = cv2.cvtColor(h[:, :, :3], cv2.COLOR_BGR2GRAY)
    h = cv2.normalize(h, None, 0, 255, cv2.NORM_MINMAX)
    h = 127 + (h - h.mean()) * 0.4
    return np.clip(h, 60, 200).astype(np.uint8)


def rmaos(roughness, metalness, ao, specular):
    if roughness is None:
        return None

    h, w = roughness.shape[:2]

    if metalness is not None and metalness.shape[:2] != (h, w):
        metalness = cv2.resize(metalness, (w, h), interpolation=cv2.INTER_LINEAR)
    if ao is not None and ao.shape[:2] != (h, w):
        ao = cv2.resize(ao, (w, h), interpolation=cv2.INTER_LINEAR)
    if specular is not None and specular.shape[:2] != (h, w):
        specular = cv2.resize(specular, (w, h), interpolation=cv2.INTER_LINEAR)

    g = metalness if metalness is not None else np.zeros((h, w), np.uint8)
    b = ao if ao is not None else np.full((h, w), 255, np.uint8)
    a = specular if specular is not None else 255 - roughness

    out = np.zeros((h, w, 4), np.uint8)
    out[..., 0] = b       # PNG stores BGR(A) in OpenCV; this becomes DDS blue/AO correctly from the PNG data.
    out[..., 1] = g       # metallic
    out[..., 2] = roughness
    out[..., 3] = a       # specular
    return out


def print_usage():
    print()
    print("Skyking PBR Generator")
    print("Usage:")
    print("  python build_pbr.py <source_folder> <output_folder> <flip_green_y/n>")
    print()


def main():
    if len(sys.argv) < 4:
        print_usage()
        fail("Missing arguments.")

    check_runtime(needs_magick=True, needs_texconv=True)

    src = Path(sys.argv[1]).resolve()
    out = Path(sys.argv[2]).resolve()
    flip = sys.argv[3].lower().strip() == "y"

    if not src.exists():
        fail(f"Source folder does not exist: {src}")

    out.mkdir(parents=True, exist_ok=True)
    tmp = out / "_tmp"
    tmp.mkdir(parents=True, exist_ok=True)

    aliases = load_config()
    files = [f for f in src.rglob("*") if f.is_file() and f.suffix.lower() in SUPPORTED_TEXTURE_EXTS]

    if not files:
        shutil.rmtree(tmp, ignore_errors=True)
        fail(f"No supported texture files found in: {src}")

    grouped = group(files)

    print()
    print("=== Skyking PBR Generator ===")
    print(f"Source: {src}")
    print(f"Output: {out}")
    print(f"Flip normal green channel: {'Yes' if flip else 'No'}")
    print(f"Texture files found: {len(files)}", flush=True)
    print(f"Texture sets found: {len(grouped)}", flush=True)
    print(f"__SKYKING_TOTAL__={len(grouped)}", flush=True)
    print()

    count = 0
    for done, (name, mats) in enumerate(grouped.items(), start=1):
        m = match(mats, aliases)

        albedo = m.get("albedo")
        normal = m.get("normal")
        rough = m.get("roughness")
        metal = m.get("metalness")
        ao = m.get("ao")
        spec = m.get("specular")
        height = m.get("height")

        if not albedo or not normal:
            print(f"skip {name} - missing albedo or normal", flush=True)
            print(f"__SKYKING_PROGRESS__={done}/{len(grouped)}", flush=True)
            continue

        print(f"Processing {name}", flush=True)

        # Albedo/diffuse is a color texture: BC7 sRGB. Alpha is preserved when present.
        a_png = texture_to_png(albedo, tmp)
        png_to_dds(a_png, out / f"{name}.dds", "BC7_UNORM", srgb=True)

        # Normal is data, not color: linear/non-sRGB.
        n_png = texture_to_png(normal, tmp)
        n = cv2.imread(str(n_png), cv2.IMREAD_UNCHANGED) if n_png else None
        if n is None:
            print(f"Could not read normal map for {name}")
        else:
            if flip and len(n.shape) == 3 and n.shape[2] >= 2:
                n[:, :, 1] = 255 - n[:, :, 1]
            npath = tmp / f"{name}_n.png"
            write_png(npath, n)
            png_to_dds(npath, out / f"{name}_n.dds", "BC7_UNORM", srgb=False)

        # Height/parallax is data: BC4 linear/non-sRGB.
        if height:
            h_png = texture_to_png(height, tmp)
            h = cv2.imread(str(h_png), cv2.IMREAD_UNCHANGED) if h_png else None
            h = parallax(h)
            if h is not None:
                hpath = tmp / f"{name}_p.png"
                write_png(hpath, h)
                png_to_dds(hpath, out / f"{name}_p.dds", "BC4_UNORM", srgb=False)

        # RMAOS is packed data: BC7 linear/non-sRGB.
        if rough:
            r_src = texture_to_png(rough, tmp)
            m_src = texture_to_png(metal, tmp) if metal else None
            a_src = texture_to_png(ao, tmp) if ao else None
            s_src = texture_to_png(spec, tmp) if spec else None

            r = cv2.imread(str(r_src), cv2.IMREAD_GRAYSCALE) if r_src else None
            metal_img = cv2.imread(str(m_src), cv2.IMREAD_GRAYSCALE) if m_src else None
            ao_img = cv2.imread(str(a_src), cv2.IMREAD_GRAYSCALE) if a_src else None
            spec_img = cv2.imread(str(s_src), cv2.IMREAD_GRAYSCALE) if s_src else None

            rma = rmaos(r, metal_img, ao_img, spec_img)
            if rma is not None:
                rpath = tmp / f"{name}_rmaos.png"
                write_png(rpath, rma)
                png_to_dds(rpath, out / f"{name}_rmaos.dds", "BC7_UNORM", srgb=False)

        count += 1
        print(f"done {name}", flush=True)
        print(f"__SKYKING_PROGRESS__={done}/{len(grouped)}", flush=True)

    shutil.rmtree(tmp, ignore_errors=True)
    print()
    print(f"done - processed {count} texture sets", flush=True)


if __name__ == "__main__":
    main()
