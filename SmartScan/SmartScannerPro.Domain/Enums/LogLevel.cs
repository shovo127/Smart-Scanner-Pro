namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the severity of a log message.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Verbose diagnostic information.
    /// </summary>
    Trace = 0,

    /// <summary>
    /// Information useful for debugging.
    /// </summary>
    Debug = 1,

    /// <summary>
    /// General application flow information.
    /// </summary>
    Info = 2,

    /// <summary>
    /// Abnormal or unexpected events.
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Execution errors.
    /// </summary>
    Error = 4,

    /// <summary>
    /// Critical failures causing the application to crash.
    /// </summary>
    Fatal = 5,
}
