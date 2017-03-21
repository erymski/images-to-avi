using System.IO;
using CommandLine;
using CommandLine.Text;

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

        [Option('i', "input", HelpText = "Directory with images to process", Required = true)]
        public string DirName { get; set; }

        [Option('o', "output", HelpText = "Pathname of output AVI file", Required = true)]
        public string OutputAvi { get; set; }

        [Option('f',"fps", DefaultValue = DefaultFPS, HelpText = "Frames per second", Required = false)]
        public int FPS { get; set; } = DefaultFPS;

        public string Mask { get; set; } = DefaultMask; // TODO: pass mask as a parameter

        /// <summary>
        /// Validate input parameters.
        /// </summary>
        public void Validate()
        {
            if (!Directory.Exists(DirName))
                throw new ValidationException($"Input dir '{DirName}' is missing");
        }

        /// <summary>
        /// Load and validate input parameters.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The settings.</returns>
        public static ISettings FromArgs(string[] args)
        {
            var setting = new Settings();
            if (!Parser.Default.ParseArguments(args, setting))
                throw new ValidationException(HelpText.AutoBuild(setting).ToString());

            setting.Validate();
            return setting;
        }
    }
}