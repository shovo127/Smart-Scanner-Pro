namespace SmartScannerPro.Application.ScanWorkflow.Models;

/// <summary>
/// Represents the internal state phase of a running workflow.
/// </summary>
internal enum WorkflowState
{
    /// <summary>
    /// The workflow has been created but not yet started.
    /// </summary>
    Created = 0,

    /// <summary>
    /// The workflow is validating parameters and connecting to the scanner.
    /// </summary>
    Initializing = 1,

    /// <summary>
    /// The workflow is actively acquiring pages.
    /// </summary>
    Acquiring = 2,

    /// <summary>
    /// The workflow is waiting for the user to confirm the page flip (Manual Duplex).
    /// </summary>
    AwaitingFlip = 3,

    /// <summary>
    /// The workflow is generating thumbnails and publishing notifications for acquired pages.
    /// </summary>
    Processing = 4,

    /// <summary>
    /// The workflow has completed successfully.
    /// </summary>
    Completed = 5,

    /// <summary>
    /// The workflow was cancelled.
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// The workflow has failed.
    /// </summary>
    Failed = 7,
}
