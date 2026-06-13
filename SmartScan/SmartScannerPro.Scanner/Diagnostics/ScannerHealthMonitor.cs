namespace SmartScannerPro.Scanner.Diagnostics;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Events;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Diagnostics;
using SmartScannerPro.Scanner.WIA.Diagnostics;

/// <summary>
/// Routes health queries and forwards status change events from WIA and Mock health monitors.
/// </summary>
public sealed class ScannerHealthMonitor : IScannerHealthMonitor
{
    private readonly MockScannerHealthMonitor mockMonitor;
    private readonly WiaScannerHealthMonitor wiaMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerHealthMonitor"/> class.
    /// </summary>
    /// <param name="mockMonitor">The mock health monitor.</param>
    /// <param name="wiaMonitor">The WIA health monitor.</param>
    public ScannerHealthMonitor(MockScannerHealthMonitor mockMonitor, WiaScannerHealthMonitor wiaMonitor)
    {
        this.mockMonitor = mockMonitor ?? throw new ArgumentNullException(nameof(mockMonitor));
        this.wiaMonitor = wiaMonitor ?? throw new ArgumentNullException(nameof(wiaMonitor));

        this.mockMonitor.HealthStatusChanged += (s, e) => this.HealthStatusChanged?.Invoke(this, e);
        this.wiaMonitor.HealthStatusChanged += (s, e) => this.HealthStatusChanged?.Invoke(this, e);
    }

    /// <inheritdoc/>
    public event EventHandler<HealthChanged>? HealthStatusChanged;

    /// <summary>
    /// Starts health monitoring on all underlying monitors.
    /// </summary>
    public void StartMonitoring()
    {
        this.mockMonitor.StartMonitoring();
        this.wiaMonitor.StartMonitoring();
    }

    /// <inheritdoc/>
    public Task<DriverHealth> GetHealthAsync(string hardwareId, CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        if (hardwareId.StartsWith("MOCK-", StringComparison.OrdinalIgnoreCase))
        {
            return this.mockMonitor.GetHealthAsync(hardwareId, cancellationToken);
        }

        return this.wiaMonitor.GetHealthAsync(hardwareId, cancellationToken);
    }
}
