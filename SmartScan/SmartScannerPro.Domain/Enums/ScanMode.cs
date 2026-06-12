namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the color depth mode for scanning.
/// </summary>
public enum ScanMode
{
    /// <summary>
    /// Full color scanning (typically 24-bit).
    /// </summary>
    Color = 0,

    /// <summary>
    /// Grayscale scanning (typically 8-bit).
    /// </summary>
    Grayscale = 1,

    /// <summary>
    /// Black and white scanning (typically 1-bit).
    /// </summary>
    BlackAndWhite = 2,
}
