﻿using System;
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
    class Program
    {
        private const int DefaultFPS = 25;

        static int Main(string[] args)
        {
            var argsLength = args.Length;
            if (argsLength < 2)
            {
                Console.WriteLine("Usage: PngToAvi.exe inputDir outputAvi [fps]");
                return -1;
            }

            string inputDir = args[0];
            if (! Directory.Exists(inputDir))
            {
                Console.WriteLine("Input dir is missing");
                return -2;
            }

            string outputAvi = args[1];

            int fps;
            if (argsLength == 2 || !int.TryParse(args[2], out fps))
            {
                fps = DefaultFPS;
            }

            var imageFiles = Directory.EnumerateFiles(inputDir, "*.png", SearchOption.TopDirectoryOnly)
                                .OrderBy(s => s)
                                .ToList();

            Process(outputAvi, fps, imageFiles);

            return 0;
        }

        private static void Process(string outputAvi, int fps, IReadOnlyCollection<string> imageFiles)
        {
            if (imageFiles.Count == 0) return;

            using (var writer = new AviWriter(outputAvi) {FramesPerSecond = fps, EmitIndex1 = true})
            {
                IAviVideoStream stream = null;
                byte[] buffer = null;
                bool first = true;
                var rectangle = new Rectangle();

                foreach (var file in imageFiles)
                {
                    using (var bitmap = (Bitmap) Image.FromFile(file))
                    {
                        if (first)
                        {
                            first = false;

                            //stream = writer.AddUncompressedVideoStream(image.Width, image.Height);
                            stream = writer.AddMotionJpegVideoStream(bitmap.Width, bitmap.Height, quality: 90);
                            //stream = writer.AddMpeg4VideoStream(image.Width, image.Height, fps, quality: 70, codec: KnownFourCCs.Codecs.MicrosoftMpeg4V2, forceSingleThreadedAccess: true);

                            buffer = new byte[bitmap.Width * bitmap.Height * 4];
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
