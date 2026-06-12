namespace SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Specifies the health condition of the scanner driver.
/// </summary>
public enum DriverHealth
{
    /// <summary>
    /// Unknown health state.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Driver is healthy and operating normally.
    /// </summary>
    Healthy = 1,

    /// <summary>
    /// Driver is experiencing minor issues or degraded performance.
    /// </summary>
    Degraded = 2,

    /// <summary>
    /// Driver has failed and cannot perform operations.
    /// </summary>
    Failed = 3
}
