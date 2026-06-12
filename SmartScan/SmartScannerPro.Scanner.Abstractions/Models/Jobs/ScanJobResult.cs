namespace SmartScannerPro.Scanner.Abstractions.Models.Jobs;

using System;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Represents the outcome of a scan job.
/// </summary>
public record ScanJobResult
{
    /// <summary>
    /// Gets the status of the job.
    /// </summary>
    public ScanJobStatus Status { get; init; }

    /// <summary>
    /// Gets the document identifier produced by this job, if successful.
    /// </summary>
    public DocumentId? ProducedDocumentId { get; init; }

    /// <summary>
    /// Gets the statistics gathered during the job.
    /// </summary>
    public required ScanStatistics Statistics { get; init; }

    /// <summary>
    /// Gets the failure reason, if the job failed.
    /// </summary>
    public FailureReason FailureReason { get; init; } = FailureReason.None;

    /// <summary>
    /// Gets the exception that caused the failure, if applicable.
    /// </summary>
    public Exception? Exception { get; init; }
}
