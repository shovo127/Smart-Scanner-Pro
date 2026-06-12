namespace SmartScannerPro.TestAssets;

/// <summary>
/// Identifies reusable synthetic documents generated for scanner integration tests.
/// </summary>
public enum DocumentKind
{
    /// <summary>
    /// Commercial invoice document.
    /// </summary>
    Invoice = 0,

    /// <summary>
    /// Narrow retail receipt document.
    /// </summary>
    Receipt = 1,

    /// <summary>
    /// Legal contract document.
    /// </summary>
    Contract = 2,

    /// <summary>
    /// Passport identity-page document.
    /// </summary>
    Passport = 3,

    /// <summary>
    /// QR code page.
    /// </summary>
    QR = 4,

    /// <summary>
    /// Barcode page.
    /// </summary>
    Barcode = 5,

    /// <summary>
    /// Blank white page.
    /// </summary>
    BlankPage = 6,

    /// <summary>
    /// Book page.
    /// </summary>
    Book = 7,
}
