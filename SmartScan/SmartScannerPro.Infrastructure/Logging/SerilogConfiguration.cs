namespace SmartScannerPro.Infrastructure.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

/// <summary>
/// Configures the Serilog logging pipeline.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Configures and creates the Serilog logger.
    /// </summary>
    /// <param name="logDirectory">The directory where logs will be stored.</param>
    /// <returns>The configured logger.</returns>
    public static Serilog.Core.Logger CreateLogger(string logDirectory)
    {
        var logFilePath = Path.Combine(logDirectory, "log-.json");

        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProperty("Application", "SmartScannerPro")
            .WriteTo.File(
                new CompactJsonFormatter(),
                logFilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                rollOnFileSizeLimit: true)
            .CreateLogger();
    }
}
