namespace SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Represents the progress of a running scan.
/// </summary>
public record ScanProgress
{
    /// <summary>
    /// Gets the overall percentage complete (0-100).
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
