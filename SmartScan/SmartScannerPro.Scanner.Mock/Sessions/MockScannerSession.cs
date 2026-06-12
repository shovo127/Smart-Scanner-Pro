namespace SmartScannerPro.Scanner.Mock.Sessions;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Jobs;

/// <summary>
/// Represents an active mock scanning session. Manages the lifecycle of scan jobs,
/// exposes pause/resume/cancel controls, and maintains session statistics.
/// </summary>
public sealed class MockScannerSession : IScannerSession
{
    private readonly MockScannerDevice device;
    private readonly MockDeviceProfile profile;
    private readonly FailureSimulationOptions failureOptions;
    private readonly CancellationTokenSource sessionCts = new();

    // Shared semaphore: initially released (not paused). Jobs acquire then immediately
    // release when not paused. When paused, the semaphore count drops to 0, blocking jobs.
    private readonly SemaphoreSlim pauseSemaphore = new(1, 1);
    private readonly List<IScanJob> jobs = new();

    private readonly DateTimeOffset startTime;
    private ScanSessionState state = ScanSessionState.Created;
#pragma warning disable CS0649 // These fields are incremented by job-tracking code in future extensions
    private int pagesScanned;
    private int blankPagesSkipped;
#pragma warning restore CS0649

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerSession"/>.
    /// </summary>
    /// <param name="device">The simulated device to bind this session to.</param>
    /// <param name="profile">The device profile.</param>
    /// <param name="failureOptions">Failure simulation options.</param>
    public MockScannerSession(
        MockScannerDevice device,
        MockDeviceProfile profile,
        FailureSimulationOptions failureOptions)
    {
        this.device = device ?? throw new ArgumentNullException(nameof(device));
        this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
        this.failureOptions = failureOptions ?? throw new ArgumentNullException(nameof(failureOptions));
        this.Id = new SessionId(Guid.NewGuid());
        this.startTime = DateTimeOffset.UtcNow;
        this.state = ScanSessionState.Initializing;
    }

    /// <inheritdoc/>
    public SessionId Id { get; }

    /// <inheritdoc/>
    public ScanSessionState State => this.state;

    /// <inheritdoc/>
    public SessionStatistics Statistics => new()
    {
        StartTime = this.startTime,
        EndTime = this.state is ScanSessionState.Completed or ScanSessionState.Failed or ScanSessionState.Cancelled
            ? DateTimeOffset.UtcNow
            : null,
        PagesScanned = this.pagesScanned,
        BlankPagesSkipped = this.blankPagesSkipped,
    };

    /// <inheritdoc/>
    public IScanJob CreateJob(ScanJobOptions options)
    {
        this.sessionCts.Token.ThrowIfCancellationRequested();

        this.state = ScanSessionState.Acquiring;
        var job = new MockScanJob(
            options,
            this.device,
            this.profile,
            this.failureOptions,
            this.pauseSemaphore,
            totalPages: 1);

        this.jobs.Add(job);
        return job;
    }

    /// <inheritdoc/>
    public Task CancelAsync(CancellationToken cancellationToken = default)
    {
        this.sessionCts.Cancel();
        this.state = ScanSessionState.Cancelled;

        // Cancel all running jobs
        foreach (var job in this.jobs)
        {
            _ = job.CancelAsync();
        }

        // Release pause semaphore so blocked jobs can observe cancellation
        if (this.pauseSemaphore.CurrentCount == 0)
        {
            this.pauseSemaphore.Release();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task PauseAsync(CancellationToken cancellationToken = default)
    {
        if (this.state == ScanSessionState.Acquiring)
        {
            this.state = ScanSessionState.Paused;
            // Drain the semaphore to block job page loops
            if (this.pauseSemaphore.CurrentCount > 0)
            {
                this.pauseSemaphore.Wait(cancellationToken);
            }
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task ResumeAsync(CancellationToken cancellationToken = default)
    {
        if (this.state == ScanSessionState.Paused)
        {
            this.state = ScanSessionState.Acquiring;
            // Release semaphore so job page loops can continue
            if (this.pauseSemaphore.CurrentCount == 0)
            {
                this.pauseSemaphore.Release();
            }
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        this.sessionCts.Cancel();
        this.sessionCts.Dispose();
        this.pauseSemaphore.Dispose();
        return ValueTask.CompletedTask;
    }
}
