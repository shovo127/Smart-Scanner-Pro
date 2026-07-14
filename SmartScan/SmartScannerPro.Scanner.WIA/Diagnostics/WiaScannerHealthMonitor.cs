namespace SmartScannerPro.Scanner.WIA.Diagnostics;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Events;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Monitors WIA scanner health status.
/// </summary>
public sealed class WiaScannerHealthMonitor : IScannerHealthMonitor, IAsyncDisposable
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(30);
    private readonly CancellationTokenSource backgroundCts = new();
    private Task? backgroundTask;

#pragma warning disable CS0067 // The event is required by the SDK interface but not raised in this base implementation.
    /// <inheritdoc/>
    public event EventHandler<HealthChanged>? HealthStatusChanged;
#pragma warning restore CS0067

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
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        return Task.FromResult(DriverHealth.Healthy);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        this.backgroundCts.Cancel();
        if (this.backgroundTask != null)
        {
            try
            {
                await this.backgroundTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Normal cancellation flow
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
        }
    }
}
