using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace UrlToPdf.Core;

public class PdfGenerator : IDisposable
{
    private readonly ILogger<PdfGenerator> _logger;
    private IBrowser? _browser;
    private bool _disposed;

    public PdfGenerator(ILogger<PdfGenerator> logger)
    {
        _logger = logger;
        _logger.LogDebug("PdfGenerator instance created");
    }

    public async Task<IBrowser> InitializeBrowserAsync(bool headless = true)
    {
        try
        {
            _logger.LogDebug("Starting browser initialization (headless: {Headless})", headless);
            
            var browserFetcher = new BrowserFetcher();
            var revisionInfo = await browserFetcher.DownloadAsync();
            _logger.LogDebug("Browser downloaded/verified. Browser: {Browser}, Platform: {Platform}", 
                revisionInfo?.Browser.ToString() ?? "unknown", 
                revisionInfo?.Platform.ToString() ?? "unknown");
            
            var launchOptions = new LaunchOptions 
            { 
                Headless = headless,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox", "--ignore-certificate-errors" },
                Timeout = 30000 // 30 seconds timeout
            };
            
            _browser = await Puppeteer.LaunchAsync(launchOptions);
            
            if (_browser == null)
            {
                throw new InvalidOperationException("Browser launch returned null");
            }
            
            _logger.LogDebug("Browser launched successfully. Process ID: {ProcessId}", _browser.Process?.Id ?? -1);
            return _browser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize browser");
            throw;
        }
    }

    public async Task GeneratePdfAsync(string url, string outputPath, PdfOptions? options = null)
    {
        try
        {
            _logger.LogInformation("Starting PDF generation for URL: {Url}, Output: {OutputPath}", url, outputPath);
            
            if (_browser == null)
            {
                _logger.LogDebug("Browser not initialized, initializing now");
                await InitializeBrowserAsync();
            }

            if (_browser == null)
            {
                throw new InvalidOperationException("Browser initialization failed - browser is still null");
            }

            await using var page = await _browser.NewPageAsync();
            
            // Set viewport
            await page.SetViewportAsync(new ViewPortOptions 
            { 
                Width = 1920, 
                Height = 1080 
            });
            
            _logger.LogDebug("Navigating to URL: {Url}", url);
            var navigationOptions = new NavigationOptions 
            { 
                WaitUntil = new[] { WaitUntilNavigation.Networkidle2 },
                Timeout = 60000 // 60 seconds
            };
            
            var response = await page.GoToAsync(url, navigationOptions);
            _logger.LogDebug("Navigation completed. Status: {Status}", response?.Status);
            
            if (options?.WaitForSelector != null)
            {
                _logger.LogDebug("Waiting for selector: {Selector}", options.WaitForSelector);
                await page.WaitForSelectorAsync(options.WaitForSelector, new WaitForSelectorOptions 
                { 
                    Timeout = 30000 
                });
            }

            await page.EmulateMediaTypeAsync(MediaType.Print);
            
            var pdfOptions = new PuppeteerSharp.PdfOptions
            {
                PrintBackground = options?.PrintBackground ?? true,
                Landscape = options?.Landscape ?? false,
                Format = options?.Format ?? PaperFormat.A4,
                MarginOptions = options?.MarginOptions ?? new MarginOptions 
                { 
                    Top = "20px", 
                    Bottom = "20px", 
                    Left = "20px", 
                    Right = "20px" 
                }
            };
            
            _logger.LogDebug("Generating PDF with options - Landscape: {Landscape}, PrintBackground: {PrintBackground}", 
                pdfOptions.Landscape, pdfOptions.PrintBackground);
            await page.PdfAsync(outputPath, pdfOptions);
            
            var fileInfo = new FileInfo(outputPath);
            _logger.LogInformation("PDF generated successfully. File size: {FileSize} bytes", fileInfo.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during PDF generation for URL: {Url}", url);
            throw;
        }
    }

    public async Task<byte[]> GeneratePdfBytesAsync(string url, PdfOptions? options = null)
    {
        try
        {
            _logger.LogDebug("Starting PDF byte generation for URL: {Url}", url);
            
            if (_browser == null)
            {
                _logger.LogDebug("Browser not initialized, initializing now");
                await InitializeBrowserAsync();
            }

            if (_browser == null)
            {
                throw new InvalidOperationException("Browser initialization failed - browser is still null");
            }

            await using var page = await _browser.NewPageAsync();
            
            await page.SetViewportAsync(new ViewPortOptions 
            { 
                Width = 1920, 
                Height = 1080 
            });
            
            _logger.LogDebug("Navigating to URL: {Url}", url);
            var response = await page.GoToAsync(url, new NavigationOptions 
            { 
                WaitUntil = new[] { WaitUntilNavigation.Networkidle2 },
                Timeout = 60000
            });
            _logger.LogDebug("Navigation completed with status: {Status}", response?.Status);
            
            if (options?.WaitForSelector != null)
            {
                _logger.LogDebug("Waiting for selector: {Selector}", options.WaitForSelector);
                await page.WaitForSelectorAsync(options.WaitForSelector, new WaitForSelectorOptions 
                { 
                    Timeout = 30000 
                });
            }

            await page.EmulateMediaTypeAsync(MediaType.Print);
            
            var pdfOptions = new PuppeteerSharp.PdfOptions
            {
                PrintBackground = options?.PrintBackground ?? true,
                Landscape = options?.Landscape ?? false,
                Format = options?.Format ?? PaperFormat.A4,
                MarginOptions = options?.MarginOptions ?? new MarginOptions 
                { 
                    Top = "20px", 
                    Bottom = "20px", 
                    Left = "20px", 
                    Right = "20px" 
                }
            };

            var pdfBytes = await page.PdfDataAsync(pdfOptions);
            _logger.LogDebug("PDF bytes generated successfully. Size: {Size} bytes", pdfBytes.Length);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during PDF byte generation for URL: {Url}", url);
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                try
                {
                    _logger.LogDebug("Disposing PdfGenerator");
                    _browser?.Dispose();
                    _logger.LogDebug("Browser disposed");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disposing browser");
                }
            }
            _disposed = true;
        }
    }
}

public class PdfOptions
{
    public string? WaitForSelector { get; set; }
    public bool PrintBackground { get; set; } = true;
    public bool Landscape { get; set; }
    public PaperFormat Format { get; set; } = PaperFormat.A4;
    public MarginOptions? MarginOptions { get; set; }
}