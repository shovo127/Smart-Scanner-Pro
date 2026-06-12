namespace SmartScannerPro.Scanner.Abstractions.Models.Sessions;

using System;

/// <summary>
/// Contains statistics about a scan session.
/// </summary>
public record SessionStatistics
{
    /// <summary>
    /// Gets the time the session started.
    /// </summary>
    public DateTimeOffset StartTime { get; init; }

    /// <summary>
    /// Gets the time the session completed or failed.
    /// </summary>
    public DateTimeOffset? EndTime { get; init; }

    /// <summary>
    /// Gets the total number of pages scanned.
    /// </summary>
    public int PagesScanned { get; init; }

    /// <summary>
    /// Gets the total number of blank pages skipped (if blank page detection is enabled).
    /// </summary>
    public int BlankPagesSkipped { get; init; }

    /// <summary>
    /// Gets the total execution duration.
    /// </summary>
    public TimeSpan Duration => this.EndTime.HasValue ? this.EndTime.Value - this.StartTime : TimeSpan.Zero;
}
