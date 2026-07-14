namespace SmartScannerPro.Application.ScanWorkflow.Models;

using System;
using System.Collections.Generic;

/// <summary>
/// Tracks the mutable internal state of a running scanning workflow.
/// Created at the start of each workflow execution and discarded on completion.
/// </summary>
internal sealed class WorkflowContext
{
    /// <summary>
    /// Gets the unique identifier for this workflow execution.
    /// </summary>
    public Guid WorkflowId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the original request that initiated this workflow.
    /// </summary>
    public required WorkflowRequest Request { get; init; }

    /// <summary>
    /// Gets the UTC timestamp when this workflow was started.
    /// </summary>
    public DateTimeOffset StartedAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the current phase of the workflow.
    /// </summary>
    public WorkflowState State { get; set; } = WorkflowState.Created;

    /// <summary>
    /// Gets the accumulating list of pages produced so far.
    /// </summary>
    public List<WorkflowPage> Pages { get; } = new();

    /// <summary>
    /// Gets or sets the number of page-level retries attempted.
    /// </summary>
    public int RetryCount { get; set; }
}
