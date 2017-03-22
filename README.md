# Images to AVI converter

Generate AVI from image sequence.

## Usage

Command line arguments:
```
  -i, --input         Required. Directory with images to process. Images sorted alphabetically and merged to AVI.
  -o, --output        Required. Pathname of output AVI file.
  -f, --fps           (Default: 25) Frames per second.
  -d, --delete-dir    (Default: False) Delete whole processing directory after AVI generated.
  --delete-images     (Default: False) Delete processed images after AVI generated.
```
### Sample
`> ImgToAvi.exe -i c:\temp\images -o c:\temp\combined.avi -f 30 -d`

## Limitations
* Only PNG files from top folder are processed.
* AVI encoded with M-JPEG codec.
