namespace SmartScannerPro.Scanner.Abstractions.Models.Pipeline;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a single step in the scanner image processing pipeline.
/// </summary>
public interface IPipelineStep
{
    /// <summary>
    /// Gets the type of pipeline step.
    /// </summary>
    PipelineStep StepType { get; }

    /// <summary>
    /// Executes this pipeline step with the given context.
    /// </summary>
    /// <param name="context">The pipeline context containing state.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the pipeline step execution.</returns>
    Task<PipelineResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default);
}
