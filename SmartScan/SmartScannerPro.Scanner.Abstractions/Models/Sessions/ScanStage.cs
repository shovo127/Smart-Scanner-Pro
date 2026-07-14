namespace SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Defines the ordered stages reported by scanner operations.
/// </summary>
public enum ScanStage
{
    /// <summary>
    /// The scanner connection is being established.
    /// </summary>
    Connecting = 0,

    /// <summary>
    /// Scanner capabilities and settings are being negotiated.
    /// </summary>
    Negotiating = 1,

    /// <summary>
    /// The scanner is preparing paper handling and acquisition settings.
    /// </summary>
    Preparing = 2,

    /// <summary>
    /// The scanner is acquiring image data from the document.
    /// </summary>
    Scanning = 3,

    /// <summary>
    /// Acquired image data is being transferred to the host.
    /// </summary>
    Transferring = 4,

    /// <summary>
    /// The acquired image is being finalized for downstream processing.
    /// </summary>
    PostProcessing = 5,

    /// <summary>
    /// The scanner job is finalizing and releasing transient resources.
    /// </summary>
    Finalizing = 6,

    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    Completed = 7,
}
