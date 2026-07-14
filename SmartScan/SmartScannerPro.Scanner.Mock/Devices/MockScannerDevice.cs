namespace SmartScannerPro.Scanner.Mock.Devices;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Capabilities;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Capabilities;
using SmartScannerPro.Scanner.Mock.Configuration;

/// <summary>
/// Simulates a physical scanner device, implementing the device, capabilities,
/// and configuration interfaces of the Scanner SDK.
/// </summary>
public sealed class MockScannerDevice : IScannerDevice, IScannerCapabilities, IScannerConfiguration
{
    private readonly MockDeviceProfile profile;
    private readonly Dictionary<string, object> currentValues = new(StringComparer.OrdinalIgnoreCase);
    private CapabilitySet capabilitySet;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerDevice"/> for the given profile.
    /// </summary>
    /// <param name="profile">The device profile to simulate.</param>
    public MockScannerDevice(MockDeviceProfile profile)
    {
        this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
        this.capabilitySet = MockCapabilityFactory.Build(profile);

        this.Descriptor = new ScannerDescriptor
        {
            HardwareId = profile.HardwareId,
            Name = profile.Name,
            Manufacturer = profile.Manufacturer,
            ConnectionType = profile.ConnectionType,
            Driver = new DriverInfo
            {
                Id = Guid.NewGuid(),
                Name = "Mock Driver",
                Type = DriverType.Mock,
                Version = new DriverVersion { Major = 1, Minor = 0, Build = 0, Revision = 0, OriginalString = "1.0.0.0" },
                Priority = DriverPriority.Preferred,
                Status = DriverStatus.Ready,
                Manufacturer = profile.Manufacturer,
                NativeProvider = "SmartScannerPro.Scanner.Mock",
            },
            ConnectionString = profile.ConnectionString,
        };
    }

    /// <inheritdoc/>
    public ScannerDescriptor Descriptor { get; }

    /// <inheritdoc/>
    public IScannerCapabilities Capabilities => this;

    /// <inheritdoc/>
    public IScannerConfiguration Configuration => this;

    /// <inheritdoc/>
    public IScannerConnection? Connection => null;

    // IScannerCapabilities ──────────────────────────────────────────────────

    /// <inheritdoc/>
    public CapabilitySet SupportedCapabilities => this.capabilitySet;

    /// <inheritdoc/>
    public Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        this.capabilitySet = MockCapabilityFactory.Build(this.profile);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<bool> SetCapabilityValueAsync<T>(string capabilityId, T value, CancellationToken cancellationToken = default)
    {
        var cap = this.capabilitySet.GetCapability<T>(capabilityId);
        if (cap is null)
        {
            return Task.FromResult(false);
        }

        if (value is not null && !cap.Value.IsSupported(value))
        {
            return Task.FromResult(false);
        }

        this.currentValues[capabilityId] = value!;
        return Task.FromResult(true);
    }

    // IScannerConfiguration ─────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task ApplyConfigurationAsync(
        IReadOnlyDictionary<string, object> settings,
        CancellationToken cancellationToken = default)
    {
        foreach (var kvp in settings)
        {
            this.currentValues[kvp.Key] = kvp.Value;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task ResetToDefaultsAsync(CancellationToken cancellationToken = default)
    {
        this.currentValues.Clear();
        this.capabilitySet = MockCapabilityFactory.Build(this.profile);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the currently applied value for a capability, falling back to the
    /// default value defined in the capability set.
    /// </summary>
    /// <typeparam name="T">The capability value type.</typeparam>
    /// <param name="capabilityId">The capability identifier.</param>
    /// <returns>The current value, or the default if not overridden.</returns>
    public T? GetCurrentValue<T>(string capabilityId)
    {
        if (this.currentValues.TryGetValue(capabilityId, out var obj) && obj is T typed)
        {
            return typed;
        }

        var cap = this.capabilitySet.GetCapability<T>(capabilityId);
        return cap is not null ? cap.Value.CurrentValue : default;
    }
}
