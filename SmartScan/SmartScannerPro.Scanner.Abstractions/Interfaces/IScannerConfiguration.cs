namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Manages the configuration settings for a scanner device.
/// </summary>
public interface IScannerConfiguration
{
    /// <summary>
    /// Applies a set of configuration values to the scanner.
    /// </summary>
    /// <param name="settings">A dictionary of capability keys and values to apply.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ApplyConfigurationAsync(System.Collections.Generic.IReadOnlyDictionary<string, object> settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the scanner configuration to its factory defaults.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ResetToDefaultsAsync(CancellationToken cancellationToken = default);
}
