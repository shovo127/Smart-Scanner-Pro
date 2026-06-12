namespace SmartScannerPro.Scanner.Abstractions.Models.Discovery;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents the result of a discovery operation.
/// </summary>
public record DiscoveryResult
{
    /// <summary>
    /// Gets the list of discovered scanners.
    /// </summary>
    public required IReadOnlyList<ScannerDescriptor> Scanners { get; init; }

    /// <summary>
    /// Gets the duration of the discovery operation.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Gets a value indicating whether the discovery timed out before completing.
    /// </summary>
    public bool TimedOut { get; init; }
}
