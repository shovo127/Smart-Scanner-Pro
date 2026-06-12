namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Defines the high-level workflow orchestration for a scanning operation.
/// </summary>
public interface IScanWorkflow
{
    /// <summary>
    /// Executes the entire scan workflow asynchronously.
    /// </summary>
    /// <param name="options">The options for the scan session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(ScanSessionOptions options, CancellationToken cancellationToken = default);
}
