namespace SmartScannerPro.Application.ScanWorkflow.Models;

using System;

/// <summary>
/// Represents a single page produced by the scanning workflow.
/// </summary>
public sealed class WorkflowPage
{
    /// <summary>
    /// Gets the 1-based final page number in the assembled document.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Gets the absolute path to the full-resolution PNG image in the staging folder.
    /// </summary>
    public string StagingFilePath { get; init; } = string.Empty;

    /// <summary>
    /// Gets the absolute path to the generated JPEG thumbnail image.
    /// </summary>
    public string ThumbnailPath { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating the origin of this page within the workflow
    /// (e.g., front side, back side, or single-side scan).
    /// </summary>
    public PageSource Source { get; init; } = PageSource.Single;

    /// <summary>
    /// Gets the UTC timestamp when this page was acquired.
    /// </summary>
    public DateTimeOffset ScannedAt { get; init; } = DateTimeOffset.UtcNow;
}
