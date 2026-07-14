namespace SmartScannerPro.UI.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using SmartScannerPro.Application.ScanWorkflow.Models;
using SmartScannerPro.Application.ScanWorkflow.Notifications;
using SmartScannerPro.Application.ScanWorkflow.Services;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;

/// <summary>
/// Main workspace view model for coordinating the desktop scanning application.
/// Delegates all scanning orchestration to <see cref="ScanWorkflowService"/> and
/// reacts to MediatR notifications for real-time page and progress updates.
/// </summary>
public sealed partial class WorkspaceViewModel :
    ObservableObject,
    INotificationHandler<PageScannedNotification>,
    INotificationHandler<WorkflowProgressNotification>
{
    private readonly IScannerEngine scannerEngine;
    private readonly ScanWorkflowService workflowService;
    private CancellationTokenSource? scanCts;

    /// <summary>
    /// Gets or sets the collection of discovered scanners.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ScannerDescriptor> scanners = new();

    /// <summary>
    /// Gets or sets the selected scanner descriptor.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartScanCommand))]
    [NotifyCanExecuteChangedFor(nameof(StartDuplexScanCommand))]
    private ScannerDescriptor? selectedScanner;

    /// <summary>
    /// Gets or sets a value indicating whether scanners are being refreshed.
    /// </summary>
    [ObservableProperty]
    private bool isRefreshing;

    /// <summary>
    /// Gets or sets a value indicating whether a scanning job is active.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartScanCommand))]
    [NotifyCanExecuteChangedFor(nameof(StartDuplexScanCommand))]
    private bool isScanning;

    /// <summary>
    /// Gets or sets the status message.
    /// </summary>
    [ObservableProperty]
    private string statusMessage = "Ready";

    /// <summary>
    /// Gets or sets the scan progress percentage.
    /// </summary>
    [ObservableProperty]
    private int progressPercentage;

    /// <summary>
    /// Gets or sets the scan progress message.
    /// </summary>
    [ObservableProperty]
    private string progressMessage = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the progress bar is visible.
    /// </summary>
    [ObservableProperty]
    private bool showProgressBar;

    /// <summary>
    /// Gets or sets the selected page view model.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RotatePageCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeletePageCommand))]
    [NotifyCanExecuteChangedFor(nameof(RescanPageCommand))]
    private PageViewModel? selectedPage;

    /// <summary>
    /// Gets or sets the collection of scanned pages.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<PageViewModel> pages = new();

    /// <summary>
    /// Gets or sets the list of supported paper sizes.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> paperSizes = new() { "A4", "Letter", "Legal" };

    /// <summary>
    /// Gets or sets the selected paper size.
    /// </summary>
    [ObservableProperty]
    private string selectedPaperSize = "A4";

    /// <summary>
    /// Gets or sets the list of supported resolutions.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<int> resolutions = new() { 75, 100, 150, 200, 300, 600, 1200 };

    /// <summary>
    /// Gets or sets the selected resolution (DPI).
    /// </summary>
    [ObservableProperty]
    private int selectedResolution = 300;

    /// <summary>
    /// Gets or sets the list of supported color modes.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> colorModes = new() { "Color", "Grayscale", "BlackAndWhite" };

    /// <summary>
    /// Gets or sets the selected color mode.
    /// </summary>
    [ObservableProperty]
    private string selectedColorMode = "Color";

    /// <summary>
    /// Gets or sets the list of supported scanner sources.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> sources = new() { "Flatbed", "AdfFront" };

    /// <summary>
    /// Gets or sets the selected scanner source.
    /// </summary>
    [ObservableProperty]
    private string selectedSource = "Flatbed";

    /// <summary>
    /// Gets or sets the filename prefix pattern.
    /// </summary>
    [ObservableProperty]
    private string fileNamePattern = "Scan_####";

    /// <summary>
    /// Gets or sets the output folder path.
    /// </summary>
    [ObservableProperty]
    private string outputFolder;

    /// <summary>
    /// Gets or sets the preview zoom level.
    /// </summary>
    [ObservableProperty]
    private double zoomLevel = 1.0;

    /// <summary>
    /// Gets or sets the preview zoom mode.
    /// </summary>
    [ObservableProperty]
    private string zoomMode = "FitPage";

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
    /// </summary>
    /// <param name="scannerEngine">The scanner engine orchestrator.</param>
    /// <param name="workflowService">The scan workflow service.</param>
    public WorkspaceViewModel(IScannerEngine scannerEngine, ScanWorkflowService workflowService)
    {
        this.scannerEngine = scannerEngine ?? throw new ArgumentNullException(nameof(scannerEngine));
        this.workflowService = workflowService ?? throw new ArgumentNullException(nameof(workflowService));
        this.outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Scans");

        _ = this.RefreshScannersAsync();
    }

    private bool CanScan => this.SelectedScanner != null && !this.IsScanning;

    private bool HasSelectedPage => this.SelectedPage != null;

    // ─── MediatR Notification Handlers ───────────────────────────────────────

    /// <inheritdoc/>
    Task INotificationHandler<PageScannedNotification>.Handle(PageScannedNotification notification, CancellationToken cancellationToken)
    {
        // Dispatch to UI thread since ObservableCollection requires it
        Application.Current.Dispatcher.Invoke(() =>
        {
            this.AddPageFromWorkflow(notification.Page);
        });

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    Task INotificationHandler<WorkflowProgressNotification>.Handle(WorkflowProgressNotification notification, CancellationToken cancellationToken)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            this.ProgressPercentage = notification.Progress.Percentage;
            this.ProgressMessage = notification.Progress.Message;
            this.StatusMessage = MapStageToStatus(notification.Progress.Stage);
        });

        return Task.CompletedTask;
    }

    // ─── Commands ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Asynchronously refreshes the available scanner devices.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [RelayCommand]
    private async Task RefreshScannersAsync()
    {
        if (this.IsRefreshing)
        {
            return;
        }

        this.IsRefreshing = true;
        this.StatusMessage = "Discovering scanners...";

        try
        {
            await this.scannerEngine.InitializeAsync().ConfigureAwait(true);
            var request = new DiscoveryRequest
            {
                Timeout = TimeSpan.FromSeconds(5),
                IncludeOffline = false
            };

            var result = await this.scannerEngine.Discovery.DiscoverAsync(request).ConfigureAwait(true);

            this.Scanners.Clear();
            foreach (var scanner in result.Scanners)
            {
                this.Scanners.Add(scanner);
            }

            if (this.SelectedScanner == null && this.Scanners.Count > 0)
            {
                this.SelectedScanner = this.Scanners[0];
            }

            this.StatusMessage = "Ready";
        }
        catch (Exception ex)
        {
            this.StatusMessage = "Discovery failed";
            MessageBox.Show($"Failed to discover scanners: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            this.IsRefreshing = false;
        }
    }

    /// <summary>
    /// Starts a single-page or ADF scan workflow.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [RelayCommand(CanExecute = nameof(CanScan))]
    private async Task StartScanAsync()
    {
        if (this.SelectedScanner == null)
        {
            return;
        }

        var mode = this.SelectedSource.StartsWith("Adf", StringComparison.OrdinalIgnoreCase)
            ? WorkflowMode.Adf
            : WorkflowMode.Single;

        await this.RunWorkflowAsync(mode, flipCallback: null).ConfigureAwait(true);
    }

    /// <summary>
    /// Starts a Manual Duplex scan workflow.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [RelayCommand(CanExecute = nameof(CanScan))]
    private async Task StartDuplexScanAsync()
    {
        if (this.SelectedScanner == null)
        {
            return;
        }

        await this.RunWorkflowAsync(WorkflowMode.ManualDuplex, this.ShowFlipPromptAsync).ConfigureAwait(true);
    }

    /// <summary>
    /// Cancels the running scan.
    /// </summary>
    [RelayCommand]
    private void CancelScan()
    {
        this.scanCts?.Cancel();
        this.StatusMessage = "Cancelled";
    }

    /// <summary>
    /// Rotates the selected page 90 degrees right.
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelectedPage))]
    private void RotatePage()
    {
        if (this.SelectedPage != null)
        {
            this.SelectedPage.Rotation = (this.SelectedPage.Rotation + 90) % 360;
        }
    }

    /// <summary>
    /// Deletes the selected page from the queue.
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelectedPage))]
    private void DeletePage()
    {
        if (this.SelectedPage != null)
        {
            var page = this.SelectedPage;
            var index = this.Pages.IndexOf(page);
            this.Pages.Remove(page);

            try
            {
                if (File.Exists(page.ImagePath))
                {
                    File.Delete(page.ImagePath);
                }
            }
            catch
            {
                // Suppress file delete failure (e.g., file locked by image preview)
            }

            for (int i = 0; i < this.Pages.Count; i++)
            {
                this.Pages[i].PageNumber = i + 1;
            }

            if (this.Pages.Count > 0)
            {
                this.SelectedPage = this.Pages[Math.Min(index, this.Pages.Count - 1)];
            }
            else
            {
                this.SelectedPage = null;
            }
        }
    }

    /// <summary>
    /// Rescans the selected page.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [RelayCommand(CanExecute = nameof(HasSelectedPage))]
    private async Task RescanPageAsync()
    {
        if (this.SelectedPage == null || this.SelectedScanner == null)
        {
            return;
        }

        var pageToReplace = this.SelectedPage;
        await this.RunWorkflowAsync(WorkflowMode.Single, flipCallback: null, replacePageVm: pageToReplace).ConfigureAwait(true);
    }

    /// <summary>
    /// Zooms the preview image in.
    /// </summary>
    [RelayCommand]
    private void ZoomIn()
    {
        this.ZoomMode = "Custom";
        this.ZoomLevel = Math.Min(5.0, this.ZoomLevel + 0.1);
    }

    /// <summary>
    /// Zooms the preview image out.
    /// </summary>
    [RelayCommand]
    private void ZoomOut()
    {
        this.ZoomMode = "Custom";
        this.ZoomLevel = Math.Max(0.1, this.ZoomLevel - 0.1);
    }

    /// <summary>
    /// Fits the preview image to width.
    /// </summary>
    [RelayCommand]
    private void ZoomFitWidth()
    {
        this.ZoomMode = "FitWidth";
        this.ZoomLevel = 1.0;
    }

    /// <summary>
    /// Fits the preview image to page.
    /// </summary>
    [RelayCommand]
    private void ZoomFitPage()
    {
        this.ZoomMode = "FitPage";
        this.ZoomLevel = 1.0;
    }

    // ─── Workflow Execution ───────────────────────────────────────────────────

    private async Task RunWorkflowAsync(
        WorkflowMode mode,
        Func<CancellationToken, Task<bool>>? flipCallback,
        PageViewModel? replacePageVm = null)
    {
        if (this.SelectedScanner == null)
        {
            return;
        }

        this.IsScanning = true;
        this.ShowProgressBar = true;
        this.ProgressPercentage = 0;
        this.ProgressMessage = "Initializing scanner session...";
        this.StatusMessage = "Connecting";
        this.scanCts = new CancellationTokenSource();

        // When replacing a specific page (rescan), clear real-time additions for that slot
        // so the notification handler adds fresh content rather than appending.
        bool isRescan = replacePageVm != null;

        try
        {
            var workflowRequest = new WorkflowRequest
            {
                HardwareId = this.SelectedScanner.HardwareId,
                Mode = mode,
                ColorMode = this.SelectedColorMode,
                Resolution = this.SelectedResolution,
                PaperSize = this.SelectedPaperSize,
                Source = this.SelectedSource,
                MaxPages = mode == WorkflowMode.Single ? 1 : null,
                FlipPromptCallback = flipCallback,
            };

            // For rescan, disable real-time page notifications (we handle the result manually)
            WorkflowResult result;
            if (isRescan)
            {
                result = await this.workflowService.ExecuteWorkflowAsync(workflowRequest, this.scanCts.Token).ConfigureAwait(true);
                this.ApplyRescanResult(result, replacePageVm!);
            }
            else
            {
                // Pages are added incrementally via INotificationHandler<PageScannedNotification>
                result = await this.workflowService.ExecuteWorkflowAsync(workflowRequest, this.scanCts.Token).ConfigureAwait(true);
            }

            switch (result.Status)
            {
                case WorkflowStatus.Completed:
                    this.StatusMessage = "Completed";
                    this.ProgressMessage = $"Successfully scanned {result.TotalPagesScanned} page(s).";

                    if (!isRescan)
                    {
                        this.MovePagesToPermanentStorage(result);
                    }

                    break;

                case WorkflowStatus.Cancelled:
                    this.StatusMessage = "Cancelled";
                    this.ProgressMessage = "Scanning operation cancelled by user.";
                    break;

                case WorkflowStatus.Failed:
                    this.StatusMessage = "Error";
                    var friendlyError = GetFriendlyErrorMessage(result.Exception);
                    this.ProgressMessage = $"Scan failed: {friendlyError}";
                    MessageBox.Show($"Scanning failed: {friendlyError}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
        catch (OperationCanceledException)
        {
            this.StatusMessage = "Cancelled";
            this.ProgressMessage = "Scanning cancelled.";
        }
        catch (Exception ex)
        {
            this.StatusMessage = "Error";
            var friendlyError = GetFriendlyErrorMessage(ex);
            this.ProgressMessage = $"Failed to run workflow: {friendlyError}";
            MessageBox.Show($"Failed to run scan workflow: {friendlyError}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            this.IsScanning = false;
            this.ShowProgressBar = false;
            this.scanCts?.Dispose();
            this.scanCts = null;
        }
    }

    private void AddPageFromWorkflow(WorkflowPage workflowPage)
    {
        if (!Directory.Exists(this.OutputFolder))
        {
            Directory.CreateDirectory(this.OutputFolder);
        }

        var pageNumber = this.Pages.Count + 1;
        var formattedNum = pageNumber.ToString("D4");
        var finalFileName = this.FileNamePattern.Replace("####", formattedNum) + ".png";
        var finalPath = Path.Combine(this.OutputFolder, finalFileName);

        int duplicateSuffix = 1;
        while (File.Exists(finalPath))
        {
            var nameWithoutExt = Path.GetFileNameWithoutExtension(finalFileName);
            finalPath = Path.Combine(this.OutputFolder, $"{nameWithoutExt}_{duplicateSuffix}.png");
            duplicateSuffix++;
        }

        if (File.Exists(workflowPage.StagingFilePath))
        {
            File.Copy(workflowPage.StagingFilePath, finalPath, overwrite: true);
        }

        var pageVm = new PageViewModel(finalPath, pageNumber);
        this.Pages.Add(pageVm);
        this.SelectedPage = pageVm;
    }

    private void ApplyRescanResult(WorkflowResult result, PageViewModel pageToReplace)
    {
        if (result.Status != WorkflowStatus.Completed || result.Pages.Count == 0)
        {
            this.StatusMessage = "Error";
            this.ProgressMessage = "Rescan failed or cancelled.";
            return;
        }

        var newPage = result.Pages[0];
        if (File.Exists(newPage.StagingFilePath))
        {
            File.Copy(newPage.StagingFilePath, pageToReplace.ImagePath, overwrite: true);
            pageToReplace.RefreshThumbnail();

            var current = this.SelectedPage;
            this.SelectedPage = null;
            this.SelectedPage = current;
        }

        this.StatusMessage = "Completed";
        this.ProgressMessage = "Page rescanned successfully.";
    }

    private void MovePagesToPermanentStorage(WorkflowResult result)
    {
        // Pages added via INotificationHandler already moved to permanent storage.
        // This is a safety pass for any pages that may have been missed (e.g., in rescan mode).
        _ = result;
    }

    private async Task<bool> ShowFlipPromptAsync(CancellationToken cancellationToken)
    {
        // Must run on the UI thread
        bool shouldContinue = false;

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var result = MessageBox.Show(
                "Front side scan complete.\n\nPlease flip your document stack and place it back in the feeder.\n\nClick OK to scan the back side, or Cancel to stop.",
                "Manual Duplex — Flip Pages",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information);

            shouldContinue = result == MessageBoxResult.OK;
        });

        return shouldContinue;
    }

    // ─── Static Helpers ───────────────────────────────────────────────────────

    private static string MapStageToStatus(Scanner.Abstractions.Models.Sessions.ScanStage stage)
    {
        return stage switch
        {
            Scanner.Abstractions.Models.Sessions.ScanStage.Connecting => "Connecting",
            Scanner.Abstractions.Models.Sessions.ScanStage.Negotiating => "Connecting",
            Scanner.Abstractions.Models.Sessions.ScanStage.Preparing => "Connecting",
            Scanner.Abstractions.Models.Sessions.ScanStage.Scanning => "Scanning",
            Scanner.Abstractions.Models.Sessions.ScanStage.Transferring => "Transferring",
            Scanner.Abstractions.Models.Sessions.ScanStage.PostProcessing => "Transferring",
            Scanner.Abstractions.Models.Sessions.ScanStage.Finalizing => "Transferring",
            Scanner.Abstractions.Models.Sessions.ScanStage.Completed => "Completed",
            _ => "Scanning",
        };
    }

    private static string GetFriendlyErrorMessage(Exception? ex)
    {
        if (ex == null)
        {
            return "Unknown error.";
        }

        if (ex is System.Runtime.InteropServices.COMException comEx)
        {
            return (uint)comEx.HResult switch
            {
                0x80210015 => "The scanner is offline or not connected.",
                0x80210003 => "The document feeder is empty.",
                0x80210005 => "Scanner device is not found or offline.",
                0x80210001 => "General WIA device transfer error.",
                _ => $"WIA communication error ({comEx.Message.Trim()})"
            };
        }

        return ex.Message;
    }
}
