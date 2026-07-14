namespace SmartScannerPro.Application.ScanWorkflow.Models;

/// <summary>
/// Represents the overall status of a completed scanning workflow.
/// </summary>
public enum WorkflowStatus
{
    /// <summary>
    /// The workflow completed successfully. All requested pages were acquired.
    /// </summary>
    Completed = 0,

    /// <summary>
    /// The workflow was cancelled by the user or by a cancellation token.
    /// Pages acquired before cancellation may be present in the result.
    /// </summary>
    Cancelled = 1,

    /// <summary>
    /// The workflow failed due to a hardware or driver error.
    /// </summary>
    Failed = 2,
}
