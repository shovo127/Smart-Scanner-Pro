namespace SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Specifies the image format used during acquisition.
/// </summary>
public enum ImageFormat
{
    /// <summary>
    /// Unknown image format.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Raw uncompressed memory bitmap (DIB).
    /// </summary>
    MemoryBitmap = 1,

    /// <summary>
    /// Uncompressed TIFF format.
    /// </summary>
    Tiff = 2,

    /// <summary>
    /// Compressed JPEG format.
    /// </summary>
    Jpeg = 3,

    /// <summary>
    /// Compressed PNG format.
    /// </summary>
    Png = 4
}
