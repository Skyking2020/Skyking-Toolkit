import copy
import json
import sys
from pathlib import Path

SUPPORTED_EXTS = {".dds", ".png", ".tga", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff"}
REQUIRED_CHANNELS = {"diffuse", "normal", "height", "rmaos"}

APP_DIR = Path(__file__).resolve().parent
CONFIG_PATH = APP_DIR / "config.json"

DEFAULT_CONFIG = {
    "defaults": {
        "emissive": False,
        "parallax": True,
        "subsurface": False,
        "smooth_angle": 75,
        "glint": {
            "screen_space_scale": 0,
            "log_microfacet_density": 0,
            "microfacet_roughness": 0,
            "density_randomization": 0,
        },
        "specular_level": 0.02,
        "subsurface_color": [1, 1, 1],
        "roughness_scale": 1,
        "subsurface_opacity": 1,
        "displacement_scale": 0.4,
    },
    "keywords": {},
    "file_overrides": {},
}


def fail(message):
    print()
    print("ERROR:")
    print(message)
    print()
    sys.exit(1)


def load_config():
    if not CONFIG_PATH.exists():
        print(f"WARNING: Missing config.json at {CONFIG_PATH}; using built-in defaults.", flush=True)
        return copy.deepcopy(DEFAULT_CONFIG)

    with CONFIG_PATH.open("r", encoding="utf-8") as f:
        user_config = json.load(f)

    config = copy.deepcopy(DEFAULT_CONFIG)
    deep_update(config, user_config)
    return config


def deep_update(target, source):
    for key, value in source.items():
        if isinstance(value, dict) and isinstance(target.get(key), dict):
            deep_update(target[key], value)
        else:
            target[key] = value


def normalize_slashes(value):
    return str(value).replace("/", "\\")


def strip_known_suffix(stem):
    lower = stem.lower()
    suffixes = [
        ("_rmaos", "rmaos"),
        ("_n", "normal"),
        ("_p", "height"),
        ("_d", "diffuse"),
    ]
    for suffix, channel in suffixes:
        if lower.endswith(suffix):
            return stem[: -len(suffix)], channel
    return stem, "diffuse"


def texture_key(path, mod_root):
    rel = path.relative_to(mod_root)
    parts = list(rel.parts)

    if parts and parts[0].lower() == "textures":
        parts = parts[1:]

    if parts and parts[0].lower() == "pbr":
        parts = parts[1:]

    stem, channel = strip_known_suffix(Path(parts[-1]).stem)
    parts[-1] = stem
    key = normalize_slashes(Path(*parts))
    return key, channel


def scan_texture_sets(mod_root):
    sets = {}

    for path in mod_root.rglob("*"):
        if not path.is_file():
            continue
        if path.suffix.lower() not in SUPPORTED_EXTS:
            continue

        key, channel = texture_key(path, mod_root)
        sets.setdefault(key, {})[channel] = path

    complete = {}
    skipped = {}
    for key, channels in sets.items():
        missing = REQUIRED_CHANNELS - set(channels.keys())
        if missing:
            skipped[key] = sorted(missing)
        else:
            complete[key] = channels

    return complete, skipped


def setting_from_value(value):
    if isinstance(value, dict):
        return copy.deepcopy(value)
    return {"displacement_scale": value}


def settings_for_texture(texture_key_value, config):
    settings = copy.deepcopy(config.get("defaults", {}))
    lower_texture = texture_key_value.lower()
    file_name = lower_texture.split("\\")[-1]

    for keyword, value in config.get("keywords", {}).items():
        if str(keyword).lower() in lower_texture:
            deep_update(settings, setting_from_value(value))

    overrides = config.get("file_overrides", {})
    for override_key, value in overrides.items():
        normalized = normalize_slashes(str(override_key)).lower()
        if normalized == lower_texture or normalized == file_name:
            deep_update(settings, setting_from_value(value))

    return settings


def build_entry(texture_key_value, config):
    entry = {"texture": texture_key_value}
    deep_update(entry, settings_for_texture(texture_key_value, config))
    return entry


def output_path_for(mod_root, json_name):
    name = json_name.strip()
    if not name:
        name = mod_root.name

    if not name.lower().endswith(".json"):
        name += ".json"

    output_folder = mod_root / "PBRNIFPatcher"
    output_folder.mkdir(parents=True, exist_ok=True)

    return output_folder / name


def print_usage():
    print()
    print("Skyking PBR JSON Builder")
    print("Usage:")
    print("  python generate.py <mod_folder> <json_name>")
    print()


def main():
    if len(sys.argv) < 3:
        print_usage()
        fail("Missing arguments.")

    mod_root = Path(sys.argv[1]).resolve()
    json_name = sys.argv[2].strip()

    if not mod_root.exists():
        fail(f"Mod folder does not exist: {mod_root}")

    config = load_config()
    out_path = output_path_for(mod_root, json_name)

    print()
    print("=== Skyking PBR JSON Builder ===")
    print(f"Mod folder: {mod_root}")
    print(f"Output JSON: {out_path}")
    print(f"Config: {CONFIG_PATH}")
    print()

    texture_sets, skipped = scan_texture_sets(mod_root)
    total = len(texture_sets)
    print(f"__SKYKING_TOTAL__={total}", flush=True)

    entries = []
    for done, texture_name in enumerate(sorted(texture_sets.keys()), start=1):
        print(f"Adding {texture_name}", flush=True)
        entries.append(build_entry(texture_name, config))
        print(f"__SKYKING_PROGRESS__={done}/{total}", flush=True)

    out_path.parent.mkdir(parents=True, exist_ok=True)
    with out_path.open("w", encoding="utf-8") as f:
        json.dump(entries, f, indent=4)
        f.write("\n")

    if skipped:
        print()
        print(f"Skipped {len(skipped)} incomplete texture sets.", flush=True)
        for key, missing in sorted(skipped.items()):
            print(f"  skipped {key} - missing: {', '.join(missing)}", flush=True)

    print()
    print(f"done - wrote {len(entries)} texture sets", flush=True)


if __name__ == "__main__":
    main()
