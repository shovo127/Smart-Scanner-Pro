namespace SmartScannerPro.Scanner.Abstractions.Models.Jobs;

/// <summary>
/// Specifies the reason why a scan job failed.
/// </summary>
public enum FailureReason
{
    /// <summary>
    /// No failure occurred.
    /// </summary>
    None = 0,

    /// <summary>
    /// The scanner was offline or disconnected.
    /// </summary>
    DeviceOffline = 1,

    /// <summary>
    /// A paper jam was detected.
    /// </summary>
    PaperJam = 2,

    /// <summary>
    /// The document feeder is out of paper.
    /// </summary>
    OutOfPaper = 3,

    /// <summary>
    /// The scanner cover is open.
    /// </summary>
    CoverOpen = 4,

    /// <summary>
    /// An unknown hardware error occurred.
    /// </summary>
    HardwareError = 5,

    /// <summary>
    /// The driver failed to initialize or execute.
    /// </summary>
    DriverError = 6,

    /// <summary>
    /// The job timed out.
    /// </summary>
    Timeout = 7,

    /// <summary>
    /// An unknown exception occurred.
    /// </summary>
    UnknownError = 99
}
