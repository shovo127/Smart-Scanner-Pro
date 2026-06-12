namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies whether to scan one or both sides of a page.
/// </summary>
public enum DuplexMode
{
    /// <summary>
    /// Scan only one side of the page.
    /// </summary>
    Simplex = 0,

    /// <summary>
    /// Scan both sides of the page.
    /// </summary>
    Duplex = 1,
}
