namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Represents an active scanning session bound to a specific device.
/// </summary>
public interface IScannerSession : IAsyncDisposable
{
    /// <summary>
    /// Gets the unique session identifier.
    /// </summary>
    SessionId Id { get; }

    /// <summary>
    /// Gets the current state of the session.
    /// </summary>
    ScanSessionState State { get; }

    /// <summary>
    /// Gets the current session statistics.
    /// </summary>
    SessionStatistics Statistics { get; }

    /// <summary>
    /// Creates a new scan job within this session.
    /// </summary>
    /// <param name="options">The options for the new scan job.</param>
    /// <returns>A new scan job instance.</returns>
    IScanJob CreateJob(ScanJobOptions options);

    /// <summary>
    /// Cancels the session and all pending jobs.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Pauses the current session.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PauseAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Resumes a paused session.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ResumeAsync(CancellationToken cancellationToken = default);
}
