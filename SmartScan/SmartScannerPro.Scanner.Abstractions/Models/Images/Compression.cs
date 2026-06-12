namespace SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Specifies the compression applied to the acquired image data.
/// </summary>
public enum Compression
{
    /// <summary>
    /// No compression applied.
    /// </summary>
    None = 0,

    /// <summary>
    /// JPEG compression.
    /// </summary>
    Jpeg = 1,

    /// <summary>
    /// Group 4 compression (often used for black and white TIFFs).
    /// </summary>
    Group4 = 2,

    /// <summary>
    /// LZW compression.
    /// </summary>
    Lzw = 3
}
