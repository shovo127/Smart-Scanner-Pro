namespace SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Specifies the type of the scanner driver.
/// </summary>
public enum DriverType
{
    /// <summary>
    /// Unknown or unclassified driver.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// WIA (Windows Image Acquisition) driver.
    /// </summary>
    WIA = 1,

    /// <summary>
    /// TWAIN driver.
    /// </summary>
    TWAIN = 2,

    /// <summary>
    /// eSCL (Apple AirPrint Scanning) driver.
    /// </summary>
    ESCL = 3,

    /// <summary>
    /// SANE (Scanner Access Now Easy) driver.
    /// </summary>
    SANE = 4,

    /// <summary>
    /// Virtual or mock driver for testing.
    /// </summary>
    Mock = 99
}
