namespace SmartScannerPro.Scanner.Abstractions.Models.Pipeline;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents the result of a single pipeline step or the entire pipeline.
/// </summary>
public record PipelineResult
{
    /// <summary>
    /// Gets a value indicating whether the pipeline step was successful.
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    /// Gets the list of error messages, if any.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets the exception that caused the failure, if applicable.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets the duration taken to execute the pipeline step.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="duration">The execution duration.</param>
    /// <returns>A successful pipeline result.</returns>
    public static PipelineResult Success(TimeSpan duration) => new() { IsSuccessful = true, Duration = duration };

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <param name="exception">The optional exception.</param>
    /// <returns>A failed pipeline result.</returns>
    public static PipelineResult Failure(string error, Exception? exception = null) =>
        new() { IsSuccessful = false, Errors = new[] { error }, Exception = exception };
}
