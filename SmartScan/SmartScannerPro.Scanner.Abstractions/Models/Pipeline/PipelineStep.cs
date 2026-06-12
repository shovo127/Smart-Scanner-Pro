namespace SmartScannerPro.Scanner.Abstractions.Models.Pipeline;

/// <summary>
/// Specifies the different stages in the scanner image processing pipeline.
/// </summary>
public enum PipelineStep
{
    /// <summary>
    /// The step where images are acquired from the scanner hardware.
    /// </summary>
    Acquire = 0,

    /// <summary>
    /// The step where raw images are pre-processed before main processing.
    /// </summary>
    PreProcess = 1,

    /// <summary>
    /// The main image processing step (e.g., enhancements, deskew).
    /// </summary>
    Process = 2,

    /// <summary>
    /// The step for post-processing (e.g., OCR, final formatting).
    /// </summary>
    PostProcess = 3,

    /// <summary>
    /// The step where the final output is exported to the destination.
    /// </summary>
    Export = 4
}
