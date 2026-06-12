namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the Optical Character Recognition (OCR) provider.
/// </summary>
public enum OCRProvider
{
    /// <summary>
    /// Use Tesseract OCR engine.
    /// </summary>
    Tesseract = 0,

    /// <summary>
    /// Use the native Windows OCR API.
    /// </summary>
    WindowsOCR = 1,
}
