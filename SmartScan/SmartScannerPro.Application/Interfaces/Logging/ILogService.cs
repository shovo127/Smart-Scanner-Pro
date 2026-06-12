namespace SmartScannerPro.Application.Interfaces.Logging;

/// <summary>
/// Abstraction for logging across the application.
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Logs informational messages that highlight the progress of the application at coarse-grained level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">The arguments to format the message.</param>
    void LogInformation(string message, params object[] args);

    /// <summary>
    /// Logs potentially harmful situations.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">The arguments to format the message.</param>
    void LogWarning(string message, params object[] args);

    /// <summary>
    /// Logs error events that might still allow the application to continue running.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="args">The arguments to format the message.</param>
    void LogError(Exception exception, string message, params object[] args);

    /// <summary>
    /// Logs severe error events that will presumably lead the application to abort.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="args">The arguments to format the message.</param>
    void LogFatal(Exception exception, string message, params object[] args);

    /// <summary>
    /// Logs fine-grained informational events that are most useful to debug an application.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">The arguments to format the message.</param>
    void LogDebug(string message, params object[] args);
}
