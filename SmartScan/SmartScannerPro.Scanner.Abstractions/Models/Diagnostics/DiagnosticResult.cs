namespace SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;

using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Contains the complete results of a diagnostic run.
/// </summary>
public record DiagnosticResult
{
    /// <summary>
    /// Gets the overall health of the scanner subsystem.
    /// </summary>
    public DriverHealth OverallHealth { get; init; }

    /// <summary>
    /// Gets the results of individual component health checks.
    /// </summary>
    public IReadOnlyList<HealthCheck> Checks { get; init; } = new List<HealthCheck>();

    /// <summary>
    /// Gets the driver specific diagnostics.
    /// </summary>
    public IReadOnlyList<DriverDiagnostic> DriverDiagnostics { get; init; } = new List<DriverDiagnostic>();

    /// <summary>
    /// Gets the performance snapshots taken during the diagnostic run.
    /// </summary>
    public IReadOnlyList<PerformanceSnapshot> PerformanceSnapshots { get; init; } = new List<PerformanceSnapshot>();
}
