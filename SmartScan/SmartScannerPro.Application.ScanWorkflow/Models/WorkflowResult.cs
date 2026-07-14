namespace SmartScannerPro.Application.ScanWorkflow.Models;

using System;
using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;

/// <summary>
/// Represents the outcome of a completed scanning workflow.
/// </summary>
public sealed class WorkflowResult
{
    /// <summary>
    /// Gets the overall status of the workflow.
    /// </summary>
    public WorkflowStatus Status { get; init; }

    /// <summary>
    /// Gets the ordered list of pages produced by the workflow.
    /// </summary>
    /// <remarks>
    /// For Manual Duplex workflows, pages are already interleaved in the correct reading order
    /// (front 1, back 1, front 2, back 2, ...).
    /// </remarks>
    public IReadOnlyList<WorkflowPage> Pages { get; init; } = Array.Empty<WorkflowPage>();

    /// <summary>
    /// Gets the total number of pages scanned during the workflow.
    /// </summary>
    public int TotalPagesScanned => this.Pages.Count;

    /// <summary>
    /// Gets the total elapsed time for the workflow.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Gets the failure reason if the workflow failed.
    /// </summary>
    public FailureReason? FailureReason { get; init; }

    /// <summary>
    /// Gets the exception that caused the workflow to fail, if applicable.
    /// </summary>
    public Exception? Exception { get; init; }
}
