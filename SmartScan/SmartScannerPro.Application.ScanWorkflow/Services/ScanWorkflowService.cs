namespace SmartScannerPro.Application.ScanWorkflow.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartScannerPro.Application.ScanWorkflow.Models;
using SmartScannerPro.Application.ScanWorkflow.Notifications;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Orchestrates the complete scanning lifecycle, including single-page, multi-page,
/// ADF, and manual duplex workflows. Publishes MediatR notifications for real-time
/// UI updates and supports cancellation, progress reporting, and structured error recovery.
/// </summary>
public sealed class ScanWorkflowService
{
    private const int MaxPageRetries = 2;
    private const string StagingFolderPrefix = "SmartScannerPro_Staging_";

    private readonly IScannerFactory scannerFactory;
    private readonly WorkflowThumbnailService thumbnailService;
    private readonly IMediator mediator;
    private readonly ILogger<ScanWorkflowService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScanWorkflowService"/> class.
    /// </summary>
    /// <param name="scannerFactory">The scanner factory for creating sessions.</param>
    /// <param name="thumbnailService">The thumbnail generation service.</param>
    /// <param name="mediator">The MediatR mediator for publishing notifications.</param>
    /// <param name="logger">The logger instance.</param>
    public ScanWorkflowService(
        IScannerFactory scannerFactory,
        WorkflowThumbnailService thumbnailService,
        IMediator mediator,
        ILogger<ScanWorkflowService> logger)
    {
        this.scannerFactory = scannerFactory ?? throw new ArgumentNullException(nameof(scannerFactory));
        this.thumbnailService = thumbnailService ?? throw new ArgumentNullException(nameof(thumbnailService));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Executes a complete scanning workflow based on the provided request parameters.
    /// </summary>
    /// <param name="request">The workflow configuration parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the workflow, including all scanned pages.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    public async Task<WorkflowResult> ExecuteWorkflowAsync(
        WorkflowRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Mode == WorkflowMode.ManualDuplex && request.FlipPromptCallback == null)
        {
            throw new InvalidOperationException(
                "WorkflowMode.ManualDuplex requires a non-null FlipPromptCallback in the WorkflowRequest.");
        }

        var context = new WorkflowContext { Request = request };
        var overallTimer = Stopwatch.StartNew();
        var stagingDir = CreateStagingDirectory(context.WorkflowId);

        this.logger.LogInformation(
            "Starting workflow {WorkflowId} | Mode={Mode} | Scanner={HardwareId}",
            context.WorkflowId, request.Mode, request.HardwareId);

