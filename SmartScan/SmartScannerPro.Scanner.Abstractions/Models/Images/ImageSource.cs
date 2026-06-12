namespace SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Specifies the physical source on the scanner from which the image was acquired.
/// </summary>
public enum ImageSource
{
    /// <summary>
    /// Unknown image source.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The scanner flatbed glass.
    /// </summary>
    Flatbed = 1,

    /// <summary>
    /// The Automatic Document Feeder (ADF) front side.
    /// </summary>
    AdfFront = 2,

    /// <summary>
    /// The Automatic Document Feeder (ADF) back side (duplex).
    /// </summary>
    AdfBack = 3
}
