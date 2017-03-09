using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SharpAvi.Codecs;
using SharpAvi.Output;

namespace ImgToAvi
{
    /// <summary>
    /// Image to avi converter.
    /// </summary>
    internal class Converter
    {
        /// <summary>
        /// Do the conversion.
        /// </summary>
        /// <param name="settings">Conversion settings.</param>
        public static void Run(ISettings settings)
        {
            var imageFiles = GetImageSequence(settings);

            Process(settings, imageFiles);
        }

        /// <summary>
        /// Get ordered filenames to be "stitched".
        /// </summary>
        /// <param name="settings">Search settings.</param>
        /// <returns>Collection of files.</returns>
        private static IReadOnlyCollection<string> GetImageSequence(ISearchSettings settings)
        {
            return Directory.EnumerateFiles(settings.DirName, settings.Mask, SearchOption.TopDirectoryOnly)
                    .OrderBy(s => s)
                    .ToList();
        }

        private static void Process(IAviSettings aviSettings, IReadOnlyCollection<string> imageFiles)
        {
            if (imageFiles.Count == 0) return;

            using (var writer = new AviWriter(aviSettings.OutputAvi) { FramesPerSecond = aviSettings.FPS, EmitIndex1 = true })
            {
                IAviVideoStream stream = null;
                byte[] buffer = null;
                bool first = true;
                var rectangle = new Rectangle();

                foreach (var file in imageFiles)
                {
                    using (var bitmap = (Bitmap)Image.FromFile(file))
                    {
                        if (first)
                        {
                            first = false;

                            //stream = writer.AddUncompressedVideoStream(image.Width, image.Height);
                            stream = writer.AddMotionJpegVideoStream(bitmap.Width, bitmap.Height, quality: 90);
                            //stream = writer.AddMpeg4VideoStream(image.Width, image.Height, fps, quality: 70, codec: KnownFourCCs.Codecs.MicrosoftMpeg4V2, forceSingleThreadedAccess: true);

                            buffer = new byte[bitmap.Width * bitmap.Height * 4 /* four bytes per pixel */];
                            rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        }


                        var raw = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                        Marshal.Copy(raw.Scan0, buffer, 0, buffer.Length);
                        bitmap.UnlockBits(raw);

                        stream.WriteFrame(true, buffer, 0, buffer.Length);
                    }
                }
            }
        }

    }
}
