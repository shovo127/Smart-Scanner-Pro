namespace SmartScannerPro.Scanner.Mock.Simulation;

using System;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Mock.Configuration;

/// <summary>
/// Evaluates failure probabilities and decides whether a simulated failure should
/// be injected into the current scan operation.
/// </summary>
public static class MockFailureSimulator
{
    /// <summary>
    /// Determines whether a specific failure type should be triggered, based on
    /// its configured probability.
    /// </summary>
    /// <param name="probability">The probability value in the range [0.0, 1.0].</param>
    /// <returns><c>true</c> if the failure should be triggered; otherwise, <c>false</c>.</returns>
    public static bool ShouldFail(double probability)
    {
        if (probability <= 0.0)
        {
            return false;
        }

        if (probability >= 1.0)
        {
            return true;
        }

        return Random.Shared.NextDouble() < probability;
    }

    /// <summary>
    /// Evaluates the pre-scan failure conditions (cover open, out of paper, driver failure).
    /// These failures are checked before a scan session starts.
    /// </summary>
    /// <param name="options">The failure simulation options to evaluate.</param>
    /// <returns>The first triggered <see cref="FailureReason"/>, or <see cref="FailureReason.None"/>.</returns>
    public static FailureReason EvaluatePreScanFailures(FailureSimulationOptions options)
    {
        if (ShouldFail(options.CoverOpenProbability))
        {
            return FailureReason.CoverOpen;
        }

        if (ShouldFail(options.OutOfPaperProbability))
        {
            return FailureReason.OutOfPaper;
        }

        if (ShouldFail(options.DriverFailureProbability))
        {
            return FailureReason.DriverError;
        }

        if (ShouldFail(options.LowMemoryProbability))
        {
            return FailureReason.HardwareError;
        }

        return FailureReason.None;
    }

    /// <summary>
    /// Evaluates the mid-scan failure conditions (paper jam, disconnect, timeout).
    /// These failures are checked per page during active scanning.
    /// </summary>
    /// <param name="options">The failure simulation options to evaluate.</param>
    /// <returns>The first triggered <see cref="FailureReason"/>, or <see cref="FailureReason.None"/>.</returns>
    public static FailureReason EvaluateMidScanFailures(FailureSimulationOptions options)
    {
        if (ShouldFail(options.PaperJamProbability))
        {
            return FailureReason.PaperJam;
        }

        if (ShouldFail(options.DisconnectProbability))
        {
            return FailureReason.DeviceOffline;
        }

        if (ShouldFail(options.TimeoutProbability))
        {
            return FailureReason.Timeout;
        }

        if (ShouldFail(options.RandomFailureProbability))
        {
            return FailureReason.UnknownError;
        }

        return FailureReason.None;
    }

    /// <summary>
    /// Builds a failed <see cref="ScanJobResult"/> for the given failure reason and statistics.
    /// </summary>
    /// <param name="reason">The failure reason to embed.</param>
    /// <param name="statistics">The statistics captured up to the point of failure.</param>
    /// <param name="exception">An optional exception to embed in the result.</param>
    /// <returns>A completed failure result record.</returns>
    public static ScanJobResult CreateFailedResult(
        FailureReason reason,
        ScanStatistics statistics,
        Exception? exception = null)
    {
        return new ScanJobResult
        {
            Status = ScanJobStatus.Failed,
            FailureReason = reason,
            Statistics = statistics,
            Exception = exception,
        };
    }
}
