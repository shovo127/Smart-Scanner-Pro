namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the type of scanning mechanism used by the device.
/// </summary>
public enum ScannerType
{
    /// <summary>
    /// Unknown or uninitialized scanner type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A flatbed scanner where the document is placed on a glass surface.
    /// </summary>
    Flatbed = 1,

    /// <summary>
    /// An Automatic Document Feeder (ADF) that pulls pages automatically.
    /// </summary>
    AutomaticDocumentFeeder = 2,

    /// <summary>
    /// A scanner capable of scanning both sides of a page simultaneously.
    /// </summary>
    Duplex = 3,
}
