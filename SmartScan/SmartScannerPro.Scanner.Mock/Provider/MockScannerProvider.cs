namespace SmartScannerPro.Scanner.Mock.Provider;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Discovery;

/// <summary>
/// The entry point for the Mock Scanner technology provider.
/// This is always supported (no hardware required) and initialises the device registry
/// from the supplied configuration.
/// </summary>
public sealed class MockScannerProvider : IScannerProvider
{
    private readonly MockScannerConfiguration configuration;
    private readonly MockScannerDeviceRegistry registry;
    private readonly MockScannerDiscoveryService discoveryService;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerProvider"/>.
    /// </summary>
    /// <param name="configuration">The mock scanner configuration.</param>
    /// <param name="registry">The device registry to populate.</param>
    /// <param name="discoveryService">The discovery service to expose.</param>
    public MockScannerProvider(
        MockScannerConfiguration configuration,
        MockScannerDeviceRegistry registry,
        MockScannerDiscoveryService discoveryService)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.discoveryService = discoveryService ?? throw new ArgumentNullException(nameof(discoveryService));
    }

    /// <inheritdoc/>
    public DriverType ProviderType => DriverType.Mock;

    /// <inheritdoc/>
    public bool IsSupported => true;

    /// <inheritdoc/>
    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        this.registry.Load(this.configuration);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public IScannerDiscoveryService GetDiscoveryService() => this.discoveryService;

    /// <inheritdoc/>
    public IScannerDriver CreateDriver(string hardwareId)
    {
        var profile = this.registry.GetDevice(hardwareId)
            ?? throw new InvalidOperationException(
                $"No mock device registered with hardware ID '{hardwareId}'.");

        return new MockScannerDriver(hardwareId, profile.Manufacturer, profile.FirmwareVersion);
    }
}
