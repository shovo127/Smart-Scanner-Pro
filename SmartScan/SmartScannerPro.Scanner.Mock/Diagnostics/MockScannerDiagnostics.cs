namespace SmartScannerPro.Scanner.Mock.Diagnostics;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;

/// <summary>
/// Provides simulated diagnostics for mock scanner devices.
/// Health metrics, driver diagnostics, and performance snapshots are generated
/// deterministically based on the device performance profile.
/// </summary>
public sealed class MockScannerDiagnostics : IScannerDiagnostics
{
    private readonly MockScannerDeviceRegistry registry;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerDiagnostics"/>.
    /// </summary>
    /// <param name="registry">The device registry to query.</param>
    public MockScannerDiagnostics(MockScannerDeviceRegistry registry)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public Task<DiagnosticResult> RunDiagnosticsAsync(
        string hardwareId,
        CancellationToken cancellationToken = default)
    {
        var profile = this.registry.GetDevice(hardwareId);
        var health = profile is null || profile.PerformanceProfile == MockPerformanceProfileType.Faulty
            ? DriverHealth.Degraded
            : DriverHealth.Healthy;

        var checks = BuildHealthChecks(profile);
        var driverDiagnostics = BuildDriverDiagnostics(profile);
        var snapshots = new List<PerformanceSnapshot> { BuildSnapshot(profile) };

        var result = new DiagnosticResult
        {
            OverallHealth = health,
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
        var profile = this.registry.GetDevice(hardwareId);
        return Task.FromResult(BuildSnapshot(profile));
    }

    private static List<HealthCheck> BuildHealthChecks(MockDeviceProfile? profile)
    {
        var isFaulty = profile?.PerformanceProfile == MockPerformanceProfileType.Faulty;

        return new List<HealthCheck>
        {
            new()
            {
                ComponentName = "Connectivity",
                Status = DriverHealth.Healthy,
                Message = "Device communication channel is responsive.",
                Timestamp = DateTimeOffset.UtcNow,
            },
            new()
            {
                ComponentName = "Hardware",
                Status = isFaulty ? DriverHealth.Degraded : DriverHealth.Healthy,
                Message = isFaulty ? "Minor hardware irregularities detected." : "Hardware self-test passed.",
                Timestamp = DateTimeOffset.UtcNow,
            },
            new()
            {
                ComponentName = "Driver",
                Status = DriverHealth.Healthy,
                Message = "Mock driver v1.0.0 loaded successfully.",
                Timestamp = DateTimeOffset.UtcNow,
            },
            new()
            {
                ComponentName = "Paper Feed",
                Status = DriverHealth.Healthy,
                Message = "Paper path clear. No jams detected.",
                Timestamp = DateTimeOffset.UtcNow,
            },
            new()
            {
                ComponentName = "Ink / Toner",
                Status = isFaulty ? DriverHealth.Degraded : DriverHealth.Healthy,
                Message = isFaulty ? "Consumable levels low." : "Consumable levels adequate.",
                Timestamp = DateTimeOffset.UtcNow,
            },
        };
    }

    private static List<DriverDiagnostic> BuildDriverDiagnostics(MockDeviceProfile? profile)
    {
        var props = new Dictionary<string, string>
        {
            { "firmware", profile?.FirmwareVersion ?? "1.0.0" },
            { "connection_quality", profile?.ConnectionType == SmartScannerPro.Scanner.Abstractions.Models.Discovery.ConnectionType.Network ? "85%" : "100%" },
            { "driver_version", "1.0.0.0" },
            { "temperature_celsius", profile?.PerformanceProfile == MockPerformanceProfileType.Faulty ? "68" : "42" },
            { "pages_since_last_service", "1247" },
            { "total_pages_lifetime", "45892" },
        };

        return new List<DriverDiagnostic>
        {
            new()
            {
                DriverId = profile?.HardwareId ?? "MOCK-UNKNOWN",
                Properties = props,
            },
        };
    }

    private static PerformanceSnapshot BuildSnapshot(MockDeviceProfile? profile)
    {
        var baseMemory = profile?.PerformanceProfile switch
        {
            MockPerformanceProfileType.Enterprise => 48L * 1024 * 1024,
            MockPerformanceProfileType.Fast => 32L * 1024 * 1024,
            MockPerformanceProfileType.Faulty => 96L * 1024 * 1024,
            _ => 24L * 1024 * 1024,
        };

        var jitter = (long)(Random.Shared.NextDouble() * 4 * 1024 * 1024);

        return new PerformanceSnapshot
        {
            Timestamp = DateTimeOffset.UtcNow,
            MemoryUsageBytes = baseMemory + jitter,
            HandleCount = 42 + Random.Shared.Next(0, 10),
            ThreadCount = 4 + Random.Shared.Next(0, 4),
        };
    }
}
