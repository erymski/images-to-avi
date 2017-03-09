using System.IO;

namespace ImgToAvi
{
    /// <summary>
    /// Settings for AVI generation.
    /// </summary>
    internal interface IAviSettings
    {
        /// <summary>
        /// Pathname of output AVI file.
        /// </summary>
        string OutputAvi { get; }

        /// <summary>
        /// Desired FPS.
        /// </summary>
        int FPS { get; }
    }

    /// <summary>
    /// Settings for image file search.
    /// </summary>
    internal interface ISearchSettings
    {
        /// <summary>
        /// Input directory with images.
        /// </summary>
        string DirName { get; }

        /// <summary>
        /// Search mask.
        /// </summary>
        string Mask { get; }
    }

    /// <summary>
    /// Combined settings.
    /// </summary>
    internal interface ISettings : IAviSettings, ISearchSettings
    {
    }

    internal class Settings : ISettings
    {
        private const int DefaultFPS = 25;
        private const string DefaultMask = "*.png";

        public string OutputAvi { get; set; }
        public int FPS { get; set; } = DefaultFPS;
        public string DirName { get; set; }
        public string Mask { get; set; } = DefaultMask; // TODO: pass mask as a parameter

        /// <summary>
        /// Validate input parameters.
        /// </summary>
        public void Validate()
        {
            if (!Directory.Exists(DirName))
                throw new ValidationException("Input dir is missing");
        }

        /// <summary>
        /// Load and validate input parameters.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The settings.</returns>
        public static ISettings FromArgs(string[] args)
        {
            if (args == null || args.Length < 2)
                throw new ValidationException("Usage: PngToAvi.exe inputDir outputAvi [fps]");

            var setting = new Settings
                            {
                                DirName = args[0],
                                OutputAvi = args[1]
                            };

            // extract optional FPS parameter
            int fps;
            if (args.Length > 2 && int.TryParse(args[2], out fps))
            {
                setting.FPS = fps;
            }

            setting.Validate();

            return setting;
        }
    }
}