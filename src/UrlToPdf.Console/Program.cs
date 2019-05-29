using CommandLine;
using Newtonsoft.Json;

namespace UrlToPdf.Console
{
    class Program
    {
        public class Options
        {
            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }

            [Option('s', "savePath", Required = true, HelpText = "The file to save the output PDF.")]
            public string SavePath { get; set; }

            [Option('u', "url", Required = true, HelpText = "The URL to convert.")]
            public string Url { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    var results = Core.UrlToPdf.Convert(o.Url, o.SavePath);
                    
                    if (o.Verbose)
                    {
                        System.Console.WriteLine($"Verbose output enabled.");
                        System.Console.WriteLine("Results:");
                        System.Console.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));
                    }
                    else
                    {
                        System.Console.WriteLine("Done! Total Time Taken: " + results.TotalSeconds);
                    }
                });
        }
    }
}
