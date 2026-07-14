namespace SmartScannerPro.Scanner.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Events;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Aggregates health monitoring and queries across all registered provider health monitors.
/// Each provider registers its own <see cref="IScannerHealthMonitor"/> implementation;
/// this orchestrator forwards events and delegates queries to the appropriate provider.
/// </summary>
public sealed class ScannerHealthMonitor : IScannerHealthMonitor
{
    private readonly IReadOnlyList<IScannerHealthMonitor> providerMonitors;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerHealthMonitor"/> class.
    /// </summary>
    /// <param name="providerMonitors">All registered provider-level health monitors.</param>
    public ScannerHealthMonitor(IEnumerable<IScannerHealthMonitor> providerMonitors)
    {
        this.providerMonitors = (providerMonitors ?? throw new ArgumentNullException(nameof(providerMonitors)))
            .ToList()
            .AsReadOnly();

        foreach (var monitor in this.providerMonitors)
        {
            monitor.HealthStatusChanged += (s, e) => this.HealthStatusChanged?.Invoke(this, e);
        }
    }

    /// <inheritdoc/>
    public event EventHandler<HealthChanged>? HealthStatusChanged;

    /// <summary>
    /// Starts health monitoring on all registered provider monitors.
    /// </summary>
    public void StartMonitoring()
    {
        foreach (var monitor in this.providerMonitors)
        {
            if (monitor is IStartableHealthMonitor startable)
            {
                startable.StartMonitoring();
            }
        }
    }

    /// <inheritdoc/>
    public async Task<DriverHealth> GetHealthAsync(string hardwareId, CancellationToken cancellationToken = default)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        foreach (var monitor in this.providerMonitors)
        {
            try
            {
                return await monitor.GetHealthAsync(hardwareId, cancellationToken).ConfigureAwait(false);
            }
            catch (NotSupportedException)
            {
                // Monitor doesn't handle this hardware ID — try the next
            }
        }

        return new DriverHealth
        {
            HardwareId = hardwareId,
            Status = HealthStatus.Unknown,
        };
    }
}
