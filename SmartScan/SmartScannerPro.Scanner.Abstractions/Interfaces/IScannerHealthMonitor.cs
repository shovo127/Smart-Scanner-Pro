namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Monitors the health and connectivity of scanner devices.
/// </summary>
public interface IScannerHealthMonitor
{
    /// <summary>
    /// Gets the current health status of a specific scanner.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The driver health status.</returns>
    Task<DriverHealth> GetHealthAsync(string hardwareId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Event raised when a scanner's health status changes.
    /// </summary>
    event EventHandler<Events.HealthChanged>? HealthStatusChanged;
}
