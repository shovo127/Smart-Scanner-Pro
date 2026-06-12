namespace SmartScannerPro.Scanner.Mock.Configuration;

using System;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Represents the per-page failure simulation probabilities for a mock scanner device.
/// Each probability value must be in the range [0.0, 1.0].
/// </summary>
public sealed record FailureSimulationOptions
{
    /// <summary>Gets the probability of a paper jam occurring per page.</summary>
    public double PaperJamProbability { get; init; }

    /// <summary>Gets the probability of the ADF running out of paper.</summary>
    public double OutOfPaperProbability { get; init; }

    /// <summary>Gets the probability of the cover being detected as open.</summary>
    public double CoverOpenProbability { get; init; }

    /// <summary>Gets the probability of a USB/network disconnect event.</summary>
    public double DisconnectProbability { get; init; }

    /// <summary>Gets the probability of a driver-level failure.</summary>
    public double DriverFailureProbability { get; init; }

    /// <summary>Gets the probability of a timeout occurring.</summary>
    public double TimeoutProbability { get; init; }

    /// <summary>Gets the probability of a random unclassified failure.</summary>
    public double RandomFailureProbability { get; init; }

    /// <summary>Gets the probability of a low-memory error.</summary>
    public double LowMemoryProbability { get; init; }

    /// <summary>
    /// Gets the default failure options with all probabilities set to zero.
    /// </summary>
    public static FailureSimulationOptions None => new();

    /// <summary>
    /// Gets failure options configured to reliably simulate a paper jam on every scan.
    /// </summary>
    public static FailureSimulationOptions AlwaysPaperJam => new() { PaperJamProbability = 1.0 };

    /// <summary>
    /// Gets failure options that simulate a faulty device with moderate random failures.
    /// </summary>
    public static FailureSimulationOptions FaultyDevice => new()
    {
        PaperJamProbability = 0.15,
        RandomFailureProbability = 0.10,
        DisconnectProbability = 0.05,
        TimeoutProbability = 0.08,
    };
}
