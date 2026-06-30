from pathlib import Path
import shutil
import cv2
import numpy as np
from PIL import Image

from .runtime import TEXCONV_EXE, MAGICK_EXE
from .process import run_command

SUPPORTED_TEXTURE_EXTS = {".dds", ".png", ".tga", ".tif", ".tiff", ".jpg", ".jpeg", ".exr"}


def ensure_dir(path: Path) -> None:
    Path(path).mkdir(parents=True, exist_ok=True)


def is_texture(path: Path) -> bool:
    return Path(path).suffix.lower() in SUPPORTED_TEXTURE_EXTS


def texconv_to_png(src: Path, tmp_dir: Path) -> Path | None:
    src = Path(src)
    tmp_dir = Path(tmp_dir)
    ensure_dir(tmp_dir)
    before = set(tmp_dir.glob("*.png"))
    result = run_command([TEXCONV_EXE, "-ft", "png", "-o", tmp_dir, "-y", src])
    if result.returncode != 0:
        return None
    expected = tmp_dir / f"{src.stem}.png"
    if expected.exists():
        return expected
    after = set(tmp_dir.glob(f"{src.stem}*.png"))
    new_files = list(after - before)
    if new_files:
        return new_files[0]
    return next(tmp_dir.glob(f"{src.stem}*.png"), None)


def magick_to_png(src: Path, tmp_dir: Path) -> Path | None:
    src = Path(src)
    tmp_dir = Path(tmp_dir)
    ensure_dir(tmp_dir)
    out = tmp_dir / f"{src.stem}.png"
    result = run_command([MAGICK_EXE, src, "-auto-level", out])
    if result.returncode == 0 and out.exists():
        return out
    return None


def image_to_png(src: Path, tmp_dir: Path) -> Path | None:
    src = Path(src)
    if not src.exists():
        print(f"Missing texture: {src}")
        return None
    suffix = src.suffix.lower()
    if suffix == ".exr":
        return magick_to_png(src, tmp_dir)
    if suffix == ".dds":
        return texconv_to_png(src, tmp_dir)
    # Use ImageMagick for TIFF/TGA/JPG/PNG too because it is consistent and handles odd bit depths better.
    return magick_to_png(src, tmp_dir)


def read_image(src: Path, tmp_dir: Path, flags=cv2.IMREAD_UNCHANGED):
    png = image_to_png(src, tmp_dir)
    if png is None:
        return None, None
    img = cv2.imread(str(png), flags)
    if img is None:
        print(f"Could not read converted image: {png}")
    return img, png


def read_gray(src: Path, tmp_dir: Path):
    img, png = read_image(src, tmp_dir, cv2.IMREAD_GRAYSCALE)
    return img, png


def write_png(path: Path, img) -> Path:
    path = Path(path)
    ensure_dir(path.parent)
    ok = cv2.imwrite(str(path), img)
    if not ok:
        raise RuntimeError(f"Could not write PNG: {path}")
    return path


def png_to_dds(png: Path, out: Path, fmt: str, srgb: bool = False) -> bool:
    png = Path(png)
    out = Path(out)
    ensure_dir(out.parent)
    if not png.exists():
        print(f"Missing PNG, cannot convert: {png}")
        return False

    dds_format = fmt
    if srgb and not dds_format.upper().endswith("_SRGB"):
        dds_format = dds_format + "_SRGB"

    result = run_command([TEXCONV_EXE, "-ft", "dds", "-f", dds_format, "-o", out.parent, "-y", png])
    if result.returncode != 0:
        return False

    produced = out.parent / f"{png.stem}.dds"
    if not produced.exists():
        print(f"Expected DDS was not created: {produced}")
        return False

    if produced.resolve() == out.resolve():
        return True

    if out.exists():
        out.unlink()
    produced.replace(out)
    return True


def copy_or_convert_to_dds(src: Path, out: Path, fmt: str, srgb: bool, tmp_dir: Path) -> bool:
    src = Path(src)
    out = Path(out)
    ensure_dir(out.parent)
    if src.suffix.lower() == ".dds":
        # For original Skyrim textures, copying is safest: it preserves exact alpha, dimensions, and compression.
        shutil.copy2(src, out)
        return True
    png = image_to_png(src, tmp_dir)
    if png is None:
        return False
    return png_to_dds(png, out, fmt, srgb=srgb)


def resize_like(img, reference):
    h, w = reference.shape[:2]
    if img.shape[:2] == (h, w):
        return img
    return cv2.resize(img, (w, h), interpolation=cv2.INTER_LINEAR)


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
