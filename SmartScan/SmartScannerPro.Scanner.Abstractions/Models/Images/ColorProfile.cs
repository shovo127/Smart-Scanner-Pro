namespace SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Specifies the color profile of the acquired image.
/// </summary>
public enum ColorProfile
{
    /// <summary>
    /// Unknown color profile.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Black and white (1-bit).
    /// </summary>
    BlackAndWhite = 1,

    /// <summary>
    /// Grayscale (8-bit or 16-bit).
    /// </summary>
    Grayscale = 2,

    /// <summary>
    /// Full color (RGB, 24-bit or 48-bit).
    /// </summary>
    Color = 3
}
