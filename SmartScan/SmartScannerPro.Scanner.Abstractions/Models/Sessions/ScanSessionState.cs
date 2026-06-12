namespace SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Specifies the current state of a scanning session.
/// </summary>
public enum ScanSessionState
{
    /// <summary>
    /// The session has been created but not started.
    /// </summary>
    Created = 0,

    /// <summary>
    /// The session is initializing the hardware.
    /// </summary>
    Initializing = 1,

    /// <summary>
    /// The session is actively acquiring images.
    /// </summary>
    Acquiring = 2,

    /// <summary>
    /// The session is processing acquired images.
    /// </summary>
    Processing = 3,

    /// <summary>
    /// The session is paused and waiting to be resumed.
    /// </summary>
    Paused = 4,

    /// <summary>
    /// The session completed successfully.
    /// </summary>
    Completed = 5,

    /// <summary>
    /// The session failed due to an error.
    /// </summary>
    Failed = 6,

    /// <summary>
    /// The session was cancelled by the user.
    /// </summary>
    Cancelled = 7
}