        try
        {
            context.State = WorkflowState.Initializing;
            var sessionOptions = BuildSessionOptions(request);

            await using var session = await this.scannerFactory
                .CreateSessionAsync(sessionOptions, cancellationToken)
                .ConfigureAwait(false);

            // Apply all capability settings
            await this.ApplyCapabilitiesAsync(session, request, cancellationToken).ConfigureAwait(false);

            context.State = WorkflowState.Acquiring;

            switch (request.Mode)
            {
                case WorkflowMode.Single:
                    await this.ExecuteSingleScanAsync(context, session, stagingDir, cancellationToken).ConfigureAwait(false);
                    break;

                case WorkflowMode.Multipage:
                    await this.ExecuteMultipageScanAsync(context, session, stagingDir, cancellationToken).ConfigureAwait(false);
                    break;

                case WorkflowMode.Adf:
                    await this.ExecuteAdfScanAsync(context, session, stagingDir, cancellationToken).ConfigureAwait(false);
                    break;

                case WorkflowMode.ManualDuplex:
                    await this.ExecuteManualDuplexAsync(context, session, stagingDir, cancellationToken).ConfigureAwait(false);
                    break;

                default:
                    throw new NotSupportedException($"Workflow mode '{request.Mode}' is not supported.");
            }

            overallTimer.Stop();
            context.State = WorkflowState.Completed;

            var result = new WorkflowResult
            {
                Status = WorkflowStatus.Completed,
                Pages = context.Pages.AsReadOnly(),
                Duration = overallTimer.Elapsed,
            };

            this.logger.LogInformation(
                "Workflow {WorkflowId} completed successfully. Pages={Count} Duration={Duration}",
                context.WorkflowId, result.TotalPagesScanned, result.Duration);

            await this.mediator
                .Publish(new WorkflowCompletedNotification(result, context.WorkflowId), CancellationToken.None)
                .ConfigureAwait(false);

            return result;
        }
        catch (OperationCanceledException)
        {
            overallTimer.Stop();
            context.State = WorkflowState.Cancelled;

            this.logger.LogInformation(
                "Workflow {WorkflowId} cancelled after {Duration}.",
                context.WorkflowId, overallTimer.Elapsed);

            var cancelledResult = new WorkflowResult
            {
                Status = WorkflowStatus.Cancelled,
                Pages = context.Pages.AsReadOnly(),
                Duration = overallTimer.Elapsed,
            };

            await this.mediator
                .Publish(new WorkflowCompletedNotification(cancelledResult, context.WorkflowId), CancellationToken.None)
                .ConfigureAwait(false);

            return cancelledResult;
        }
        catch (Exception ex)
        {
            overallTimer.Stop();
            context.State = WorkflowState.Failed;

            this.logger.LogError(ex, "Workflow {WorkflowId} failed after {Duration}.", context.WorkflowId, overallTimer.Elapsed);

            var failedResult = new WorkflowResult
            {
                Status = WorkflowStatus.Failed,
                Pages = context.Pages.AsReadOnly(),
                Duration = overallTimer.Elapsed,
                FailureReason = TranslateException(ex),
                Exception = ex,
            };

            await this.mediator
                .Publish(new WorkflowCompletedNotification(failedResult, context.WorkflowId), CancellationToken.None)
                .ConfigureAwait(false);

            return failedResult;
        }
        finally
        {
            CleanStagingOnCancelOrFail(context, stagingDir);
        }
    }

    // ─── Strategy Implementations ────────────────────────────────────────────────

    private async Task ExecuteSingleScanAsync(
        WorkflowContext context,
        IScannerSession session,
        string stagingDir,
        CancellationToken cancellationToken)
    {
        this.logger.LogDebug("Workflow {Id}: ExecuteSingleScan", context.WorkflowId);
        var page = await this.AcquirePageAsync(context, session, stagingDir, pageNumber: 1, PageSource.Single, cancellationToken).ConfigureAwait(false);
        if (page != null)
        {
            context.Pages.Add(page);
            await this.PublishPageAsync(context, page, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task ExecuteMultipageScanAsync(
        WorkflowContext context,
        IScannerSession session,
        string stagingDir,
        CancellationToken cancellationToken)
    {
        this.logger.LogDebug("Workflow {Id}: ExecuteMultipageScan", context.WorkflowId);
        int pageNumber = 1;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (context.Request.MaxPages.HasValue && pageNumber > context.Request.MaxPages.Value)
            {
                break;
            }

            var page = await this.AcquirePageAsync(context, session, stagingDir, pageNumber, PageSource.Single, cancellationToken).ConfigureAwait(false);
            if (page == null)
            {
                break;
            }

            context.Pages.Add(page);
            await this.PublishPageAsync(context, page, cancellationToken).ConfigureAwait(false);
            pageNumber++;

            // For multi-page (flatbed) mode, we break after each page.
            // The caller controls how many pages to acquire by setting MaxPages.
            if (!context.Request.MaxPages.HasValue || pageNumber > context.Request.MaxPages.Value)
            {
                break;
            }
        }
    }

    private async Task ExecuteAdfScanAsync(
        WorkflowContext context,
        IScannerSession session,
        string stagingDir,
        CancellationToken cancellationToken)
    {
        this.logger.LogDebug("Workflow {Id}: ExecuteAdfScan", context.WorkflowId);

        // A single ADF job acquires all available pages in one shot
        var jobOptions = BuildJobOptions(context.Request, session);
        var job = session.CreateJob(jobOptions);
        int currentPage = context.Pages.Count + 1;

        var progress = new Progress<ScanProgress>(p =>
        {
            var notification = new WorkflowProgressNotification(p, currentPage, context.WorkflowId);
            _ = this.mediator.Publish(notification, CancellationToken.None);
        });

        var result = await job.ExecuteAsync(progress, cancellationToken).ConfigureAwait(false);

        if (result.Status == ScanJobStatus.Cancelled)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new OperationCanceledException();
        }

        if (result.Status == ScanJobStatus.Failed)
        {
            throw new InvalidOperationException(
                $"ADF scan job failed: {result.FailureReason}",
                result.Exception);
        }

        await this.ProcessJobResultAsync(context, result, stagingDir, PageSource.Single, cancellationToken).ConfigureAwait(false);
    }

    private async Task ExecuteManualDuplexAsync(
        WorkflowContext context,
        IScannerSession session,
        string stagingDir,
        CancellationToken cancellationToken)
    {
        this.logger.LogDebug("Workflow {Id}: ExecuteManualDuplex — scanning front side", context.WorkflowId);

        // Phase 1: Scan all front pages
        var frontPages = new List<WorkflowPage>();

        var frontJobOptions = BuildJobOptions(context.Request, session);
        var frontJob = session.CreateJob(frontJobOptions);
        int frontPageNumber = 1;

        var frontProgress = new Progress<ScanProgress>(p =>
        {
            var notification = new WorkflowProgressNotification(p, frontPageNumber, context.WorkflowId);
            _ = this.mediator.Publish(notification, CancellationToken.None);
        });

        var frontResult = await frontJob.ExecuteAsync(frontProgress, cancellationToken).ConfigureAwait(false);

        if (frontResult.Status == ScanJobStatus.Cancelled)
        {
            throw new OperationCanceledException();
        }

        if (frontResult.Status == ScanJobStatus.Failed)
        {
            throw new InvalidOperationException(
                $"Manual duplex front-side scan failed: {frontResult.FailureReason}",
                frontResult.Exception);
        }

        // Build front pages (not published yet — wait for interleave)
        int stagingBase = 0;
        foreach (var path in frontResult.ScannedFilePaths)
        {
            stagingBase++;
            var staged = await this.StageAndThumbnailAsync(path, stagingDir, stagingBase, cancellationToken).ConfigureAwait(false);
            frontPages.Add(new WorkflowPage
            {
                PageNumber = stagingBase,
                StagingFilePath = staged.StagingPath,
                ThumbnailPath = staged.ThumbnailPath,
                Source = PageSource.FrontSide,
                ScannedAt = DateTimeOffset.UtcNow,
            });
        }

        this.logger.LogInformation(
            "Workflow {Id}: Front side complete ({Count} pages). Awaiting flip confirmation.",
            context.WorkflowId, frontPages.Count);

        // Phase 2: Prompt user to flip pages
        context.State = WorkflowState.AwaitingFlip;
        var shouldContinue = await context.Request.FlipPromptCallback!(cancellationToken).ConfigureAwait(false);

        if (!shouldContinue)
        {
            this.logger.LogInformation("Workflow {Id}: User cancelled at flip prompt.", context.WorkflowId);
            throw new OperationCanceledException("User cancelled at the manual duplex flip prompt.");
        }

        context.State = WorkflowState.Acquiring;
        this.logger.LogDebug("Workflow {Id}: ExecuteManualDuplex — scanning back side", context.WorkflowId);

        // Phase 3: Scan all back pages
        var backJobOptions = BuildJobOptions(context.Request, session);
        var backJob = session.CreateJob(backJobOptions);
        int backPageNumber = frontPages.Count + 1;

        var backProgress = new Progress<ScanProgress>(p =>
        {
            var notification = new WorkflowProgressNotification(p, backPageNumber, context.WorkflowId);
            _ = this.mediator.Publish(notification, CancellationToken.None);
        });

        var backResult = await backJob.ExecuteAsync(backProgress, cancellationToken).ConfigureAwait(false);

        if (backResult.Status == ScanJobStatus.Cancelled)
        {
            throw new OperationCanceledException();
        }

        if (backResult.Status == ScanJobStatus.Failed)
        {
            throw new InvalidOperationException(
                $"Manual duplex back-side scan failed: {backResult.FailureReason}",
                backResult.Exception);
        }

        var backPages = new List<WorkflowPage>();
        int backStagingBase = stagingBase;
        foreach (var path in backResult.ScannedFilePaths)
        {
            backStagingBase++;
            var staged = await this.StageAndThumbnailAsync(path, stagingDir, backStagingBase, cancellationToken).ConfigureAwait(false);
            backPages.Add(new WorkflowPage
            {
                PageNumber = backStagingBase,
                StagingFilePath = staged.StagingPath,
                ThumbnailPath = staged.ThumbnailPath,
                Source = PageSource.BackSide,
                ScannedAt = DateTimeOffset.UtcNow,
            });
        }

        // Phase 4: Interleave front and back in correct reading order.
        // The back pages are scanned in reverse order (last page first when flipped),
        // so we reverse the back list before interleaving.
        var reversedBack = Enumerable.Reverse(backPages).ToList();
        int totalInterleaved = Math.Max(frontPages.Count, reversedBack.Count);
        int finalPageNumber = 1;

        for (int i = 0; i < totalInterleaved; i++)
        {
            if (i < frontPages.Count)
            {
                var front = frontPages[i] with { PageNumber = finalPageNumber++ };
                context.Pages.Add(front);
                await this.PublishPageAsync(context, front, cancellationToken).ConfigureAwait(false);
            }

            if (i < reversedBack.Count)
            {
                var back = reversedBack[i] with { PageNumber = finalPageNumber++ };
                context.Pages.Add(back);
                await this.PublishPageAsync(context, back, cancellationToken).ConfigureAwait(false);
            }
        }

        this.logger.LogInformation(
            "Workflow {Id}: Manual duplex complete. Total interleaved pages={Count}",
            context.WorkflowId, context.Pages.Count);
    }

    // ─── Page Acquisition Helpers ─────────────────────────────────────────────

    private async Task<WorkflowPage?> AcquirePageAsync(
        WorkflowContext context,
        IScannerSession session,
        string stagingDir,
        int pageNumber,
        PageSource source,
        CancellationToken cancellationToken)
    {
        int attempt = 0;
        while (attempt <= MaxPageRetries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            attempt++;

            var jobOptions = BuildJobOptions(context.Request, session);
            var job = session.CreateJob(jobOptions);
            int capturedPage = pageNumber;

            var progress = new Progress<ScanProgress>(p =>
            {
                var notification = new WorkflowProgressNotification(p, capturedPage, context.WorkflowId);
                _ = this.mediator.Publish(notification, CancellationToken.None);
            });

            ScanJobResult result;
            try
            {
                result = await job.ExecuteAsync(progress, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Workflow {Id}: Page {Page} acquisition threw on attempt {Attempt}.", context.WorkflowId, pageNumber, attempt);
                if (attempt > MaxPageRetries)
                {
                    throw;
                }

                context.RetryCount++;
                continue;
            }

            if (result.Status == ScanJobStatus.Cancelled)
            {
                throw new OperationCanceledException();
            }

            if (result.Status == ScanJobStatus.Failed)
            {
                this.logger.LogWarning(
                    "Workflow {Id}: Page {Page} job failed ({Reason}) on attempt {Attempt}.",
                    context.WorkflowId, pageNumber, result.FailureReason, attempt);

                if (attempt > MaxPageRetries)
                {
                    throw new InvalidOperationException(
                        $"Page {pageNumber} scan failed after {MaxPageRetries} retries: {result.FailureReason}",
                        result.Exception);
                }

                context.RetryCount++;
                continue;
            }

            if (result.ScannedFilePaths.Count == 0)
            {
                this.logger.LogWarning("Workflow {Id}: Page {Page} completed but produced no files.", context.WorkflowId, pageNumber);
                return null;
            }

            var sourcePath = result.ScannedFilePaths[0];
            var staged = await this.StageAndThumbnailAsync(sourcePath, stagingDir, pageNumber, cancellationToken).ConfigureAwait(false);

            return new WorkflowPage
            {
                PageNumber = pageNumber,
                StagingFilePath = staged.StagingPath,
                ThumbnailPath = staged.ThumbnailPath,
                Source = source,
                ScannedAt = DateTimeOffset.UtcNow,
            };
        }

        return null;
    }

    private async Task ProcessJobResultAsync(
        WorkflowContext context,
        ScanJobResult result,
        string stagingDir,
        PageSource source,
        CancellationToken cancellationToken)
    {
        int pageBase = context.Pages.Count;
        for (int i = 0; i < result.ScannedFilePaths.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int pageNumber = pageBase + i + 1;
            var staged = await this.StageAndThumbnailAsync(
                result.ScannedFilePaths[i], stagingDir, pageNumber, cancellationToken).ConfigureAwait(false);

            var page = new WorkflowPage
            {
                PageNumber = pageNumber,
                StagingFilePath = staged.StagingPath,
                ThumbnailPath = staged.ThumbnailPath,
                Source = source,
                ScannedAt = DateTimeOffset.UtcNow,
            };

            context.Pages.Add(page);
            await this.PublishPageAsync(context, page, cancellationToken).ConfigureAwait(false);
        }
    }

    // ─── File Helpers ─────────────────────────────────────────────────────────

    private async Task<(string StagingPath, string ThumbnailPath)> StageAndThumbnailAsync(
        string sourcePath,
        string stagingDir,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var stagingPath = Path.Combine(stagingDir, $"page_{pageNumber:D4}.png");
        var thumbnailPath = Path.Combine(stagingDir, $"thumb_{pageNumber:D4}.jpg");

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, stagingPath, overwrite: true);
        }

        await this.thumbnailService.GenerateAsync(stagingPath, thumbnailPath, cancellationToken).ConfigureAwait(false);

        return (stagingPath, thumbnailPath);
    }

    // ─── Notification Helpers ─────────────────────────────────────────────────

    private async Task PublishPageAsync(
        WorkflowContext context,
        WorkflowPage page,
        CancellationToken cancellationToken)
    {
        context.State = WorkflowState.Processing;
        await this.mediator
            .Publish(new PageScannedNotification(page, context.WorkflowId), cancellationToken)
            .ConfigureAwait(false);
        context.State = WorkflowState.Acquiring;
    }

    // ─── Static Helpers ───────────────────────────────────────────────────────

    private static string CreateStagingDirectory(Guid workflowId)
    {
        var stagingDir = Path.Combine(
            Path.GetTempPath(),
            $"{StagingFolderPrefix}{workflowId:N}");

        Directory.CreateDirectory(stagingDir);
        return stagingDir;
    }

    private static void CleanStagingOnCancelOrFail(WorkflowContext context, string stagingDir)
    {
        // Only clean up staging on failure or cancellation; successful runs leave pages for consumers
        if (context.State is not WorkflowState.Completed)
        {
            try
            {
                if (Directory.Exists(stagingDir))
                {
                    Directory.Delete(stagingDir, recursive: true);
                }
            }
            catch (Exception ex)
            {
                // Log but do not rethrow — cleanup failure must not mask the original error
                Console.Error.WriteLine($"[WorkflowCleanup] Failed to delete staging dir {stagingDir}: {ex.Message}");
            }
        }
    }

    private static ScanSessionOptions BuildSessionOptions(WorkflowRequest request)
    {
        return new ScanSessionOptions
        {
            HardwareId = request.HardwareId,
            Timeout = request.Timeout ?? TimeSpan.FromMinutes(5),
        };
    }

    private static ScanJobOptions BuildJobOptions(WorkflowRequest request, IScannerSession session)
    {
        return new ScanJobOptions
        {
            SessionOptions = new ScanSessionOptions
            {
                HardwareId = request.HardwareId,
                Timeout = request.Timeout ?? TimeSpan.FromMinutes(5),
            },
            IsBackgroundJob = false,
            PromptForMorePages = false,
        };
    }

    private static async Task ApplyCapabilitiesAsync(
        IScannerSession session,
        WorkflowRequest request,
        CancellationToken cancellationToken)
    {
        await session.Device.Capabilities
            .SetCapabilityValueAsync("document-source", request.Source, cancellationToken)
            .ConfigureAwait(false);

        await session.Device.Capabilities
            .SetCapabilityValueAsync("color-mode", request.ColorMode, cancellationToken)
            .ConfigureAwait(false);

        await session.Device.Capabilities
            .SetCapabilityValueAsync("resolution", request.Resolution, cancellationToken)
            .ConfigureAwait(false);

        await session.Device.Capabilities
            .SetCapabilityValueAsync("paper-size", request.PaperSize, cancellationToken)
            .ConfigureAwait(false);
    }

    private static FailureReason TranslateException(Exception ex)
    {
        return ex switch
        {
            OperationCanceledException => FailureReason.None,
            TimeoutException => FailureReason.Timeout,
            System.Runtime.InteropServices.COMException comEx
                when (uint)comEx.HResult == 0x80210015 => FailureReason.DeviceOffline,
            System.Runtime.InteropServices.COMException comEx
                when (uint)comEx.HResult == 0x80210003 => FailureReason.OutOfPaper,
            _ => FailureReason.UnknownError,
        };
    }
}
