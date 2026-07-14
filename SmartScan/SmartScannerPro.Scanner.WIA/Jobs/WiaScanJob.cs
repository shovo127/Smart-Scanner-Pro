namespace SmartScannerPro.Scanner.WIA.Jobs;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.WIA.Devices;
using SmartScannerPro.Scanner.WIA.Helpers;

/// <summary>
/// Executes a discrete scan job using Windows Image Acquisition (WIA).
/// Uses late-bound dynamic COM calls to eliminate compile-time dependencies on MSBuild COM resolution.
/// </summary>
public sealed class WiaScanJob : IScanJob
{
    private const string WiaFormatPng = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
    private readonly WiaScannerDevice device;
    private readonly SemaphoreSlim pauseSemaphore;
    private readonly CancellationToken sessionToken;
    private volatile ScanJobStatus status = ScanJobStatus.Queued;
    private CancellationTokenSource? internalCts;

    /// <summary>
    /// Initializes a new instance of the <see cref="WiaScanJob"/> class.
    /// </summary>
    /// <param name="options">The job configuration options.</param>
    /// <param name="device">The WIA device wrapper to scan from.</param>
    /// <param name="pauseSemaphore">The shared pause semaphore.</param>
    /// <param name="sessionToken">The session-level cancellation token.</param>
    public WiaScanJob(
        ScanJobOptions options,
        WiaScannerDevice device,
        SemaphoreSlim pauseSemaphore,
        CancellationToken sessionToken)
    {
        this.Options = options ?? throw new ArgumentNullException(nameof(options));
        this.device = device ?? throw new ArgumentNullException(nameof(device));
        this.pauseSemaphore = pauseSemaphore ?? throw new ArgumentNullException(nameof(pauseSemaphore));
        this.sessionToken = sessionToken;
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
        this.internalCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.sessionToken);
        var token = this.internalCts.Token;
        this.status = ScanJobStatus.Running;

        var overallTimer = Stopwatch.StartNew();
        var acquisitionTimer = new Stopwatch();
        var processingTimer = new Stopwatch();
        int pagesAcquired = 0;

