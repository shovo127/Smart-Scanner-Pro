namespace SmartScannerPro.Scanner.Mock.Diagnostics;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Events;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Simulation;

/// <summary>
/// Monitors simulated scanner health and fires <see cref="HealthStatusChanged"/> events
/// when a fault is injected by the <see cref="MockFailureSimulator"/>.
/// A background loop performs periodic health checks every 30 seconds.
/// </summary>
public sealed class MockScannerHealthMonitor : IScannerHealthMonitor, IAsyncDisposable
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(30);

    private readonly MockScannerDeviceRegistry registry;
    private readonly MockScannerConfiguration configuration;
    private readonly CancellationTokenSource backgroundCts = new();
    private Task? backgroundTask;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerHealthMonitor"/>.
    /// </summary>
    /// <param name="registry">The device registry to monitor.</param>
    /// <param name="configuration">The configuration providing failure options.</param>
    public MockScannerHealthMonitor(
        MockScannerDeviceRegistry registry,
        MockScannerConfiguration configuration)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc/>
    public event EventHandler<HealthChanged>? HealthStatusChanged;

    /// <summary>
    /// Starts the background health monitoring loop.
    /// </summary>
    public void StartMonitoring()
    {
        this.backgroundTask = Task.Run(() => this.MonitorLoopAsync(this.backgroundCts.Token));
    }

    /// <inheritdoc/>
    public Task<DriverHealth> GetHealthAsync(
        string hardwareId,
        CancellationToken cancellationToken = default)
    {
        var profile = this.registry.GetDevice(hardwareId);
        if (profile is null)
        {
            return Task.FromResult(DriverHealth.Unknown);
        }

        var options = profile.FailureOverride ?? this.configuration.GlobalFailureOptions;

        // If random failure probability is high, simulate degraded health
        var health = MockFailureSimulator.ShouldFail(options.RandomFailureProbability)
            ? DriverHealth.Degraded
            : DriverHealth.Healthy;

        return Task.FromResult(health);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        this.backgroundCts.Cancel();
        if (this.backgroundTask is not null)
        {
            try
            {
                await this.backgroundTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected on cancellation
            }
        }

        this.backgroundCts.Dispose();
    }

    private async Task MonitorLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(CheckInterval, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            foreach (var device in this.registry.GetAllDevices())
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var options = device.FailureOverride ?? this.configuration.GlobalFailureOptions;
                if (MockFailureSimulator.ShouldFail(options.RandomFailureProbability))
                {
                    this.HealthStatusChanged?.Invoke(
                        this,
                        new HealthChanged(device.HardwareId, DriverHealth.Degraded, DateTimeOffset.UtcNow));
                }
            }
        }
    }
}
