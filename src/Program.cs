using System;
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

            using (var writer = new AviWriter(outputAvi) { FramesPerSecond = fps, EmitIndex1 = true })
            {
                IAviVideoStream stream = null;
                byte[] buffer = null;
                bool first = true;
                var rectangle = new Rectangle();

                IEnumerable<string> files = Directory.EnumerateFiles(inputDir, "*.png", SearchOption.TopDirectoryOnly).OrderBy(s => s);

                foreach (var file in files)
                {
                    {
                        if (first)
                        {
                            first = false;

                            //stream = writer.AddUncompressedVideoStream(image.Width, image.Height);
                            //stream = writer.AddMpeg4VideoStream(image.Width, image.Height, fps, quality: 70, codec: KnownFourCCs.Codecs.MicrosoftMpeg4V2, forceSingleThreadedAccess: true);

                        }



                        stream.WriteFrame(true, buffer, 0, buffer.Length);
                    }
                }
            }

            return 0;
        }
    }
}
