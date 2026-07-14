namespace SmartScannerPro.Scanner.WIA;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.WIA.Discovery;

/// <summary>
/// Provides the entry point for the WIA scanner technology provider.
/// </summary>
public sealed class WiaScannerProvider : IScannerProvider
{
    private readonly WiaScannerDiscoveryService discoveryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WiaScannerProvider"/> class.
    /// </summary>
    /// <param name="discoveryService">The discovery service to expose.</param>
    public WiaScannerProvider(WiaScannerDiscoveryService discoveryService)
    {
        this.discoveryService = discoveryService ?? throw new ArgumentNullException(nameof(discoveryService));
    }

    /// <inheritdoc/>
    public DriverType ProviderType => DriverType.WIA;

    /// <inheritdoc/>
    public bool IsSupported => OperatingSystem.IsWindows();

    /// <inheritdoc/>
    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public IScannerDiscoveryService GetDiscoveryService() => this.discoveryService;

    /// <inheritdoc/>
    public IScannerDriver CreateDriver(string hardwareId)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        return new WiaScannerDriver(hardwareId, "Generic", "1.0.0.0");
    }
}
