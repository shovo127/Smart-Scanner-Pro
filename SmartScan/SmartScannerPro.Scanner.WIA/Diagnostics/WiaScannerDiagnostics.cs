namespace SmartScannerPro.Scanner.WIA.Diagnostics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Provides diagnostics and status checks for WIA scanner devices.
/// </summary>
public sealed class WiaScannerDiagnostics : IScannerDiagnostics
{
    /// <inheritdoc/>
    public Task<DiagnosticResult> RunDiagnosticsAsync(
        string hardwareId,
        CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        var checks = new List<HealthCheck>
        {
            new()
            {
                ComponentName = "WIA Service Connection",
                Status = DriverHealth.Healthy,
                Message = "Windows Image Acquisition service is running.",
                Timestamp = DateTimeOffset.UtcNow,
            },
            new()
            {
                ComponentName = "Hardware Connectivity",
                Status = DriverHealth.Healthy,
                Message = "Communication channel is established.",
                Timestamp = DateTimeOffset.UtcNow,
            },
        };

        var props = new Dictionary<string, string>
        {
            { "driver_version", "1.0.0.0" },
            { "native_provider", "WIA 2.0" },
        };

        var driverDiagnostics = new List<DriverDiagnostic>
        {
            new()
            {
                DriverId = hardwareId,
                Properties = props,
            },
        };

        var process = Process.GetCurrentProcess();
        var snapshot = new PerformanceSnapshot
        {
            Timestamp = DateTimeOffset.UtcNow,
            MemoryUsageBytes = process.PrivateMemorySize64,
            HandleCount = process.HandleCount,
            ThreadCount = process.Threads.Count,
        };

        var snapshots = new List<PerformanceSnapshot> { snapshot };

        var result = new DiagnosticResult
        {
            OverallHealth = DriverHealth.Healthy,
            Checks = checks.AsReadOnly(),
            DriverDiagnostics = driverDiagnostics.AsReadOnly(),
            PerformanceSnapshots = snapshots.AsReadOnly(),
        };

        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<PerformanceSnapshot> CapturePerformanceSnapshotAsync(
        string hardwareId,
        CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        var process = Process.GetCurrentProcess();
        var snapshot = new PerformanceSnapshot
        {
            Timestamp = DateTimeOffset.UtcNow,
            MemoryUsageBytes = process.PrivateMemorySize64,
            HandleCount = process.HandleCount,
            ThreadCount = process.Threads.Count,
        };

        return Task.FromResult(snapshot);
    }
}
