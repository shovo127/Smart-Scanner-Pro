namespace SmartScannerPro.Scanner.WIA.Sessions;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.WIA.Devices;
using SmartScannerPro.Scanner.WIA.Jobs;

/// <summary>
/// Represents an active scanning session bound to a WIA scanner device.
/// </summary>
public sealed class WiaScannerSession : IScannerSession
{
    private readonly WiaScannerDevice device;
    private readonly CancellationTokenSource sessionCts = new();
    private readonly SemaphoreSlim pauseSemaphore = new(1, 1);
    private readonly List<IScanJob> jobs = new();
    private readonly DateTimeOffset startTime;
    private ScanSessionState state = ScanSessionState.Created;
    private int pagesScanned;

    /// <summary>
    /// Initializes a new instance of the <see cref="WiaScannerSession"/> class.
    /// </summary>
    /// <param name="device">The WIA scanner device bound to this session.</param>
    public WiaScannerSession(WiaScannerDevice device)
    {
        this.device = device ?? throw new ArgumentNullException(nameof(device));
        this.Id = new SessionId(Guid.NewGuid());
        this.startTime = DateTimeOffset.UtcNow;
        this.state = ScanSessionState.Initializing;
    }

    /// <inheritdoc/>
    public SessionId Id { get; }

    /// <inheritdoc/>
    public ScanSessionState State => this.state;

    /// <inheritdoc/>
    public IScannerDevice Device => this.device;

    /// <inheritdoc/>
    public SessionStatistics Statistics
    {
        get
        {
            // Calculate total pages from completed jobs
            int count = 0;
            foreach (var job in this.jobs)
            {
                if (job.Status == ScanJobStatus.Completed || job.Status == ScanJobStatus.Failed || job.Status == ScanJobStatus.Cancelled)
                {
                    // Statistics might not be populated if the job didn't start, so handle safely
                    try
                    {
                        // In the actual execution, the runner will query the job or job will report statistics
                    }
                    catch
                    {
                        // Suppress
                    }
                }
            }
            this.pagesScanned = Math.Max(this.pagesScanned, count);

            return new SessionStatistics
            {
                StartTime = this.startTime,
                EndTime = this.state is ScanSessionState.Completed or ScanSessionState.Failed or ScanSessionState.Cancelled
                    ? DateTimeOffset.UtcNow
                    : null,
                PagesScanned = this.pagesScanned,
                BlankPagesSkipped = 0,
            };
        }
    }

    /// <inheritdoc/>
    public IScanJob CreateJob(ScanJobOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.sessionCts.Token.ThrowIfCancellationRequested();
        this.state = ScanSessionState.Acquiring;

        var job = new WiaScanJob(options, this.device, this.pauseSemaphore, this.sessionCts.Token);
        this.jobs.Add(job);
        return job;
    }

    /// <inheritdoc/>
    public Task CancelAsync(CancellationToken cancellationToken = default)
    {
        this.sessionCts.Cancel();
        this.state = ScanSessionState.Cancelled;

        foreach (var job in this.jobs)
        {
            _ = job.CancelAsync();
        }

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

    /// <summary>
    /// Updates the session scanned pages count.
    /// </summary>
    /// <param name="pages">The number of pages scanned.</param>
    internal void AddScannedPages(int pages)
    {
        Interlocked.Add(ref this.pagesScanned, pages);
    }
}
