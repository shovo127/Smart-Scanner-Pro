namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the PDF standard version for export.
/// </summary>
public enum PDFVersion
{
    /// <summary>
    /// PDF version 1.4
    /// </summary>
    V1_4 = 0,

    /// <summary>
    /// PDF version 1.5
    /// </summary>
    V1_5 = 1,

    /// <summary>
    /// PDF version 1.6
    /// </summary>
    V1_6 = 2,

    /// <summary>
    /// PDF version 1.7
    /// </summary>
    V1_7 = 3,

    /// <summary>
    /// PDF/A standard for long-term archiving.
    /// </summary>
    PdfA = 4,
}
