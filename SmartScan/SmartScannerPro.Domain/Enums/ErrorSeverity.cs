namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the severity of an error or issue.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Informational level.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning level. The operation may proceed, but issues exist.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error level. The operation failed but is recoverable.
    /// </summary>
    Error = 2,

    /// <summary>
    /// Critical level. The application cannot recover from this state.
    /// </summary>
    Critical = 3,
}
