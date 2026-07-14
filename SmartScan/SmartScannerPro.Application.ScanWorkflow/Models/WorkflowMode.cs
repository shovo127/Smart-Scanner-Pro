namespace SmartScannerPro.Application.ScanWorkflow.Models;

/// <summary>
/// Specifies the scanning workflow mode that determines how pages are acquired.
/// </summary>
public enum WorkflowMode
{
    /// <summary>
    /// Acquires a single page from a flatbed or ADF. Stops after one page.
    /// </summary>
    Single = 0,

    /// <summary>
    /// Acquires multiple pages one at a time, prompting the user after each page.
    /// Suitable for flatbed scanners performing multi-page document scans.
    /// </summary>
    Multipage = 1,

    /// <summary>
    /// Acquires all pages from an Automatic Document Feeder until the feeder is empty.
    /// </summary>
    Adf = 2,

    /// <summary>
    /// Acquires the front side of all pages, prompts the user to flip the stack,
    /// then acquires the back sides. Pages are interleaved to produce a correct duplex document.
    /// Suitable for simplex ADF scanners performing duplex document scans.
    /// </summary>
    ManualDuplex = 3,
}
