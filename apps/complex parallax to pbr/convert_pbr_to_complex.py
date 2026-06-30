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


def build_complex_m(diffuse, height, rmaos=None):
    h, w = diffuse.shape[:2]
    if height.shape[:2] != (h, w):
        height = cv2.resize(height, (w, h), interpolation=cv2.INTER_LINEAR)

    diff_rgb = diffuse[:, :, :3] if diffuse.ndim == 3 else cv2.cvtColor(diffuse, cv2.COLOR_GRAY2BGR)
    green = diff_rgb[:, :, 1].astype(np.float32)
    green = green + (65 - green.mean())
    green = np.clip(green, 0, 255).astype(np.uint8)

    red = np.zeros((h, w), dtype=np.uint8)
    blue = np.zeros((h, w), dtype=np.uint8)

    if rmaos is not None and rmaos.ndim == 3:
        if rmaos.shape[:2] != (h, w):
            rmaos = cv2.resize(rmaos, (w, h), interpolation=cv2.INTER_LINEAR)
        # RMAOS in OpenCV B,G,R,A view: blue/AO at channel 0, green/metal at 1, red/rough at 2.
        blue = rmaos[:, :, 1] if rmaos.shape[2] >= 2 else blue
        red = rmaos[:, :, 1] if rmaos.shape[2] >= 2 else red

    out = np.zeros((h, w, 4), dtype=np.uint8)
    out[:, :, 0] = blue
    out[:, :, 1] = green
    out[:, :, 2] = red
    out[:, :, 3] = height
    return out


def process_set(parts, src_root, out_root, tmp):
    diffuse_path = parts.get("albedo")
    normal_path = parts.get("normal")
    height_path = parts.get("height")
    rmaos_path = parts.get("rmaos")
    if not diffuse_path or not normal_path or not height_path:
        return False

    rel_dir = diffuse_path.parent.relative_to(src_root)
    out_dir = out_root / rel_dir
    out_dir.mkdir(parents=True, exist_ok=True)
    name = diffuse_path.stem
    if name.lower().endswith("_d"):
        name = name[:-2]

    print(f"Processing {rel_dir / name}")

    copy_or_convert_to_dds(diffuse_path, out_dir / f"{name}.dds", "BC7_UNORM", True, tmp)
    copy_or_convert_to_dds(normal_path, out_dir / f"{name}_n.dds", "BC7_UNORM", False, tmp)
    copy_or_convert_to_dds(height_path, out_dir / f"{name}_p.dds", "BC4_UNORM", False, tmp)

    diffuse, _ = read_image(diffuse_path, tmp, cv2.IMREAD_UNCHANGED)
    height, _ = read_image(height_path, tmp, cv2.IMREAD_GRAYSCALE)
    rmaos, _ = read_image(rmaos_path, tmp, cv2.IMREAD_UNCHANGED) if rmaos_path else (None, None)
    if diffuse is None or height is None:
        print("  skipped - could not read diffuse or height")
        return False

    m_map = build_complex_m(diffuse, height, rmaos)
    m_png = tmp / f"{name}_m.png"
    write_png(m_png, m_map)
    png_to_dds(m_png, out_dir / f"{name}_m.dds", "BC7_UNORM", srgb=False)
    return True


def main():
    if len(sys.argv) < 3:
        print("Skyking PBR to Complex Parallax")
        print("Usage: python convert_pbr_to_complex.py <source_folder> <output_folder>")
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
    print("=== Skyking PBR to Complex Parallax ===")
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
