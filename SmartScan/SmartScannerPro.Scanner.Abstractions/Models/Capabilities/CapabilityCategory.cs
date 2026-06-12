namespace SmartScannerPro.Scanner.Abstractions.Models.Capabilities;

/// <summary>
/// Represents the logical grouping of scanner capabilities.
/// </summary>
public enum CapabilityCategory
{
    /// <summary>
    /// Unknown capability category.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// General device capabilities.
    /// </summary>
    Device = 1,

    /// <summary>
    /// Image acquisition and processing capabilities.
    /// </summary>
    Image = 2,

    /// <summary>
    /// Document handling capabilities (e.g., ADF, Flatbed).
    /// </summary>
    DocumentHandling = 3,

    /// <summary>
    /// Hardware-specific capabilities.
    /// </summary>
    Hardware = 4
}
