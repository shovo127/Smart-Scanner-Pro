namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the compression algorithm used for the output.
/// </summary>
public enum CompressionMode
{
    /// <summary>
    /// No compression.
    /// </summary>
    None = 0,

    /// <summary>
    /// Lossless Lempel-Ziv-Welch compression.
    /// </summary>
    Lzw = 1,

    /// <summary>
    /// Lossy JPEG compression.
    /// </summary>
    Jpeg = 2,

    /// <summary>
    /// DEFLATE (Zip) compression.
    /// </summary>
    Zip = 3,
}
