namespace SmartScannerPro.Scanner.Abstractions.Models.Discovery;

/// <summary>
/// Specifies the hardware connection type of the scanner.
/// </summary>
public enum ConnectionType
{
    /// <summary>
    /// Unknown connection type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// USB connected scanner.
    /// </summary>
    Usb = 1,

    /// <summary>
    /// Network connected scanner (Ethernet/Wi-Fi).
    /// </summary>
    Network = 2,

    /// <summary>
    /// Virtual or mock scanner.
    /// </summary>
    Virtual = 3
}
