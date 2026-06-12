namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;

/// <summary>
/// Provides operations for discovering scanner devices.
/// </summary>
public interface IScannerDiscoveryService
{
    /// <summary>
    /// Discovers scanners matching the specified request criteria.
    /// </summary>
    /// <param name="request">The discovery request parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the discovery operation.</returns>
    Task<DiscoveryResult> DiscoverAsync(DiscoveryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Occurs when a new scanner is discovered dynamically (e.g., via Plug and Play).
    /// </summary>
    event EventHandler<ScannerDescriptor>? ScannerDiscovered;

    /// <summary>
    /// Occurs when a scanner is removed or disconnected dynamically.
    /// </summary>
    event EventHandler<ScannerDescriptor>? ScannerLost;
}
