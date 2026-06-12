namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Represents a specific scanner technology provider (e.g., WIA, TWAIN, Mock).
/// </summary>
public interface IScannerProvider
{
    /// <summary>
    /// Gets the type of driver technology this provider supports.
    /// </summary>
    DriverType ProviderType { get; }

    /// <summary>
    /// Gets a value indicating whether this provider is supported on the current operating system and architecture.
    /// </summary>
    bool IsSupported { get; }

    /// <summary>
    /// Initializes the provider.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the discovery service specific to this provider.
    /// </summary>
    /// <returns>The discovery service.</returns>
    IScannerDiscoveryService GetDiscoveryService();

    /// <summary>
    /// Gets the driver factory specific to this provider.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier.</param>
    /// <returns>The driver factory.</returns>
    IScannerDriver CreateDriver(string hardwareId);
}
