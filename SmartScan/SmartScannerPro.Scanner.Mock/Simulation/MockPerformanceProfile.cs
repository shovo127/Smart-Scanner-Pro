namespace SmartScannerPro.Scanner.Mock.Simulation;

using System;
using SmartScannerPro.Scanner.Mock.Configuration;

/// <summary>
/// Holds the timing delays for each stage of a scan operation, calibrated per performance profile.
/// All values represent simulated hardware latency.
/// </summary>
public sealed record MockPerformanceProfile
{
    /// <summary>Gets the delay simulating hardware discovery broadcast time.</summary>
    public TimeSpan DiscoveryDelay { get; init; }

    /// <summary>Gets the delay simulating initial hardware connection and negotiation.</summary>
    public TimeSpan ConnectDelay { get; init; }

    /// <summary>Gets the delay per page during physical image acquisition.</summary>
    public TimeSpan PerPageAcquisitionDelay { get; init; }

    /// <summary>Gets the delay simulating image data transfer from device to host.</summary>
    public TimeSpan PerPageTransferDelay { get; init; }

    /// <summary>Gets the delay for final session teardown.</summary>
    public TimeSpan FinishDelay { get; init; }

    /// <summary>
    /// Gets the standard deviation for random jitter applied to each delay.
    /// Only used by the <see cref="MockPerformanceProfileType.Faulty"/> profile.
    /// </summary>
    public TimeSpan JitterStdDev { get; init; }

    /// <summary>
    /// Resolves the performance profile for the given profile type.
    /// </summary>
    /// <param name="profileType">The profile type to resolve.</param>
    /// <returns>The calibrated <see cref="MockPerformanceProfile"/> instance.</returns>
    public static MockPerformanceProfile For(MockPerformanceProfileType profileType)
    {
        return profileType switch
        {
            MockPerformanceProfileType.Fast => Fast,
            MockPerformanceProfileType.Slow => Slow,
            MockPerformanceProfileType.Network => Network,
            MockPerformanceProfileType.Faulty => Faulty,
            MockPerformanceProfileType.Enterprise => Enterprise,
            _ => Fast,
        };
    }

    /// <summary>Fast consumer ADF scanner — sub-second per page.</summary>
    public static MockPerformanceProfile Fast => new()
    {
        DiscoveryDelay = TimeSpan.FromMilliseconds(300),
        ConnectDelay = TimeSpan.FromMilliseconds(100),
        PerPageAcquisitionDelay = TimeSpan.FromMilliseconds(200),
        PerPageTransferDelay = TimeSpan.FromMilliseconds(80),
        FinishDelay = TimeSpan.FromMilliseconds(100),
        JitterStdDev = TimeSpan.Zero,
    };

    /// <summary>Consumer flatbed scanner — slow motor movement, high-quality scans.</summary>
    public static MockPerformanceProfile Slow => new()
    {
        DiscoveryDelay = TimeSpan.FromMilliseconds(2000),
        ConnectDelay = TimeSpan.FromMilliseconds(800),
        PerPageAcquisitionDelay = TimeSpan.FromMilliseconds(3000),
        PerPageTransferDelay = TimeSpan.FromMilliseconds(500),
        FinishDelay = TimeSpan.FromMilliseconds(400),
        JitterStdDev = TimeSpan.FromMilliseconds(200),
    };

    /// <summary>Networked MFP — subject to LAN latency and queue depth.</summary>
    public static MockPerformanceProfile Network => new()
    {
        DiscoveryDelay = TimeSpan.FromMilliseconds(1500),
        ConnectDelay = TimeSpan.FromMilliseconds(500),
        PerPageAcquisitionDelay = TimeSpan.FromMilliseconds(800),
        PerPageTransferDelay = TimeSpan.FromMilliseconds(300),
        FinishDelay = TimeSpan.FromMilliseconds(200),
        JitterStdDev = TimeSpan.FromMilliseconds(150),
    };

    /// <summary>Unstable device — wide timing jitter, random stalls.</summary>
    public static MockPerformanceProfile Faulty => new()
    {
        DiscoveryDelay = TimeSpan.FromMilliseconds(2500),
        ConnectDelay = TimeSpan.FromMilliseconds(1000),
        PerPageAcquisitionDelay = TimeSpan.FromMilliseconds(1500),
        PerPageTransferDelay = TimeSpan.FromMilliseconds(400),
        FinishDelay = TimeSpan.FromMilliseconds(300),
        JitterStdDev = TimeSpan.FromMilliseconds(800),
    };

    /// <summary>High-throughput enterprise scanner — consistent, optimised timings.</summary>
    public static MockPerformanceProfile Enterprise => new()
    {
        DiscoveryDelay = TimeSpan.FromMilliseconds(500),
        ConnectDelay = TimeSpan.FromMilliseconds(150),
        PerPageAcquisitionDelay = TimeSpan.FromMilliseconds(150),
        PerPageTransferDelay = TimeSpan.FromMilliseconds(60),
        FinishDelay = TimeSpan.FromMilliseconds(80),
        JitterStdDev = TimeSpan.Zero,
    };
}
