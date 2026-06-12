namespace SmartScannerPro.Application.Interfaces.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides application and scanner diagnostics.
/// </summary>
public interface IDiagnosticsService
{
    /// <summary>
    /// Gathers system and hardware diagnostics.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dictionary of diagnostic information.</returns>
    Task<IDictionary<string, string>> GetSystemDiagnosticsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a hardware test on a specific scanner.
    /// </summary>
    /// <param name="scannerId">The scanner identifier as a string.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the scanner passes the test; otherwise, false.</returns>
    Task<bool> TestScannerHardwareAsync(string scannerId, CancellationToken cancellationToken = default);
}
