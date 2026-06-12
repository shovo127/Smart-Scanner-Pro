namespace SmartScannerPro.Scanner.Abstractions.Models.Sessions;

using System;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides options for initializing and running a scan session.
/// </summary>
public record ScanSessionOptions
{
    /// <summary>
    /// Gets the target scanner hardware identifier.
    /// </summary>
    public required string HardwareId { get; init; }

    /// <summary>
    /// Gets the profile ID to use for this session.
    /// </summary>
    public ProfileId? ProfileId { get; init; }

    /// <summary>
    /// Gets a value indicating whether to enable continuous scanning.
    /// </summary>
    public bool ContinuousScanning { get; init; }

    /// <summary>
    /// Gets the maximum duration allowed for the session.
    /// </summary>
    public TimeSpan? Timeout { get; init; }
}
