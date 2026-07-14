namespace SmartScannerPro.Scanner.Mock.Factory;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Sessions;

/// <summary>
/// Implements <see cref="IScannerProviderFactory"/> for the Mock scanner provider.
/// Handles any hardware identifier prefixed with <c>"MOCK-"</c>.
/// </summary>
public sealed class MockScannerProviderFactory : IScannerProviderFactory
{
    private readonly MockScannerDeviceRegistry registry;
    private readonly MockScannerConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerProviderFactory"/>.
    /// </summary>
    /// <param name="registry">The device registry used to resolve hardware identifiers.</param>
    /// <param name="configuration">The mock configuration providing global failure options.</param>
    public MockScannerProviderFactory(
        MockScannerDeviceRegistry registry,
        MockScannerConfiguration configuration)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc/>
    public string ProviderName => "Mock";

    /// <inheritdoc/>
    public bool CanHandle(string hardwareId) =>
        !string.IsNullOrWhiteSpace(hardwareId) &&
        hardwareId.StartsWith("MOCK-", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public Task<IScannerSession> CreateSessionAsync(
        ScanSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        var profile = this.registry.GetDevice(options.HardwareId)
            ?? throw new InvalidOperationException(
                $"No mock scanner registered with hardware ID '{options.HardwareId}'. " +
                "Ensure the device is present in MockScannerConfiguration.");

        var device = new MockScannerDevice(profile);
        var failureOptions = profile.FailureOverride ?? this.configuration.GlobalFailureOptions;
        var session = new MockScannerSession(device, profile, failureOptions);

        return Task.FromResult<IScannerSession>(session);
    }
}
