namespace SmartScannerPro.Scanner.Abstractions.Models.Pipeline;

using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Context;
using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Holds the context and state of an item flowing through the pipeline.
/// </summary>
public record PipelineContext
{
    /// <summary>
    /// Gets the core operation context for the current session.
    /// </summary>
    public required OperationContext OperationContext { get; init; }

    /// <summary>
    /// Gets the raw image currently being processed.
    /// </summary>
    public RawImage? CurrentImage { get; init; }

    /// <summary>
    /// Gets a dictionary of properties to share state between pipeline steps.
    /// </summary>
    public IDictionary<string, object> Properties { get; init; } = new Dictionary<string, object>();
}
