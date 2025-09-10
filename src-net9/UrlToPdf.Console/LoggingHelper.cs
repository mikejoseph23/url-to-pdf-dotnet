using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace UrlToPdf.Console;

public static class LoggingHelper
{
    public static ILoggerFactory CreateLoggerFactory(bool verbose = false)
    {
        var logLevel = verbose ? LogEventLevel.Debug : LogEventLevel.Information;
        
        // Ensure logs directory exists
        var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        Directory.CreateDirectory(logsDirectory);
        
        // Create log file name with timestamp
        var logFileName = $"urltopdf-{DateTime.Now:yyyyMMdd-HHmmss}.log";
        var logFilePath = Path.Combine(logsDirectory, logFileName);
        
        // Configure Serilog
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.File(
                logFilePath,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Debug,
                fileSizeLimitBytes: 10485760, // 10MB
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 5);

        Log.Logger = loggerConfiguration.CreateLogger();
        
        // Create LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger);
        });
        
        // Log startup information
        var logger = loggerFactory.CreateLogger("LoggingHelper");
        logger.LogInformation("Logging initialized. Log file: {LogFile}", logFilePath);
        logger.LogInformation("Log level: {LogLevel}", logLevel);
        
        return loggerFactory;
    }
    
    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
    }
}