from pathlib import Path

NORMAL_SUFFIXES = ("_n", "_normal", "_norm")
PARALLAX_SUFFIXES = ("_p", "_height", "_displacement")
COMPLEX_M_SUFFIXES = ("_m", "_complex")
RMAOS_SUFFIXES = ("_rmaos", "_orm")
DATA_SUFFIXES = NORMAL_SUFFIXES + PARALLAX_SUFFIXES + COMPLEX_M_SUFFIXES + RMAOS_SUFFIXES + (
    "_s", "_spec", "_specular", "_g", "_glow", "_e", "_emissive", "_b", "_backlight",
    "_rough", "_roughness", "_metal", "_metallic", "_ao", "_ambientocclusion", "_alpha"
)


def strip_known_suffix(stem: str) -> str:
    low = stem.lower()
    for suffix in sorted(DATA_SUFFIXES, key=len, reverse=True):
        if low.endswith(suffix):
            return stem[: -len(suffix)]
    return stem


def classify_texture(path: Path) -> str:
    stem = Path(path).stem.lower()
    name = Path(path).name.lower()
    if stem.endswith(NORMAL_SUFFIXES) or "normal" in name:
        return "normal"
    if stem.endswith(COMPLEX_M_SUFFIXES):
        return "complex_m"
    if stem.endswith(PARALLAX_SUFFIXES) or "height" in name or "displacement" in name:
        return "height"
    if stem.endswith(RMAOS_SUFFIXES):
        return "rmaos"
    if "rough" in name:
        return "roughness"
    if "metal" in name:
        return "metalness"
    if "ao" in name or "ambientocclusion" in name:
        return "ao"
    if "spec" in name:
        return "specular"
    if "albedo" in name or "diffuse" in name or "basecolor" in name or "base_color" in name:
        return "albedo"
    if stem == strip_known_suffix(stem):
        return "albedo"
    return "unknown"


def scan_texture_sets(root: Path, supported_exts: set[str]) -> dict[str, dict[str, Path]]:
    root = Path(root)
    sets: dict[str, dict[str, Path]] = {}
    for p in root.rglob("*"):
        if not p.is_file() or p.suffix.lower() not in supported_exts:
            continue
        kind = classify_texture(p)
        if kind == "unknown":
            continue
        base = strip_known_suffix(p.stem)
        rel_dir = p.parent.relative_to(root)
        key = str(rel_dir / base).replace("\\", "/").lower()
        sets.setdefault(key, {})[kind] = p
    return sets
