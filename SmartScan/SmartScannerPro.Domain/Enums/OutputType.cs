namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the primary output format for a scan job.
/// </summary>
public enum OutputType
{
    /// <summary>
    /// Output as a Portable Document Format (PDF) file.
    /// </summary>
    Pdf = 0,

    /// <summary>
    /// Output as individual image files.
    /// </summary>
    Image = 1,
}
