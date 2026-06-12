namespace SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;

using System;

/// <summary>
/// Represents a snapshot of performance metrics at a specific point in time.
/// </summary>
public record PerformanceSnapshot
{
    /// <summary>
    /// Gets the timestamp of the snapshot.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the memory consumed by the scanner process/driver (in bytes).
    /// </summary>
    public long MemoryUsageBytes { get; init; }

    /// <summary>
    /// Gets the active number of handles held by the driver.
    /// </summary>
    public int HandleCount { get; init; }

    /// <summary>
    /// Gets the number of threads currently executing in the driver.
    /// </summary>
    public int ThreadCount { get; init; }
}
