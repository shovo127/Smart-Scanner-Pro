namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Represents a discrete scanning job within a session.
/// </summary>
public interface IScanJob
{
    /// <summary>
    /// Gets the options configured for this job.
    /// </summary>
    ScanJobOptions Options { get; }

    /// <summary>
    /// Gets the current status of the scan job.
    /// </summary>
    ScanJobStatus Status { get; }

    /// <summary>
    /// Executes the scan job asynchronously.
    /// </summary>
    /// <param name="progress">An optional provider for progress updates.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the scan job.</returns>
    Task<ScanJobResult> ExecuteAsync(IProgress<ScanProgress>? progress = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the running job.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelAsync();
}
