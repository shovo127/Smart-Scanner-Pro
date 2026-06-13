namespace SmartScannerPro.Scanner.Diagnostics;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;
using SmartScannerPro.Scanner.Mock.Diagnostics;
using SmartScannerPro.Scanner.WIA.Diagnostics;

/// <summary>
/// Routes scanner diagnostics queries to the appropriate provider diagnostic service based on the HardwareId.
/// </summary>
public sealed class ScannerDiagnostics : IScannerDiagnostics
{
    private readonly MockScannerDiagnostics mockDiagnostics;
    private readonly WiaScannerDiagnostics wiaDiagnostics;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerDiagnostics"/> class.
    /// </summary>
    /// <param name="mockDiagnostics">The mock diagnostics service.</param>
    /// <param name="wiaDiagnostics">The WIA diagnostics service.</param>
    public ScannerDiagnostics(MockScannerDiagnostics mockDiagnostics, WiaScannerDiagnostics wiaDiagnostics)
    {
        this.mockDiagnostics = mockDiagnostics ?? throw new ArgumentNullException(nameof(mockDiagnostics));
        this.wiaDiagnostics = wiaDiagnostics ?? throw new ArgumentNullException(nameof(wiaDiagnostics));
    }

    /// <inheritdoc/>
    public Task<DiagnosticResult> RunDiagnosticsAsync(string hardwareId, CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        if (hardwareId.StartsWith("MOCK-", StringComparison.OrdinalIgnoreCase))
        {
            return this.mockDiagnostics.RunDiagnosticsAsync(hardwareId, cancellationToken);
        }

        return this.wiaDiagnostics.RunDiagnosticsAsync(hardwareId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<PerformanceSnapshot> CapturePerformanceSnapshotAsync(string hardwareId, CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        if (hardwareId.StartsWith("MOCK-", StringComparison.OrdinalIgnoreCase))
        {
            return this.mockDiagnostics.CapturePerformanceSnapshotAsync(hardwareId, cancellationToken);
        }

        return this.wiaDiagnostics.CapturePerformanceSnapshotAsync(hardwareId, cancellationToken);
    }
}
