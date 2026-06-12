namespace SmartScannerPro.Scanner.Mock.Configuration;

/// <summary>
/// Describes the timing model used by a mock scanner profile.
/// </summary>
public enum MockPerformanceProfileType
{
    /// <summary>Fast consumer ADF scanning.</summary>
    Fast = 0,

    /// <summary>Slow flatbed scanning.</summary>
    Slow = 1,

    /// <summary>Variable network latency.</summary>
    Network = 2,

    /// <summary>Fault-prone device timing.</summary>
    Faulty = 3,

    /// <summary>High-throughput enterprise timing.</summary>
    Enterprise = 4,
}
