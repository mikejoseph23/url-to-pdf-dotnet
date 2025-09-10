using CommandLine;
using Microsoft.Extensions.Logging;
using UrlToPdf.Core;

namespace UrlToPdf.Console;

class Program
{
    private static ILoggerFactory? _loggerFactory;
    private static ILogger<Program>? _logger;

    public class Options
    {
        [Option('u', "url", Required = true, HelpText = "The URL to be parsed")]
        public string Url { get; set; } = string.Empty;

        [Option('o', "output", Required = true, HelpText = "The path to save the output file.")]
        public string OutputPath { get; set; } = string.Empty;

        [Option('w', "wait-for-selector", Required = false, HelpText = "Wait for css selector before rendering of PDF.")]
        public string? WaitForSelector { get; set; }

        [Option('l', "landscape", Required = false, HelpText = "Output the PDF in landscape mode.")]
        public bool Landscape { get; set; }

        [Option('b', "print-background", Required = false, Default = true, HelpText = "Print the background.")]
        public bool PrintBackground { get; set; } = true;

        [Option("headless", Required = false, Default = true, HelpText = "Run browser in headless mode.")]
        public bool Headless { get; set; } = true;
        
        [Option('v', "verbose", Required = false, Default = false, HelpText = "Enable verbose logging.")]
        public bool Verbose { get; set; }
    }

    static async Task Main(string[] args)
    {
        PrintLogo();
        
        var result = Parser.Default.ParseArguments<Options>(args);
        
        await result.WithParsedAsync(async options =>
        {
            // Initialize logging
            _loggerFactory = LoggingHelper.CreateLoggerFactory(options.Verbose);
            _logger = _loggerFactory.CreateLogger<Program>();
            
            try
            {
                _logger.LogDebug("=== URL to PDF Converter Started ===");
                _logger.LogDebug("Arguments: {Arguments}", string.Join(" ", args));
                
                await GeneratePdfAsync(options);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Unhandled exception in Main");
                System.Console.Error.WriteLine($"✗ Fatal error: {ex.Message}");
                Environment.Exit(1);
            }
            finally
            {
                _logger?.LogDebug("=== URL to PDF Converter Finished ===");
                LoggingHelper.CloseAndFlush();
                _loggerFactory?.Dispose();
            }
        });
        
        result.WithNotParsed(errors =>
        {
            // Command line parsing failed
            Environment.Exit(1);
        });
    }

    private static void PrintLogo()
    {
        System.Console.WriteLine(@"
 _   _ ____  _       _          ____  ____  _____ 
| | | |  _ \| |     | |_ ___   |  _ \|  _ \|  ___|
| | | | |_) | |     | __/ _ \  | |_) | | | | |_   
| |_| |  _ <| |___  | || (_) | |  __/| |_| |  _|  
 \___/|_| \_\_____|  \__\___/  |_|   |____/|_|    
                                                   
URL to PDF Converter v2.0 (.NET 9)
");
    }

    private static async Task GeneratePdfAsync(Options options)
    {
        if (_logger == null || _loggerFactory == null)
        {
            throw new InvalidOperationException("Logger not initialized");
        }
        
        try
        {
            _logger.LogDebug("Starting PDF generation process");
            _logger.LogInformation("URL: {Url}", options.Url);
            _logger.LogInformation("Output: {OutputPath}", options.OutputPath);
            
            System.Console.WriteLine($"Converting {options.Url} to PDF...");
            
            // Create PdfGenerator with logger
            var generatorLogger = _loggerFactory.CreateLogger<PdfGenerator>();
            using var generator = new PdfGenerator(generatorLogger);
            
            await generator.InitializeBrowserAsync(options.Headless);
            
            var pdfOptions = new PdfOptions
            {
                WaitForSelector = options.WaitForSelector,
                PrintBackground = options.PrintBackground,
                Landscape = options.Landscape
            };
            
            _logger.LogDebug("Generating PDF...");
            
            await generator.GeneratePdfAsync(options.Url, options.OutputPath, pdfOptions);
            
            System.Console.WriteLine($"✓ PDF saved to: {options.OutputPath}");
            _logger.LogInformation("PDF generation completed successfully");
            
            // Verify file was created
            if (File.Exists(options.OutputPath))
            {
                var fileInfo = new FileInfo(options.OutputPath);
                _logger.LogInformation("Output file verified. Size: {FileSize} bytes", fileInfo.Length);
            }
            else
            {
                _logger.LogWarning("Output file not found after generation: {OutputPath}", options.OutputPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF");
            _logger.LogError("Exception Type: {ExceptionType}", ex.GetType().FullName);
            _logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);
            
            if (ex.InnerException != null)
            {
                _logger.LogError("Inner Exception: {InnerException}", ex.InnerException.Message);
                _logger.LogError("Inner Exception Type: {InnerExceptionType}", ex.InnerException.GetType().FullName);
                _logger.LogError("Inner Stack Trace: {InnerStackTrace}", ex.InnerException.StackTrace);
            }
            
            System.Console.Error.WriteLine($"✗ Error generating PDF: {ex.Message}");
            
            if (options.Verbose)
            {
                System.Console.Error.WriteLine($"Exception details: {ex}");
            }
            else
            {
                System.Console.Error.WriteLine("Run with -v or --verbose for detailed error information");
                System.Console.Error.WriteLine($"Check logs in: {Path.Combine(Directory.GetCurrentDirectory(), "logs")}");
            }
            
            Environment.Exit(1);
        }
    }
}