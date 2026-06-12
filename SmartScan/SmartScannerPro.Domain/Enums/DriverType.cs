namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the driver protocol used to communicate with the scanner.
/// </summary>
public enum DriverType
{
    /// <summary>
    /// Unknown driver protocol.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// TWAIN API protocol.
    /// </summary>
    Twain = 1,

    /// <summary>
    /// Windows Image Acquisition (WIA) protocol.
    /// </summary>
    Wia = 2,

    /// <summary>
    /// eSCL (AirScan) network protocol.
    /// </summary>
    ESCL = 3,

    /// <summary>
    /// A simulated driver for testing and development.
    /// </summary>
    Simulated = 99,
}
