namespace SmartScannerPro.Scanner.Mock.Configuration;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

/// <summary>
/// Root configuration record for the Mock Scanner Provider.
/// Holds the fleet of simulated devices and global failure simulation settings.
/// </summary>
public sealed record MockScannerConfiguration
{
    /// <summary>
    /// Gets the ordered list of simulated scanner device profiles.
    /// </summary>
    public IReadOnlyList<MockDeviceProfile> Devices { get; init; } = Array.Empty<MockDeviceProfile>();

    /// <summary>
    /// Gets the global failure simulation options applied to all devices that do not
    /// specify a per-device override.
    /// </summary>
    public FailureSimulationOptions GlobalFailureOptions { get; init; } = FailureSimulationOptions.None;

    /// <summary>
    /// Gets or sets the rate (0.0–1.0) at which blank pages are injected into scans.
    /// Useful for testing blank-page detection pipelines.
    /// </summary>
    public double BlankPageRate { get; init; }

    /// <summary>
    /// Gets or sets the maximum number of pages per scan job. When zero, jobs are
    /// unlimited (controlled by the caller).
    /// </summary>
    public int MaxPagesPerJob { get; init; }

    /// <summary>
    /// Creates the default Mock Scanner configuration containing all seven
    /// preconfigured reference devices and safe (zero-failure) defaults.
    /// </summary>
    /// <returns>A fully populated <see cref="MockScannerConfiguration"/>.</returns>
    public static MockScannerConfiguration CreateDefault()
    {
        return LoadFromProfilesDirectory(Path.Combine(AppContext.BaseDirectory, "Profiles"));
    }

    /// <summary>
    /// Creates a single-device configuration intended for fast unit testing,
    /// using an enterprise profile with no failure simulation.
    /// </summary>
    /// <returns>A minimal <see cref="MockScannerConfiguration"/> with one device.</returns>
    public static MockScannerConfiguration CreateMinimal()
    {
        var profile = LoadProfilesFromDirectory(Path.Combine(AppContext.BaseDirectory, "Profiles"))
            .FirstOrDefault();
        return new MockScannerConfiguration
        {
            GlobalFailureOptions = FailureSimulationOptions.None,
            Devices = profile is null ? Array.Empty<MockDeviceProfile>() : new[] { profile },
        };
    }

    /// <summary>
    /// Loads all scanner profiles from a directory.
    /// </summary>
    /// <param name="directoryPath">The profile directory.</param>
    /// <returns>A scanner configuration.</returns>
    public static MockScannerConfiguration LoadFromProfilesDirectory(string directoryPath)
    {
        return new MockScannerConfiguration
        {
            GlobalFailureOptions = FailureSimulationOptions.None,
            Devices = LoadProfilesFromDirectory(directoryPath),
        };
    }

    private static IReadOnlyList<MockDeviceProfile> LoadProfilesFromDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return Array.Empty<MockDeviceProfile>();
        }

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var profiles = new List<MockDeviceProfile>();
        foreach (var path in Directory.EnumerateFiles(directoryPath, "*.json"))
        {
            using var stream = File.OpenRead(path);
            var profile = JsonSerializer.Deserialize<MockDeviceProfile>(stream, serializerOptions);
            if (profile is not null)
            {
                profiles.Add(profile);
            }
        }

        return new ReadOnlyCollection<MockDeviceProfile>(profiles.OrderBy(x => x.Name).ToList());
    }
}
