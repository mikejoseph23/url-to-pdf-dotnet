using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using CommandLine;

namespace UrlToPdf.Console
{
    class Program
    {
        public class Options
        {
            [Option('u', "url", Required = true, HelpText = "The URL to be parsed")]
            public string Url { get; set; }

            [Option('s', "save-path", Required = true, HelpText = "The path to save the output file.")]
            public string SavePath { get; set; }

            [Option('s', "wait-for-selector", Required = false, HelpText = "Wait for css selector before rendering of PDF.")]
            public string WaitForSelector { get; set; }

            [Option('l', "landscape", Required = false, HelpText = "Output the PDF in landscape mode.")]
            public bool Landscape { get; set; }

            [Option('b', "print-background", Required = false, HelpText = "Print the background.")]
            public bool PrintBackground { get; set; }

            public Options()
            {
                PrintBackground = true;
            }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    Start(o).Wait();
                });
        }

        public static async Task Start(Options options)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var launchOptions = new LaunchOptions { Headless = true, IgnoreHTTPSErrors = true };
            var browser = await Puppeteer.LaunchAsync(launchOptions);

            var page = await browser.NewPageAsync();
            await page.GoToAsync(options.Url);
            if (!string.IsNullOrEmpty(options.WaitForSelector))
            {
                await page.WaitForSelectorAsync(options.WaitForSelector);
            }

            await page.EmulateMediaAsync(MediaType.Print);
            var pdfOptions = new PdfOptions
            {
                PrintBackground = options.PrintBackground,
                Landscape = options.Landscape
            };

            await page.PdfAsync(options.SavePath, pdfOptions);
        }
    }
}
