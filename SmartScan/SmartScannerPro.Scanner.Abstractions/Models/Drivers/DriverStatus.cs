namespace SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Specifies the current status of the scanner driver.
/// </summary>
public enum DriverStatus
{
    /// <summary>
    /// The driver is unloaded.
    /// </summary>
    Unloaded = 0,

    /// <summary>
    /// The driver is loaded and ready.
    /// </summary>
    Ready = 1,

    /// <summary>
    /// The driver is currently busy (e.g., scanning).
    /// </summary>
    Busy = 2,

    /// <summary>
    /// The driver is in an error state.
    /// </summary>
    Error = 3,

    /// <summary>
    /// The driver is offline or not responding.
    /// </summary>
    Offline = 4
}
