# Map Projector Studio

MPS is a suite of tools allowing manipulation of world map images.

The primary use is to convert maps to other map projections. It also has a number of parameters that affect map output.

The project is separated into 3 parts:

1. Core Library, can be compiled into a DLL and used to support map projection in other software
2. CLI, a command line interface for the core library that supports all the arguments that the core library supports
3. (Not Started) GUI, a visual application supporting all the functionality of the library, in addition to layers

**Current State of the code is It Works On My Computer(TM)** -- caveat emptor!

## Getting Started

Right now the only way to run this is to clone the repository and build the project. `MapProjectorCLI.exe` is what 
you'll use to do the projections.

Ideally you will start with an image file of a map already in an `Equirectangular` projection. Path that file in to 
the -f/--file parameter of the CLI.

If you have a map in a different projection like `Mercator`, you can use the cli to convert it to `Equirectangular` 
with the following parameters:

`-f path/to/your/file.png -o target/path/to/your/output_equirect_file.png --projection mercator --invert`

Then use `output_equirect_file.png` as your new file input for further processing.

## Options

*   --projection     (Default: LatLong) Target Projection (or Source Projection if Invert flag specified)
    * Supported Projections:
        * LatLong, Equirect, Equirectangular (aliases)
        * EqualArea
        * Sinusoidal
        * Sinusoidal2
        * Mollweide
        * Mercator
        * Cylindrical
        * Azimuthal
        * Orthographic
        * Rectilinear
        * Stereographic
        * Gnomonic
        * Perspective
        * Bonne
        * Hammer
*  -f, --file       Required. Source File Path
*  -o, --out        Required. Output File Path
*  --adjust         (Default: false) Set source image to adjusted width and height options before processing
*  -w, --width      Target Width
* -h, --height     Target Height
* --bg             Background Image File Path
* -i, --invert     Invert the specified operation
* --loop           (Default: 1) Number of Images to Output along given loop increments
    * --tiltinc        (Default: 0) Tilt Increment (Degrees)
    * --turninc        (Default: 0) Turn Increment (Degrees)
    * --latinc         (Default: 0) Latitude Increment (Degrees)
    * --longinc        (Default: 0) Longitude Increment (Degrees)
    * --xinc           (Default: 0) X (Horizontal) Increment (Pixels)
    * --yinc           (Default: 0) Y (Vertical) Increment (Pixels)
    * --zinc           (Default: 0) Z (Zoom) Increment (Pixels)
    * --dateinc        (Default: 0) Date Increment (Days)
    * --timeinc        (Default: 0) Time Increment (Hours)
* --tilt           (Default: 0) Rotation Around Center Point (Degrees)
* --turn           (Default: 0) Vertical Rotation (Degrees)
* --rotate         (Default: 0) Rotate in 2D plane (Degrees)
* --lat            (Default: 0) Latitude of Center (Degrees)
* --lon            (Default: 0) Longitude of Center (Degrees)
* --scale          (Default: 1) Output Scale (Percent, 1 = 100%)
* --radius         (Default: 0, unused) Radius around center point that the output is rendered
* --xoff           (Default: 0) X Offset
* --yoff           (Default: 0) Y Offset
* --bgColor        Background color R,G,B (0-255)
* -a               (Default: 1) Only relevant to Sinusoidal2 projection
* --aw             (Default: 0.349065850398866) Only relevant to Perspective projection
* -x               (Default: 8) X dimension of viewing position, measured in planet radii
* -y               (Default: 0) Y dimension of viewing position, measured in planet radii
* -z               (Default: 0) Z dimension of viewing position, measured in planet radii
* --ox             (Default: 1) X dimension of Oblateness, measured in planet radii
* --oy             (Default: 1) Y dimension of Oblateness, measured in planet radii
* --oz             (Default: 1.1) Z dimension of Oblateness, measured in planet radii
* --sun            (Default: false) Simulate the sun on the perspective view (date and time are relevant)
    * --time       (Default: 0) Hours from midnight, UTC, in decimal hours (so 4.5 is half past four in the morning).
    * --date       (Default: 0) Day number in year. 
* -p               (Default: 0) Reference parallel for projections that can use it: equalarea, sinusoidal, mollweide
* --conic          (Default: 1) Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic
* --conicr         (Default: 0) Conic Radius. Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic
* --widget         Comma separated list of map widgets to render: Grid, Analemma, TemporaryHours, LocalHours, Altitudes, Tropics, Dateline, Datetime
* --gridx          (Default: 30) X Spacing of Grid (Degrees) (use --widget grid)
* --gridy          (Default: 30) Y Spacing of Grid (Degrees) (use --widget grid)
* --gridoff        (Default: 0) Grid Angular Offset (use --widget grid)
* --gridcolor      Grid line color R,G,B (0-255) (use --widget grid)
* --widgetColor    Widget Color  R,G,B (0-255) (use --widget)
* --wlat           (Default: 0) Widget Origin Latitude (Radians)
* --wlon           (Default: 0) Widget Origin Longitude (Radians)
* --wday           (Default: 0) Widget Day (for Dateline and Datetime widgets)

## Goals

[*] Cross platform, including Windows
[*] Handling of arbitrary filetypes instead of only `ppm`
[*] The core is a library that can be included in any other project
[*] A separate CLI that has parity with the original MMPS software
[ ] GUI application allowing visual manipulation of maps and widgets, including eventually layers that can independently moved
[ ] Improved visualization for the various widgets like the grid and analemma
[ ] Parameterize the various hardcode planet data like Inclination

## History

This is a port and spiritual successor of MMPS (https://github.com/matthewarcus/mmps) by Mattew Warcus. 
The code is similar in some respects to MMPS, although updated in a variety of ways. 

This is not perfectly backward compatible, although it does support all the projection features that MMPS did 
(but not the features of the separate `stars` utility).

## License

Per the original MMPS codebase, this software is released under GPLv3, tldr; you can freely modify or distribute 
this as long as you make the source available and keep the license intact.