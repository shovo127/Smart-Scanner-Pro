namespace SmartScannerPro.Scanner.Abstractions.Models.Jobs;

/// <summary>
/// Specifies the current execution status of a scan job.
/// </summary>
public enum ScanJobStatus
{
    /// <summary>
    /// The job is queued for execution.
    /// </summary>
    Queued = 0,

    /// <summary>
    /// The job is actively running.
    /// </summary>
    Running = 1,

    /// <summary>
    /// The job has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The job failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The job was cancelled.
    /// </summary>
    Cancelled = 4
}
