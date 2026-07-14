namespace SmartScannerPro.Tests.Integration.Mock;

using System.Threading.Tasks;
using FluentAssertions;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Factory;
using Xunit;

/// <summary>
/// Verifies mock scanner session behavior.
/// </summary>
public sealed class MockSessionTests
{
    /// <summary>
    /// Verifies session creation for a known device.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task Session_CreatesSuccessfully_ForKnownDevice()
    {
        var factory = CreateFactory();

        var session = await factory.CreateSessionAsync(new ScanSessionOptions
        {
            HardwareId = "MOCK-FUJITSU-SCANSNAP-IX1600",
        });

        session.State.Should().Be(ScanSessionState.Initializing);
        await session.DisposeAsync();
    }

    /// <summary>
    /// Verifies cancellation updates state.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task Session_CanCancel_CleanlyWithoutException()
    {
        var factory = CreateFactory();

        await using var session = await factory.CreateSessionAsync(new ScanSessionOptions
        {
            HardwareId = "MOCK-FUJITSU-SCANSNAP-IX1600",
        });

        await session.CancelAsync();

        session.State.Should().Be(ScanSessionState.Cancelled);
    }

    private static MockScannerFactory CreateFactory()
    {
        var configuration = MockScannerConfiguration.CreateDefault();
        var registry = new MockScannerDeviceRegistry();
        registry.Load(configuration);
        return new MockScannerFactory(registry, configuration);
    }
}
