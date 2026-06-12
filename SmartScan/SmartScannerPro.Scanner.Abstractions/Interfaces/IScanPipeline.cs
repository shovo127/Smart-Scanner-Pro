namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Models.Pipeline;

/// <summary>
/// Represents the execution pipeline for processing scanned images.
/// </summary>
public interface IScanPipeline
{
    /// <summary>
    /// Gets the ordered list of steps registered in the pipeline.
    /// </summary>
    IReadOnlyList<IPipelineStep> Steps { get; }

    /// <summary>
    /// Adds a step to the pipeline.
    /// </summary>
    /// <param name="step">The pipeline step to add.</param>
    void AddStep(IPipelineStep step);

    /// <summary>
    /// Removes a step from the pipeline.
    /// </summary>
    /// <param name="step">The pipeline step to remove.</param>
    /// <returns>True if successfully removed; otherwise, false.</returns>
    bool RemoveStep(IPipelineStep step);

    /// <summary>
    /// Clears all steps from the pipeline.
    /// </summary>
    void Clear();
}
