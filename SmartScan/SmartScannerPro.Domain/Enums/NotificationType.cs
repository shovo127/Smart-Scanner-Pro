namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the type of user notification.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// An informational notification.
    /// </summary>
    Info = 0,

    /// <summary>
    /// A success notification.
    /// </summary>
    Success = 1,

    /// <summary>
    /// A warning notification.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// An error notification.
    /// </summary>
    Error = 3,
}
