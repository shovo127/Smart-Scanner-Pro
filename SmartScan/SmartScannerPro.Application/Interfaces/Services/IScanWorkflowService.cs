namespace SmartScannerPro.Application.Interfaces.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Orchestrates scanning workflows and sessions.
/// </summary>
public interface IScanWorkflowService
{
    /// <summary>
    /// Starts a new scan session.
    /// </summary>
    /// <param name="scannerId">The scanner identifier.</param>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The newly created session identifier.</returns>
    Task<SessionId> StartSessionAsync(ScannerId scannerId, ProfileId profileId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels an active scan session.
    /// </summary>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelSessionAsync(SessionId sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes an active scan session.
    /// </summary>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The generated document identifier.</returns>
    Task<DocumentId> CompleteSessionAsync(SessionId sessionId, CancellationToken cancellationToken = default);
}
