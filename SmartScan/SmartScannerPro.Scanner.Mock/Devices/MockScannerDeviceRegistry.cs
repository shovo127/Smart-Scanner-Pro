namespace SmartScannerPro.Scanner.Mock.Devices;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SmartScannerPro.Scanner.Mock.Configuration;

/// <summary>
/// Thread-safe in-memory registry of all simulated scanner devices.
/// Populated from <see cref="MockScannerConfiguration"/> during provider initialisation.
/// </summary>
public sealed class MockScannerDeviceRegistry
{
    private readonly ConcurrentDictionary<string, MockDeviceProfile> devices =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Populates the registry from the given configuration.
    /// Any existing devices are replaced.
    /// </summary>
    /// <param name="configuration">The mock scanner configuration to load.</param>
    public void Load(MockScannerConfiguration configuration)
    {
        this.devices.Clear();
        foreach (var profile in configuration.Devices)
        {
            this.devices[profile.HardwareId] = profile;
        }
    }

    /// <summary>
    /// Retrieves the device profile for the given hardware identifier.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier.</param>
    /// <returns>The <see cref="MockDeviceProfile"/>, or <c>null</c> if not found.</returns>
    public MockDeviceProfile? GetDevice(string hardwareId)
    {
        return this.devices.TryGetValue(hardwareId, out var profile) ? profile : null;
    }

    /// <summary>
    /// Returns all registered device profiles.
    /// </summary>
    /// <returns>A snapshot of all device profiles.</returns>
    public IReadOnlyList<MockDeviceProfile> GetAllDevices()
    {
        return new List<MockDeviceProfile>(this.devices.Values).AsReadOnly();
    }

    /// <summary>
    /// Gets the number of registered devices.
    /// </summary>
    public int Count => this.devices.Count;
}
