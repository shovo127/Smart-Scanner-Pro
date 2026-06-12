namespace SmartScannerPro.Scanner.Abstractions.Models.Jobs;

using System;

/// <summary>
/// Provides detailed statistics for a completed or failed scan job.
/// </summary>
public record ScanStatistics
{
    /// <summary>
    /// Gets the number of successfully scanned pages.
    /// </summary>
    public int PagesAcquired { get; init; }

    /// <summary>
    /// Gets the number of pages successfully processed.
    /// </summary>
    public int PagesProcessed { get; init; }

    /// <summary>
    /// Gets the total execution duration.
    /// </summary>
    public TimeSpan TotalDuration { get; init; }

    /// <summary>
    /// Gets the time spent physically acquiring images.
    /// </summary>
    public TimeSpan AcquisitionDuration { get; init; }

    /// <summary>
    /// Gets the time spent processing images (OCR, enhancement, etc.).
    /// </summary>
    public TimeSpan ProcessingDuration { get; init; }
}
