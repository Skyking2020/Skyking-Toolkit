# Skyking Toolkit v1.0

A complete texture pipeline for Skyrim Special Edition and Community
Shaders.

Skyking Toolkit combines several standalone utilities into a single
application for creating Complex Parallax, Community Shaders PBR
textures, converting between formats, and generating JSON material
files.

Designed for Skyrim texture artists using Photoshop, Quixel Mixer,
Substance Painter, or existing texture mods.

Skyking Toolkit

Created by Skyking.

Please do not redistribute modified versions without permission.

------------------------------------------------------------------------

# Quick Start

1.  Launch **Skyking Toolkit.exe**
2.  Select the tool you want to use.
3.  Choose your source and output folders.
4.  Click **Generate**.
5.  Monitor progress in the live log window.
6.  Your completed textures will be written to the selected output
    folder.

------------------------------------------------------------------------

# Features

-   Generate Complex Parallax textures from legacy Skyrim texture sets.
-   Generate Community Shaders PBR textures from legacy Skyrim texture
    sets.
-   Build complete texture sets from Quixel Mixer or Substance Painter
    exports.
-   Convert PBR textures to Complex Parallax.
-   Convert Complex Parallax textures to PBR.
-   Automatically generate Community Shaders JSON files.
-   Live progress bars and logging.
-   Configurable processing through JSON configuration files.

------------------------------------------------------------------------

# Installation

1.  Extract the archive.
2.  Keep the folder structure intact.
3.  Launch **Skyking Toolkit.exe**.

No installation is required.

------------------------------------------------------------------------

# Tools

## Parallax Old Mods

Converts traditional Skyrim texture sets into:

-   Complex Parallax
-   Community Shaders PBR
-   Both simultaneously

Supports per material configuration through `config.json`.

## Build From Source

Builds texture sets directly from Quixel Mixer or Substance Painter
exports.

Outputs:

-   Community Shaders PBR
-   Complex Parallax
-   Both

Includes an option to flip the green channel of normal maps for software
that exports OpenGL normals.

## PBR ⇆ Complex Parallax

Converts existing texture sets between formats.

Useful for converting an entire mod without recreating textures from
scratch.

## JSON Generator

Scans a texture folder and automatically creates Community Shaders
material JSON files.

Supports:

-   Default settings
-   Category based overrides
-   Individual texture overrides

------------------------------------------------------------------------

# Naming Conventions

The toolkit groups textures into texture sets using their filenames.

Example:

``` text
farmhouse01.dds
farmhouse01_n.dds
farmhouse01_p.dds
farmhouse01_rmaos.dds
```

These four files are treated as one material.

## Supported Texture Names

### Diffuse

`farmhouse01.dds`

### Normal

`farmhouse01_n.dds`

### Height / Parallax

`farmhouse01_p.dds`

### RMAOS

`farmhouse01_rmaos.dds`

A complete PBR texture set consists of all four files.

Textures may be stored in any folder structure. The toolkit preserves
folder structure during processing.

Supported formats:

-   DDS
-   PNG
-   TGA
-   JPG
-   JPEG
-   BMP
-   TIF
-   TIFF

DDS is recommended for Skyrim.

------------------------------------------------------------------------

# Configuration Files

Several tools include a `config.json` file.

These files allow processing behavior to be customized without editing
the Python scripts.

## Default

Applies to every texture.

## Categories

Applies settings when a texture path contains a keyword.

Example:

-   wood
-   stone
-   snow

## Overrides

Applies settings to a specific texture name.

Example:

-   mountainslab01

## Exclude

Skips textures containing specific keywords.

Example:

-   window
-   fx

------------------------------------------------------------------------

# Parallax Generator Settings

## contrast_factor

Controls overall height map strength.

## blur_radius

Controls Gaussian blur applied to generated height maps.

Higher values create smoother transitions.

## clamp_low

Minimum height value.

## clamp_high

Maximum height value.

## red_value / green_value / blue_value

Base RGB values used for generated mask textures.

## red_brightness / green_brightness / blue_brightness

Brightness multipliers for each channel.

------------------------------------------------------------------------

# JSON Generator Configuration

The JSON Generator configuration controls values written into Community
Shaders material definitions.

Examples include:

-   displacement_scale
-   roughness_scale
-   specular_level
-   emissive
-   parallax
-   smooth_angle
-   glint settings

Keyword and file overrides work exactly like the Parallax Generator
configuration.

------------------------------------------------------------------------

# Folder Structure

``` text
Skyking Toolkit
│
├── Skyking Toolkit.exe
├── apps
├── runtime
├── shared
└── README.md
```

Do not move folders after extracting.

------------------------------------------------------------------------

# Troubleshooting

-   Ensure the folder structure remains intact.
-   Do not rename Python scripts or configuration files.
-   Verify required `config.json` files are present.
-   Check the log window for detailed processing information.

------------------------------------------------------------------------

# Credits

Created by Skyking.

Special thanks to the Skyrim modding community and the Community Shaders
developers for continuing to advance Skyrim graphics.

------------------------------------------------------------------------

# Version

**Skyking Toolkit v1.0**

Future versions may include additional presets, more conversion options,
a configuration editor, and user interface improvements.
