namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;

/// <summary>
/// Provides deep diagnostic capabilities for troubleshooting scanner issues.
/// </summary>
public interface IScannerDiagnostics
{
    /// <summary>
    /// Runs a comprehensive diagnostic suite on the specified scanner.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The diagnostic results.</returns>
    Task<DiagnosticResult> RunDiagnosticsAsync(string hardwareId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Captures a point-in-time performance snapshot of the scanner subsystem.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The performance snapshot.</returns>
    Task<PerformanceSnapshot> CapturePerformanceSnapshotAsync(string hardwareId, CancellationToken cancellationToken = default);
}
