namespace SmartScannerPro.Tests.Integration.Mock;

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Discovery;
using Xunit;

/// <summary>
/// Verifies mock scanner discovery behavior.
/// </summary>
public sealed class MockDiscoveryTests
{
    /// <summary>
    /// Verifies that all profile files are discovered.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task Discovery_ReturnsAllConfiguredDevices()
    {
        var service = CreateService();

        var result = await service.DiscoverAsync(new DiscoveryRequest());

        result.Scanners.Should().HaveCount(7);
        result.TimedOut.Should().BeFalse();
    }

    /// <summary>
    /// Verifies USB filtering.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task Discovery_FiltersByUsbConnectionType()
    {
        var service = CreateService();

        var result = await service.DiscoverAsync(new DiscoveryRequest
        {
            AllowedConnectionTypes = new List<ConnectionType> { ConnectionType.Usb }.AsReadOnly(),
        });

        result.Scanners.Should().AllSatisfy(scanner => scanner.ConnectionType.Should().Be(ConnectionType.Usb));
    }

    /// <summary>
    /// Verifies driver filtering.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task Discovery_FiltersByMockDriverType_ReturnsAll()
    {
        var service = CreateService();

        var result = await service.DiscoverAsync(new DiscoveryRequest
        {
            AllowedDriverTypes = new List<DriverType> { DriverType.Mock }.AsReadOnly(),
        });

        result.Scanners.Should().HaveCount(7);
    }

    private static MockScannerDiscoveryService CreateService()
    {
        var configuration = MockScannerConfiguration.CreateDefault();
        var registry = new MockScannerDeviceRegistry();
        registry.Load(configuration);
        return new MockScannerDiscoveryService(registry);
    }
}
