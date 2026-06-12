namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Represents the low-level driver interface for communicating with the scanner hardware.
/// </summary>
public interface IScannerDriver : IAsyncDisposable
{
    /// <summary>
    /// Gets the metadata and information about this driver.
    /// </summary>
    DriverInfo Info { get; }

    /// <summary>
    /// Initializes the driver for use.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
