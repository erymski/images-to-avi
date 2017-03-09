using System;

namespace ImgToAvi
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                ISettings settings = Settings.FromArgs(args);
                Converter.Run(settings);
            }
            catch (ValidationException ve)
            {
                Console.WriteLine(ve.Message);
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -2;
            }

            return 0;
        }
    }
}
