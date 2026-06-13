namespace SmartScannerPro.Tests.Integration.Wia;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.WIA.Discovery;
using Xunit;

/// <summary>
/// Verifies the WIA scanner discovery service behavior.
/// </summary>
public sealed class WiaDiscoveryTests
{
    /// <summary>
    /// Verifies that discovery runs successfully without throwing exceptions,
    /// even if no physical WIA scanner is connected.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task DiscoverAsync_ExecutesCleanly_EvenWithoutHardware()
    {
        var service = new WiaScannerDiscoveryService();

        var result = await service.DiscoverAsync(new DiscoveryRequest());

        result.Should().NotBeNull();
        result.TimedOut.Should().BeFalse();
        result.Duration.Should().BeGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Verifies that filtering by connection type USB works correctly if any devices are found.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task DiscoverAsync_WithUsbConnectionTypeFilter_FiltersCorrectly()
    {
        var service = new WiaScannerDiscoveryService();
        var request = new DiscoveryRequest
        {
            AllowedConnectionTypes = new List<ConnectionType> { ConnectionType.Usb }.AsReadOnly(),
        };

        var result = await service.DiscoverAsync(request);

        result.Should().NotBeNull();
        foreach (var scanner in result.Scanners)
        {
            scanner.ConnectionType.Should().Be(ConnectionType.Usb);
        }
    }

    /// <summary>
    /// Verifies that filtering by driver type WIA works correctly.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task DiscoverAsync_WithWiaDriverTypeFilter_FiltersCorrectly()
    {
        var service = new WiaScannerDiscoveryService();
        var request = new DiscoveryRequest
        {
            AllowedDriverTypes = new List<DriverType> { DriverType.WIA }.AsReadOnly(),
        };

        var result = await service.DiscoverAsync(request);

        result.Should().NotBeNull();
        foreach (var scanner in result.Scanners)
        {
            scanner.Driver.Type.Should().Be(DriverType.WIA);
        }
    }
}
