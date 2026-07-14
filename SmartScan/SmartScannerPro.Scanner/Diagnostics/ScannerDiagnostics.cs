namespace SmartScannerPro.Scanner.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;

/// <summary>
/// Aggregates diagnostics queries across all registered provider diagnostics services.
/// Each provider registers its own <see cref="IScannerDiagnostics"/> implementation;
/// this orchestrator delegates based on hardware ID prefix matching.
/// </summary>
public sealed class ScannerDiagnostics : IScannerDiagnostics
{
    private readonly IReadOnlyList<IScannerDiagnostics> providerDiagnostics;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerDiagnostics"/> class.
    /// </summary>
    /// <param name="providerDiagnostics">All registered provider-level diagnostics services.</param>
    public ScannerDiagnostics(IEnumerable<IScannerDiagnostics> providerDiagnostics)
    {
        this.providerDiagnostics = (providerDiagnostics ?? throw new ArgumentNullException(nameof(providerDiagnostics)))
            .ToList()
            .AsReadOnly();
    }

    /// <inheritdoc/>
    public async Task<DiagnosticResult> RunDiagnosticsAsync(string hardwareId, CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        // Try each registered provider until one succeeds
        foreach (var service in this.providerDiagnostics)
        {
            try
            {
                var result = await service.RunDiagnosticsAsync(hardwareId, cancellationToken).ConfigureAwait(false);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotSupportedException)
            {
                // Provider doesn't handle this hardware ID — try the next
            }
        }

        return new DiagnosticResult
        {
            HardwareId = hardwareId,
            IsOnline = false,
            ErrorMessage = "No diagnostics provider is available for this device.",
        };
    }

    /// <inheritdoc/>
    public async Task<PerformanceSnapshot> CapturePerformanceSnapshotAsync(string hardwareId, CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        foreach (var service in this.providerDiagnostics)
        {
            try
            {
                var snapshot = await service.CapturePerformanceSnapshotAsync(hardwareId, cancellationToken).ConfigureAwait(false);
                if (snapshot != null)
                {
                    return snapshot;
                }
            }
            catch (NotSupportedException)
            {
                // Provider doesn't handle this hardware ID — try the next
            }
        }

        return new PerformanceSnapshot
        {
            HardwareId = hardwareId,
            CapturedAt = DateTimeOffset.UtcNow,
        };
    }
}
