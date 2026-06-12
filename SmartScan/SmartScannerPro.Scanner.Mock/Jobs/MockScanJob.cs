namespace SmartScannerPro.Scanner.Mock.Jobs;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Images;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Mock.Capabilities;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Simulation;

/// <summary>
/// Simulates the execution of a discrete scanning job, including hardware latency,
/// image generation, failure injection, progress reporting, and cancellation support.
/// </summary>
public sealed class MockScanJob : IScanJob
{
    private readonly MockScannerDevice device;
    private readonly MockDeviceProfile profile;
    private readonly FailureSimulationOptions failureOptions;
    private readonly MockImageGenerator imageGenerator;
    private readonly SemaphoreSlim pauseSemaphore;
    private readonly int totalPages;
    private volatile ScanJobStatus status = ScanJobStatus.Queued;
    private CancellationTokenSource? internalCts;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScanJob"/>.
    /// </summary>
    /// <param name="options">The job configuration options.</param>
    /// <param name="device">The simulated device to execute against.</param>
    /// <param name="profile">The device performance profile.</param>
    /// <param name="failureOptions">The failure simulation options.</param>
    /// <param name="pauseSemaphore">Shared semaphore used for session-level pause/resume control.</param>
    /// <param name="totalPages">Total number of pages to simulate. Defaults to 1.</param>
    public MockScanJob(
        ScanJobOptions options,
        MockScannerDevice device,
        MockDeviceProfile profile,
        FailureSimulationOptions failureOptions,
        SemaphoreSlim pauseSemaphore,
        int totalPages = 1)
    {
        this.Options = options ?? throw new ArgumentNullException(nameof(options));
        this.device = device ?? throw new ArgumentNullException(nameof(device));
        this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
        this.failureOptions = failureOptions ?? throw new ArgumentNullException(nameof(failureOptions));
        this.pauseSemaphore = pauseSemaphore ?? throw new ArgumentNullException(nameof(pauseSemaphore));
        this.totalPages = totalPages > 0 ? totalPages : 1;
        this.imageGenerator = new MockImageGenerator();
    }

    /// <inheritdoc/>
    public ScanJobOptions Options { get; }

    /// <inheritdoc/>
    public ScanJobStatus Status => this.status;

