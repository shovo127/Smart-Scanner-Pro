namespace SmartScannerPro.Application.ScanWorkflow.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Encapsulates all parameters required to execute a scanning workflow.
/// </summary>
public sealed class WorkflowRequest
{
    /// <summary>
    /// Gets the hardware identifier of the target scanner device.
    /// </summary>
    public required string HardwareId { get; init; }

    /// <summary>
    /// Gets the scanning workflow mode (Single, Multipage, ADF, ManualDuplex).
    /// </summary>
    public WorkflowMode Mode { get; init; } = WorkflowMode.Single;

    /// <summary>
    /// Gets the color mode setting (e.g., <c>"Color"</c>, <c>"Grayscale"</c>, <c>"BlackAndWhite"</c>).
    /// </summary>
    public string ColorMode { get; init; } = "Color";

    /// <summary>
    /// Gets the scan resolution in dots per inch.
    /// </summary>
    public int Resolution { get; init; } = 300;

    /// <summary>
    /// Gets the paper size identifier (e.g., <c>"A4"</c>, <c>"Letter"</c>, <c>"Legal"</c>).
    /// </summary>
    public string PaperSize { get; init; } = "A4";

    /// <summary>
    /// Gets the document source (e.g., <c>"Flatbed"</c>, <c>"AdfFront"</c>).
    /// </summary>
    public string Source { get; init; } = "Flatbed";

    /// <summary>
    /// Gets the maximum number of pages to acquire.
    /// When <see langword="null"/>, the workflow will continue until the feeder
    /// is empty or the user stops acquisition.
    /// </summary>
    public int? MaxPages { get; init; }

    /// <summary>
    /// Gets a session-level timeout. When <see langword="null"/>, the default timeout is used.
    /// </summary>
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Gets an optional asynchronous callback invoked during Manual Duplex scanning to prompt
    /// the user to flip the page stack. The callback should return <see langword="true"/> to
    /// continue scanning the back side, or <see langword="false"/> to cancel.
    /// </summary>
    /// <remarks>
    /// Required when <see cref="Mode"/> is <see cref="WorkflowMode.ManualDuplex"/>.
    /// When <see langword="null"/> in ManualDuplex mode, the workflow will throw
    /// <see cref="InvalidOperationException"/> at runtime.
    /// </remarks>
    public Func<CancellationToken, Task<bool>>? FlipPromptCallback { get; init; }
}
