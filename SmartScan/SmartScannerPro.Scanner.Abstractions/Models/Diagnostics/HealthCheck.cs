namespace SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;

using System;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Represents the result of a single hardware or driver health check.
/// </summary>
public record HealthCheck
{
    /// <summary>
    /// Gets the name of the component being checked.
    /// </summary>
    public required string ComponentName { get; init; }

    /// <summary>
    /// Gets the health status of the component.
    /// </summary>
    public DriverHealth Status { get; init; }

    /// <summary>
    /// Gets a descriptive message about the health status.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Gets the timestamp of when the check was performed.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