    /// <inheritdoc/>
    public async Task<ScanJobResult> ExecuteAsync(
        IProgress<ScanProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        this.internalCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var token = this.internalCts.Token;
        this.status = ScanJobStatus.Running;

        var overallTimer = Stopwatch.StartNew();
        var acquisitionTimer = new Stopwatch();
        var processingTimer = new Stopwatch();

        // ── Pre-scan failure check ─────────────────────────────────────────
        var preScanFailure = MockFailureSimulator.EvaluatePreScanFailures(this.failureOptions);
        if (preScanFailure != FailureReason.None)
        {
            this.status = ScanJobStatus.Failed;
            return MockFailureSimulator.CreateFailedResult(
                preScanFailure,
                BuildStatistics(0, 0, overallTimer.Elapsed, acquisitionTimer.Elapsed, processingTimer.Elapsed));
        }

        var perfProfile = MockPerformanceProfile.For(this.profile.PerformanceProfile);

        // ── Connect phase ──────────────────────────────────────────────────
        MockProgressSimulator.ReportConnecting(progress);
        try
        {
            await this.DelayWithJitter(perfProfile.ConnectDelay, perfProfile.JitterStdDev, token)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            this.status = ScanJobStatus.Cancelled;
            return this.BuildCancelledResult(0, overallTimer.Elapsed, acquisitionTimer.Elapsed, processingTimer.Elapsed);
        }

        var colorMode = this.ResolveColorMode();

        // ── Page scanning loop ─────────────────────────────────────────────
        int pagesAcquired = 0;
        for (int page = 1; page <= this.totalPages; page++)
        {
            token.ThrowIfCancellationRequested();

            // Respect session-level pause
            await this.pauseSemaphore.WaitAsync(token).ConfigureAwait(false);
            this.pauseSemaphore.Release();

            // Mid-scan failure check (per page)
            var midScanFailure = MockFailureSimulator.EvaluateMidScanFailures(this.failureOptions);
            if (midScanFailure != FailureReason.None)
            {
                this.status = ScanJobStatus.Failed;
                return MockFailureSimulator.CreateFailedResult(
                    midScanFailure,
                    BuildStatistics(pagesAcquired, 0, overallTimer.Elapsed, acquisitionTimer.Elapsed, processingTimer.Elapsed));
            }

            // Scanning phase
            MockProgressSimulator.ReportScanning(progress, page, this.totalPages);
            acquisitionTimer.Start();
            try
            {
                await this.DelayWithJitter(perfProfile.PerPageAcquisitionDelay, perfProfile.JitterStdDev, token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                this.status = ScanJobStatus.Cancelled;
                return this.BuildCancelledResult(pagesAcquired, overallTimer.Elapsed, acquisitionTimer.Elapsed, processingTimer.Elapsed);
            }

            acquisitionTimer.Stop();

            // Image generation
            processingTimer.Start();
            using var rawImage = await this.imageGenerator.GenerateAsync(
                page - 1,
                colorMode,
                resolutionDpi: 300,
                addNoise: false,
                addSkew: false,
                cancellationToken: token).ConfigureAwait(false);
            processingTimer.Stop();

            // Transfer phase
            MockProgressSimulator.ReportImageTransfer(progress, page, this.totalPages);
            try
            {
                await this.DelayWithJitter(perfProfile.PerPageTransferDelay, perfProfile.JitterStdDev, token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                this.status = ScanJobStatus.Cancelled;
                return this.BuildCancelledResult(pagesAcquired, overallTimer.Elapsed, acquisitionTimer.Elapsed, processingTimer.Elapsed);
            }

            pagesAcquired++;
        }

        // ── Finish phase ───────────────────────────────────────────────────
        MockProgressSimulator.ReportFinishing(progress);
        try
        {
            await this.DelayWithJitter(perfProfile.FinishDelay, perfProfile.JitterStdDev, token)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            this.status = ScanJobStatus.Cancelled;
            return this.BuildCancelledResult(pagesAcquired, overallTimer.Elapsed, acquisitionTimer.Elapsed, processingTimer.Elapsed);
        }

        overallTimer.Stop();
        this.status = ScanJobStatus.Completed;

        return new ScanJobResult
        {
            Status = ScanJobStatus.Completed,
            Statistics = BuildStatistics(
                pagesAcquired,
                0,
                overallTimer.Elapsed,
                acquisitionTimer.Elapsed,
                processingTimer.Elapsed),
        };
    }

    /// <inheritdoc/>
    public Task CancelAsync()
    {
        this.internalCts?.Cancel();
        this.status = ScanJobStatus.Cancelled;
        return Task.CompletedTask;
    }

    private ColorProfile ResolveColorMode()
    {
        var colorModeStr = this.device.GetCurrentValue<string>(MockCapabilityFactory.CapIdColorMode);
        if (Enum.TryParse<ColorProfile>(colorModeStr, ignoreCase: true, out var parsed))
        {
            return parsed;
        }

        return ColorProfile.Color;
    }

    private async Task DelayWithJitter(TimeSpan baseDelay, TimeSpan jitter, CancellationToken token)
    {
        if (jitter == TimeSpan.Zero || baseDelay == TimeSpan.Zero)
        {
            await Task.Delay(baseDelay, token).ConfigureAwait(false);
            return;
        }

        var jitterMs = (Random.Shared.NextDouble() * 2 - 1) * jitter.TotalMilliseconds;
        var actual = Math.Max(0, baseDelay.TotalMilliseconds + jitterMs);
        await Task.Delay(TimeSpan.FromMilliseconds(actual), token).ConfigureAwait(false);
    }

    private ScanJobResult BuildCancelledResult(int pagesAcquired, TimeSpan total, TimeSpan acquisition, TimeSpan processing)
    {
        return new ScanJobResult
        {
            Status = ScanJobStatus.Cancelled,
            Statistics = BuildStatistics(pagesAcquired, 0, total, acquisition, processing),
        };
    }

    private static ScanStatistics BuildStatistics(
        int pagesAcquired,
        int pagesProcessed,
        TimeSpan total,
        TimeSpan acquisition,
        TimeSpan processing)
    {
        return new ScanStatistics
        {
            PagesAcquired = pagesAcquired,
            PagesProcessed = pagesProcessed,
            TotalDuration = total,
            AcquisitionDuration = acquisition,
            ProcessingDuration = processing,
        };
    }
}
