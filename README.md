# Images to AVI converter

Generate AVI from image sequence.

## Usage

`> ImgToAvi.exe inputDir outputAvi [fps]`

Command line arguments:
* `inputDir` - directory with images. Images sorted alphabetically and merged to AVI.
* `outputAvi` - Pathname of the output AVI file
* `fps` - Frames per second. Optional, 25 by default.

## Limitations
* Only PNG files from top folder are processed.
* AVI encoded with M-JPEG codec.
