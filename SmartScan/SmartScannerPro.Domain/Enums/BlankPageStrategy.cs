namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies how blank pages are handled during scanning.
/// </summary>
public enum BlankPageStrategy
{
    /// <summary>
    /// Keep blank pages in the document.
    /// </summary>
    Keep = 0,

    /// <summary>
    /// Automatically remove blank pages from the document.
    /// </summary>
    Remove = 1,
}
