namespace SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Represents the progress of a running scan.
/// </summary>
public record ScanProgress
{
    /// <summary>
    /// Gets the current stage of the scan operation.
    /// </summary>
    public ScanStage Stage { get; init; } = ScanStage.Connecting;

    /// <summary>
    /// Gets the overall percentage complete derived from the current stage.
    /// </summary>
    public int Percentage { get; init; }

    /// <summary>
    /// Gets the current page number being scanned or processed.
    /// </summary>
    public int CurrentPage { get; init; }

    /// <summary>
    /// Gets a user-friendly message describing the current operation.
    /// </summary>
    public string Message { get; init; } = string.Empty;
}
