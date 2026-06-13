namespace SmartScannerPro.UI.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Main workspace view model for coordinating the desktop scanning application.
/// </summary>
public sealed partial class WorkspaceViewModel : ObservableObject
{
    private readonly IScannerEngine scannerEngine;
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
    public WorkspaceViewModel(IScannerEngine scannerEngine)
    {
        this.scannerEngine = scannerEngine ?? throw new ArgumentNullException(nameof(scannerEngine));
        this.outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Scans");

        // Automatically discover scanners on load
        _ = this.RefreshScannersAsync();
    }

    private bool CanScan => this.SelectedScanner != null && !this.IsScanning;

    private bool HasSelectedPage => this.SelectedPage != null;

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
    /// Starts the scanning job.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [RelayCommand(CanExecute = nameof(CanScan))]
    private async Task StartScanAsync()
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

        try
        {
            // Configure session options
            var sessionOptions = new ScanSessionOptions
            {
                HardwareId = this.SelectedScanner.HardwareId,
                Timeout = TimeSpan.FromMinutes(2)
            };

            await using var session = await this.scannerEngine.Factory.CreateSessionAsync(sessionOptions, this.scanCts.Token).ConfigureAwait(true);

            // Apply capability settings to the device inside the session
            await session.Device.Capabilities.SetCapabilityValueAsync("document-source", this.SelectedSource, this.scanCts.Token).ConfigureAwait(true);
            await session.Device.Capabilities.SetCapabilityValueAsync("color-mode", this.SelectedColorMode, this.scanCts.Token).ConfigureAwait(true);
            await session.Device.Capabilities.SetCapabilityValueAsync("resolution", this.SelectedResolution, this.scanCts.Token).ConfigureAwait(true);
            await session.Device.Capabilities.SetCapabilityValueAsync("paper-size", this.SelectedPaperSize, this.scanCts.Token).ConfigureAwait(true);

            // Create scan job options
            var jobOptions = new ScanJobOptions
            {
                SessionOptions = sessionOptions,
                IsBackgroundJob = false,
                PromptForMorePages = false
            };

            var job = session.CreateJob(jobOptions);

            var progressReporter = new Progress<ScanProgress>(p =>
            {
                this.ProgressPercentage = p.Percentage;
                this.ProgressMessage = p.Message;
                
                // Map stages to status messages
                this.StatusMessage = p.Stage switch
                {
                    ScanStage.Connecting => "Connecting",
                    ScanStage.Preparing => "Connecting",
                    ScanStage.Scanning => "Scanning",
                    ScanStage.Transferring => "Transferring",
                    ScanStage.Finalizing => "Transferring",
                    ScanStage.Completed => "Completed",
                    _ => "Scanning"
                };
            });

            var result = await job.ExecuteAsync(progressReporter, this.scanCts.Token).ConfigureAwait(true);

            if (result.Status == ScanJobStatus.Completed)
            {
                this.StatusMessage = "Completed";
                this.ProgressMessage = "Saving scanned pages...";
                
                // Save output folder
                if (!Directory.Exists(this.OutputFolder))
                {
                    Directory.CreateDirectory(this.OutputFolder);
                }

                // Process output file naming and move/copy pages to final output directory
                int startIndex = this.Pages.Count + 1;
                foreach (var tempPath in result.ScannedFilePaths)
                {
                    if (File.Exists(tempPath))
                    {
                        var formattedNum = startIndex.ToString("D4");
                        var finalFileName = this.FileNamePattern.Replace("####", formattedNum) + ".png";
                        var finalPath = Path.Combine(this.OutputFolder, finalFileName);

                        // Ensure filename is unique if duplicates exist
                        int duplicateSuffix = 1;
                        while (File.Exists(finalPath))
                        {
                            var filenameNoExt = Path.GetFileNameWithoutExtension(finalFileName);
                            finalPath = Path.Combine(this.OutputFolder, $"{filenameNoExt}_{duplicateSuffix}.png");
                            duplicateSuffix++;
                        }

                        File.Copy(tempPath, finalPath, true);

                        var pageVm = new PageViewModel(finalPath, startIndex);
                        this.Pages.Add(pageVm);
                        this.SelectedPage = pageVm;
                        startIndex++;
                    }
                }

                this.ProgressMessage = $"Successfully scanned {result.ScannedFilePaths.Count} pages.";
            }
            else if (result.Status == ScanJobStatus.Cancelled)
            {
                this.StatusMessage = "Cancelled";
                this.ProgressMessage = "Scanning operation cancelled by user.";
            }
            else if (result.Status == ScanJobStatus.Failed)
            {
                this.StatusMessage = "Error";
                var friendlyError = this.GetFriendlyErrorMessage(result.Exception);
                this.ProgressMessage = $"Scan failed: {friendlyError}";
                MessageBox.Show($"Scanning failed: {friendlyError}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            var friendlyError = this.GetFriendlyErrorMessage(ex);
            this.ProgressMessage = $"Failed to initialize session: {friendlyError}";
            MessageBox.Show($"Failed to run scan job: {friendlyError}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            this.IsScanning = false;
            this.ShowProgressBar = false;
            this.scanCts?.Dispose();
            this.scanCts = null;
        }
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

            // Try to delete file if it was temporary
            try
            {
                if (File.Exists(page.ImagePath))
                {
                    File.Delete(page.ImagePath);
                }
            }
            catch
            {
                // Suppress file delete failure (e.g. file locked by process)
            }

            // Re-order remaining pages
            for (int i = 0; i < this.Pages.Count; i++)
            {
                this.Pages[i].PageNumber = i + 1;
            }

            // Select next page in queue
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
        this.IsScanning = true;
        this.ShowProgressBar = true;
        this.ProgressPercentage = 0;
        this.ProgressMessage = "Rescanning page...";
        this.StatusMessage = "Connecting";

        this.scanCts = new CancellationTokenSource();

        try
        {
            var sessionOptions = new ScanSessionOptions
            {
                HardwareId = this.SelectedScanner.HardwareId,
                Timeout = TimeSpan.FromMinutes(2)
            };

            await using var session = await this.scannerEngine.Factory.CreateSessionAsync(sessionOptions, this.scanCts.Token).ConfigureAwait(true);

            // Apply capability settings to the device
            await session.Device.Capabilities.SetCapabilityValueAsync("document-source", this.SelectedSource, this.scanCts.Token).ConfigureAwait(true);
            await session.Device.Capabilities.SetCapabilityValueAsync("color-mode", this.SelectedColorMode, this.scanCts.Token).ConfigureAwait(true);
            await session.Device.Capabilities.SetCapabilityValueAsync("resolution", this.SelectedResolution, this.scanCts.Token).ConfigureAwait(true);
            await session.Device.Capabilities.SetCapabilityValueAsync("paper-size", this.SelectedPaperSize, this.scanCts.Token).ConfigureAwait(true);

            var jobOptions = new ScanJobOptions
            {
                SessionOptions = sessionOptions,
                IsBackgroundJob = false,
                PromptForMorePages = false
            };

            var job = session.CreateJob(jobOptions);
            var progressReporter = new Progress<ScanProgress>(p =>
            {
                this.ProgressPercentage = p.Percentage;
                this.ProgressMessage = p.Message;
                this.StatusMessage = p.Stage == ScanStage.Scanning ? "Scanning" : "Transferring";
            });

            var result = await job.ExecuteAsync(progressReporter, this.scanCts.Token).ConfigureAwait(true);

            if (result.Status == ScanJobStatus.Completed && result.ScannedFilePaths.Count > 0)
            {
                this.StatusMessage = "Completed";
                var newTempPath = result.ScannedFilePaths[0];

                if (File.Exists(newTempPath))
                {
                    // Copy new scan over the old image path to preserve location
                    File.Copy(newTempPath, pageToReplace.ImagePath, true);
                    pageToReplace.RefreshThumbnail();

                    // Trigger property change for Preview refresh
                    var currentSelected = this.SelectedPage;
                    this.SelectedPage = null;
                    this.SelectedPage = currentSelected;
                }

                this.ProgressMessage = "Page rescanned successfully.";
            }
            else
            {
                this.StatusMessage = "Error";
                this.ProgressMessage = "Rescan failed or cancelled.";
            }
        }
        catch (Exception ex)
        {
            this.StatusMessage = "Error";
            MessageBox.Show($"Failed to rescan page: {this.GetFriendlyErrorMessage(ex)}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            this.IsScanning = false;
            this.ShowProgressBar = false;
            this.scanCts?.Dispose();
            this.scanCts = null;
        }
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

    private string GetFriendlyErrorMessage(Exception? ex)
    {
        if (ex == null)
        {
            return "Unknown error.";
        }

        // Friendly message mapping, hiding any raw COM/HResult codes from general users
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