        try
        {
            token.ThrowIfCancellationRequested();

            // 1. Connecting Stage
            progress?.Report(new ScanProgress
            {
                Stage = ScanStage.Connecting,
                Percentage = 10,
                Message = "Connecting to WIA hardware...",
            });

            var scanResult = await StaThread.RunAsync(() =>
            {
                var deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
                if (deviceManagerType == null)
                {
                    throw new COMException("WIA is not installed or supported on this system.", unchecked((int)0x8021000b));
                }

                dynamic deviceManager = Activator.CreateInstance(deviceManagerType)!;
                dynamic deviceInfos = deviceManager.DeviceInfos;
                dynamic? wiaDevice = null;

                for (int i = 1; i <= deviceInfos.Count; i++)
                {
                    dynamic info = deviceInfos[i];
                    if (string.Equals(info.DeviceID, this.device.Descriptor.HardwareId, StringComparison.OrdinalIgnoreCase))
                    {
                        wiaDevice = info.Connect();
                        break;
                    }
                }

                if (wiaDevice == null)
                {
                    throw new COMException("WIA scanner device not found or offline.", unchecked((int)0x80210005));
                }

                try
                {
                    token.ThrowIfCancellationRequested();

                    // 2. Preparing Stage
                    progress?.Report(new ScanProgress
                    {
                        Stage = ScanStage.Preparing,
                        Percentage = 20,
                        Message = "Negotiating WIA capabilities...",
                    });

                    // Resolve capability settings
                    var source = this.device.GetCurrentValue<string>("document-source") ?? "Flatbed";
                    var duplex = this.device.GetCurrentValue<bool>("duplex");
                    var resolution = this.device.GetCurrentValue<int>("resolution");
                    if (resolution <= 0)
                    {
                        resolution = 300;
                    }

                    var colorMode = this.device.GetCurrentValue<string>("color-mode") ?? "Color";
                    var paperSize = this.device.GetCurrentValue<string>("paper-size") ?? "A4";

                    // Configure Document Handling Select (Paper Source)
                    // 1 represents FEEDER, 2 represents FLATBED, 4 represents DUPLEX
                    int handlingSelect = 2;
                    bool isAdf = source.StartsWith("Adf", StringComparison.OrdinalIgnoreCase);

                    if (isAdf)
                    {
                        handlingSelect = duplex ? (1 | 4) : 1;
                    }

                    SetDeviceProperty(wiaDevice.Properties, 3088, handlingSelect);

                    if (wiaDevice.Items.Count == 0)
                    {
                        throw new COMException("The WIA scanner device has no active items.", unchecked((int)0x80210001));
                    }

                    // Get primary scanning item (WIA 2.0 uses 1-based indexing)
                    dynamic wiaItem = wiaDevice.Items[1];

                    // Set Resolution (Horizontal/Vertical)
                    SetItemProperty(wiaItem.Properties, 6147, resolution); // WIA_IPS_XRES
                    SetItemProperty(wiaItem.Properties, 6148, resolution); // WIA_IPS_YRES

                    // Set Color Mode (DataType)
                    int wiaColorMode = colorMode switch
                    {
                        "BlackAndWhite" => 0,
                        "Grayscale" => 1,
                        _ => 2, // Color
                    };
                    SetItemProperty(wiaItem.Properties, 4103, wiaColorMode); // WIA_IPA_DATATYPE

                    // Compute extents for paper size to prevent overflows
                    double widthInches = 8.27; // A4
                    double heightInches = 11.69;

                    if (paperSize.Equals("Letter", StringComparison.OrdinalIgnoreCase))
                    {
                        widthInches = 8.5;
                        heightInches = 11.0;
                    }
                    else if (paperSize.Equals("Legal", StringComparison.OrdinalIgnoreCase))
                    {
                        widthInches = 8.5;
                        heightInches = 14.0;
                    }

                    int widthPixels = (int)(widthInches * resolution);
                    int heightPixels = (int)(heightInches * resolution);

                    SetItemProperty(wiaItem.Properties, 6149, 0); // WIA_IPS_XPOS
                    SetItemProperty(wiaItem.Properties, 6150, 0); // WIA_IPS_YPOS
                    SetItemProperty(wiaItem.Properties, 6151, widthPixels); // WIA_IPS_XEXTENT
                    SetItemProperty(wiaItem.Properties, 6152, heightPixels); // WIA_IPS_YEXTENT

                    // 3. Scanning Loop
                    var tempFiles = new List<string>();

                    while (true)
                    {
                        token.ThrowIfCancellationRequested();

                        // Block if session is paused
                        this.pauseSemaphore.Wait(token);
                        this.pauseSemaphore.Release();

                        progress?.Report(new ScanProgress
                        {
                            Stage = ScanStage.Scanning,
                            Percentage = 40 + Math.Min(30, pagesAcquired * 10),
                            CurrentPage = pagesAcquired + 1,
                            Message = $"Scanning page {pagesAcquired + 1}...",
                        });

                        acquisitionTimer.Start();
                        dynamic imageFile;
                        try
                        {
                            imageFile = wiaItem.Transfer(WiaFormatPng);
                        }
                        // 0x80210003 represents WIA_ERROR_PAPER_EMPTY
                        catch (COMException comEx) when ((uint)comEx.HResult == 0x80210003)
                        {
                            if (isAdf && pagesAcquired > 0)
                            {
                                // ADF has finished scanning all available pages
                                break;
                            }
                            throw;
                        }
                        finally
                        {
                            acquisitionTimer.Stop();
                        }

                        token.ThrowIfCancellationRequested();

                        progress?.Report(new ScanProgress
                        {
                            Stage = ScanStage.Transferring,
                            Percentage = 50 + Math.Min(30, pagesAcquired * 10),
                            CurrentPage = pagesAcquired + 1,
                            Message = $"Transferring page {pagesAcquired + 1} data...",
                        });

                        processingTimer.Start();
                        try
                        {
                            string tempFile = Path.Combine(Path.GetTempPath(), $"wia_page_{Guid.NewGuid():N}.png");
                            if (File.Exists(tempFile))
                            {
                                File.Delete(tempFile);
                            }

                            imageFile.SaveFile(tempFile);
                            tempFiles.Add(tempFile);
                            pagesAcquired++;
                        }
                        finally
                        {
                            processingTimer.Stop();
                            if (imageFile != null && Marshal.IsComObject(imageFile))
                            {
                                Marshal.ReleaseComObject(imageFile);
                            }
                        }

                        if (!isAdf)
                        {
                            break; // Flatbed only scans one page
                        }

                        // Check feeder status
                        bool hasMorePaper = false;
                        try
                        {
                            // 3087 is WIA_DPS_DOCUMENT_HANDLING_STATUS
                            var statusVal = GetDeviceProperty<int>(wiaDevice.Properties, 3087, 0);
                            hasMorePaper = (statusVal & 0x1) != 0; // FEED_READY
                        }
                        catch
                        {
                            hasMorePaper = true; // Query next page and handle empty exception
                        }

                        if (!hasMorePaper)
                        {
                            break;
                        }
                    }

                    // 4. Finalizing
                    progress?.Report(new ScanProgress
                    {
                        Stage = ScanStage.Finalizing,
                        Percentage = 90,
                        Message = "Cleaning up WIA resources...",
                    });

                    return tempFiles;
                }
                finally
                {
                    if (wiaDevice != null && Marshal.IsComObject(wiaDevice))
                    {
                        Marshal.ReleaseComObject(wiaDevice);
                    }
                }
            }).ConfigureAwait(false);

            overallTimer.Stop();
            this.status = ScanJobStatus.Completed;

            progress?.Report(new ScanProgress
            {
                Stage = ScanStage.Completed,
                Percentage = 100,
                Message = "Scanning completed successfully.",
            });

            return new ScanJobResult
            {
                Status = ScanJobStatus.Completed,
                ScannedFilePaths = scanResult.AsReadOnly(),
                Statistics = new ScanStatistics
                {
                    PagesAcquired = pagesAcquired,
                    PagesProcessed = pagesAcquired,
                    TotalDuration = overallTimer.Elapsed,
                    AcquisitionDuration = acquisitionTimer.Elapsed,
                    ProcessingDuration = processingTimer.Elapsed,
                },
            };
        }
        catch (OperationCanceledException)
        {
            this.status = ScanJobStatus.Cancelled;
            overallTimer.Stop();
            return new ScanJobResult
            {
                Status = ScanJobStatus.Cancelled,
                Statistics = new ScanStatistics
                {
                    PagesAcquired = pagesAcquired,
                    PagesProcessed = pagesAcquired,
                    TotalDuration = overallTimer.Elapsed,
                    AcquisitionDuration = acquisitionTimer.Elapsed,
                    ProcessingDuration = processingTimer.Elapsed,
                },
            };
        }
        catch (Exception ex)
        {
            this.status = ScanJobStatus.Failed;
            overallTimer.Stop();
            var reason = WiaErrorTranslator.Translate(ex);
            return new ScanJobResult
            {
                Status = ScanJobStatus.Failed,
                FailureReason = reason,
                Exception = ex,
                Statistics = new ScanStatistics
                {
                    PagesAcquired = pagesAcquired,
                    PagesProcessed = pagesAcquired,
                    TotalDuration = overallTimer.Elapsed,
                    AcquisitionDuration = acquisitionTimer.Elapsed,
                    ProcessingDuration = processingTimer.Elapsed,
                },
            };
        }
    }

    /// <inheritdoc/>
    public Task CancelAsync()
    {
        this.internalCts?.Cancel();
        this.status = ScanJobStatus.Cancelled;
        return Task.CompletedTask;
    }

    private static void SetDeviceProperty(dynamic properties, int propertyId, object value)
    {
        try
        {
            object? propObj = properties[propertyId];
            if (propObj != null)
            {
                dynamic property = propObj;
                property.Value = value;
            }
        }
        catch (Exception ex)
        {
            throw new COMException($"Failed to set WIA device property {propertyId}: {ex.Message}", ex);
        }
    }

    private static void SetItemProperty(dynamic properties, int propertyId, object value)
    {
        try
        {
            object? propObj = properties[propertyId];
            if (propObj != null)
            {
                dynamic property = propObj;
                property.Value = value;
            }
        }
        catch
        {
            // Fail silently to adapt to varying scanner driver support
        }
    }

    private static T GetDeviceProperty<T>(dynamic properties, int propertyId, T defaultValue)
    {
        try
        {
            object? propObj = properties[propertyId];
            if (propObj != null)
            {
                dynamic property = propObj;
                var val = property.Value;
                if (val is T typedVal)
                {
                    return typedVal;
                }
                if (val != null)
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }
            }
        }
        catch
        {
            // Fallback
        }
        return defaultValue;
    }
}
