namespace SmartScannerPro.Scanner.Mock.Simulation;

using System;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Generates realistic multi-stage progress notifications for a mock scan operation.
/// Progress is divided into phases: Connecting (0-10%), Scanning (10-80%),
/// Image Transfer (80-95%), Finishing (95-100%).
/// </summary>
public static class MockProgressSimulator
{
    private const int ConnectingStart = 0;
    private const int ConnectingEnd = 10;
    private const int ScanningStart = 10;
    private const int ScanningEnd = 80;
    private const int TransferStart = 80;
    private const int TransferEnd = 95;
    private const int FinishingStart = 95;
    private const int FinishingEnd = 100;

    /// <summary>
    /// Reports the initial connecting phase (0–10%).
    /// </summary>
    /// <param name="progress">The progress reporter to notify.</param>
    public static void ReportConnecting(IProgress<ScanProgress>? progress)
    {
        progress?.Report(new ScanProgress
        {
            Percentage = ConnectingStart,
            CurrentPage = 0,
            Message = "Connecting to scanner...",
        });

        progress?.Report(new ScanProgress
        {
            Percentage = ConnectingEnd,
            CurrentPage = 0,
            Message = "Scanner ready.",
        });
    }

    /// <summary>
    /// Reports progress during the physical scanning of a single page.
    /// </summary>
    /// <param name="progress">The progress reporter to notify.</param>
    /// <param name="currentPage">The one-based index of the page currently being scanned.</param>
    /// <param name="totalPages">The total number of pages expected in this job.</param>
    public static void ReportScanning(IProgress<ScanProgress>? progress, int currentPage, int totalPages)
    {
        if (progress is null)
        {
            return;
        }

        var pageRatio = totalPages > 0 ? (double)(currentPage - 1) / totalPages : 0;
        var percentage = ScanningStart + (int)((ScanningEnd - ScanningStart) * pageRatio);

        progress.Report(new ScanProgress
        {
            Percentage = Math.Clamp(percentage, ScanningStart, ScanningEnd - 1),
            CurrentPage = currentPage,
            Message = $"Scanning page {currentPage} of {totalPages}...",
        });
    }

    /// <summary>
    /// Reports progress during image data transfer from the device to the host.
    /// </summary>
    /// <param name="progress">The progress reporter to notify.</param>
    /// <param name="currentPage">The one-based index of the page being transferred.</param>
    /// <param name="totalPages">The total number of pages expected in this job.</param>
    public static void ReportImageTransfer(IProgress<ScanProgress>? progress, int currentPage, int totalPages)
    {
        if (progress is null)
        {
            return;
        }

        var pageRatio = totalPages > 0 ? (double)currentPage / totalPages : 1.0;
        var percentage = TransferStart + (int)((TransferEnd - TransferStart) * pageRatio);

        progress.Report(new ScanProgress
        {
            Percentage = Math.Clamp(percentage, TransferStart, TransferEnd - 1),
            CurrentPage = currentPage,
            Message = $"Transferring page {currentPage} image data...",
        });
    }

    /// <summary>
    /// Reports the finishing phase (95–100%), indicating session teardown.
    /// </summary>
    /// <param name="progress">The progress reporter to notify.</param>
    public static void ReportFinishing(IProgress<ScanProgress>? progress)
    {
        progress?.Report(new ScanProgress
        {
            Percentage = FinishingStart,
            CurrentPage = 0,
            Message = "Finalising scan...",
        });

        progress?.Report(new ScanProgress
        {
            Percentage = FinishingEnd,
            CurrentPage = 0,
            Message = "Scan complete.",
        });
    }
}
