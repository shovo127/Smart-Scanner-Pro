namespace SmartScannerPro.Application.Interfaces.Logging;

/// <summary>
/// Abstraction for logging across the application.
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Logs informational messages that highlight the progress of the application at coarse-grained level.
    /// </summary>
    void LogInformation(string message, params object[] args);

    /// <summary>
    /// Logs potentially harmful situations.
    /// </summary>
    void LogWarning(string message, params object[] args);

    /// <summary>
    /// Logs error events that might still allow the application to continue running.
    /// </summary>
    void LogError(Exception exception, string message, params object[] args);

    /// <summary>
    /// Logs severe error events that will presumably lead the application to abort.
    /// </summary>
    void LogFatal(Exception exception, string message, params object[] args);

    /// <summary>
    /// Logs fine-grained informational events that are most useful to debug an application.
    /// </summary>
    void LogDebug(string message, params object[] args);
}
