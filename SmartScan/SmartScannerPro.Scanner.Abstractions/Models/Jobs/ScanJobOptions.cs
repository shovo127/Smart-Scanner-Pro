namespace SmartScannerPro.Scanner.Abstractions.Models.Jobs;

using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Provides options for executing a specific scan job within a session.
/// </summary>
public record ScanJobOptions
{
    /// <summary>
    /// Gets a value indicating whether this is a background job or interactive.
    /// </summary>
    public bool IsBackgroundJob { get; init; }

    /// <summary>
    /// Gets the session options from which this job was derived.
    /// </summary>
    public required ScanSessionOptions SessionOptions { get; init; }

    /// <summary>
    /// Gets a value indicating whether to prompt the user for more pages.
    /// </summary>
    public bool PromptForMorePages { get; init; }
}
