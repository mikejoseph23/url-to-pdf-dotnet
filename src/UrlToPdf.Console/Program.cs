
using System;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using PuppeteerSharp.Mobile;

namespace UrlToPdf.Console
{
    class Program
    {
        public static async Task Start(string url, string savePath, string waitForSelector = "")
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var launchOptions = new LaunchOptions();
            launchOptions.Headless = true;
            var browser = await Puppeteer.LaunchAsync(launchOptions);

            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            if (!string.IsNullOrEmpty(waitForSelector))
            {
                await page.WaitForSelectorAsync(waitForSelector);
            }

            await page.EmulateMediaAsync(MediaType.Print);

            var pdfOptions = new PdfOptions { PrintBackground = true };
            await page.PdfAsync(savePath, pdfOptions);
        }

        static void Main(string[] args)
        {
            var url = args[0];
            var savePath = args[1];
            var waitForSelector = "";
            if (args.Length == 3) waitForSelector= args[2];
            Start(url, savePath, waitForSelector).Wait();
        }

        #region Works, but has to run to time-out

        //private static string GetNodeJsCode()
        //{
        //    var code = @"
        //        const puppeteer = require('puppeteer');

        //        async function printPDF () {
        //          const browser = await puppeteer.launch({ 
        //         headless: true,
        //         executablePath: './node_modules/puppeteer/.local-chromium/win64-662092/chrome-win/chrome.exe' 
        //          });

        //          const page = await browser.newPage();
        //          await page.goto(
        //         'https://app.wingmaninsurance.com/app/quotes/quote-letter-printable/73ba3fd1-efc9-4637-a010-ee2f1868c8e1',
        //         { 
        //                waitUntil: 'networkidle0' 
        //            }
        //          );
        //          const pdf = await page.pdf({
        //         format: 'A4',
        //         path: 'C:/Temp/PdfTest.pdf',
        //         printBackground: true
        //          });

        //          await browser.close();
        //          return pdf;
        //        }

        //    //return function (data, callback) {
        //    //    callback(null, 'Node.js welcomes ' + data);
        //    //}

        //    return printPDF;";


        //    return code;
        //}

        //public static async Task Start()
        //{
        //    var code = GetNodeJsCode();
        //    var func = Edge.Func(code);

        //    var timeout = 5000;
        //    var task = func.Invoke("test");

        //    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        //    if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
        //    {
        //        // task completed within timeout
        //        System.Console.WriteLine("Got here.");
        //    }
        //    else
        //    {
        //        System.Console.WriteLine("Got there.");
        //        // timeout logic
        //    }

        //}

        //static void Main(string[] args)
        //{
        //    Start().Wait();
        //}

        #endregion

        #region ORIGINAL TEST CODE

        // ---- ZOMBIE - ORIGINAL TEST CODE ----
        //public class Options
        //{
        //    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        //    public bool Verbose { get; set; }

        //    [Option('s', "savePath", Required = true, HelpText = "The file to save the output PDF.")]
        //    public string SavePath { get; set; }

        //    [Option('u', "url", Required = true, HelpText = "The URL to convert.")]
        //    public string Url { get; set; }
        //}

        //static void Main(string[] args)
        //{
        //    Parser.Default.ParseArguments<Options>(args)
        //        .WithParsed<Options>(o =>
        //        {
        //            var results = Core.UrlToPdf.Convert(o.Url, o.SavePath);

        //            if (o.Verbose)
        //            {
        //                System.Console.WriteLine($"Verbose output enabled.");
        //                System.Console.WriteLine("Results:");
        //                System.Console.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));
        //            }
        //            else
        //            {
        //                System.Console.WriteLine("Done! Total Time Taken: " + results.TotalSeconds);
        //            }
        //        });
        //}

        #endregion
    }
}
