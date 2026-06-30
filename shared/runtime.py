from pathlib import Path
import sys


def find_toolkit_root(start_file: str | Path) -> Path:
    p = Path(start_file).resolve()
    for parent in [p.parent, *p.parents]:
        if (parent / "runtime").exists() and (parent / "apps").exists():
            return parent
    # app files live at Skyking Toolkit/apps/app name/file.py
    return p.parents[2]


TOOLKIT_ROOT = find_toolkit_root(__file__)
RUNTIME_DIR = TOOLKIT_ROOT / "runtime"
APPS_DIR = TOOLKIT_ROOT / "apps"
SHARED_DIR = TOOLKIT_ROOT / "shared"
PYTHON_EXE = RUNTIME_DIR / "python-embed" / "python.exe"
TEXCONV_EXE = RUNTIME_DIR / "texconv.exe"
MAGICK_EXE = RUNTIME_DIR / "magick.exe"


def require_file(path: Path, label: str) -> None:
    if not path.exists():
        raise FileNotFoundError(f"Missing {label}: {path}")


def check_runtime(needs_magick: bool = False, needs_texconv: bool = True) -> None:
    if needs_texconv:
        require_file(TEXCONV_EXE, "texconv.exe")
    if needs_magick:
        require_file(MAGICK_EXE, "magick.exe")
