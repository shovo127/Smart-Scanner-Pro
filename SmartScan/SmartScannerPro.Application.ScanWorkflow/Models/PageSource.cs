namespace SmartScannerPro.Application.ScanWorkflow.Models;

/// <summary>
/// Represents the origin of a scanned page within a workflow.
/// </summary>
public enum PageSource
{
    /// <summary>
    /// A single-side scan (flatbed, single ADF pass, or <see cref="WorkflowMode.Single"/>).
    /// </summary>
    Single = 0,

    /// <summary>
    /// The front side of a duplex page pair.
    /// </summary>
    FrontSide = 1,

    /// <summary>
    /// The back side of a duplex page pair.
    /// </summary>
    BackSide = 2,
}
