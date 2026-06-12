namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Capabilities;

/// <summary>
/// Provides access to query and modify scanner capabilities.
/// </summary>
public interface IScannerCapabilities
{
    /// <summary>
    /// Gets the complete set of capabilities supported by the scanner.
    /// </summary>
    CapabilitySet SupportedCapabilities { get; }

    /// <summary>
    /// Refreshes the capabilities from the physical device.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RefreshAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the value of a specific capability.
    /// </summary>
    /// <typeparam name="T">The data type of the capability value.</typeparam>
    /// <param name="capabilityId">The identifier of the capability.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the capability was successfully set; otherwise, false.</returns>
    Task<bool> SetCapabilityValueAsync<T>(string capabilityId, T value, CancellationToken cancellationToken = default);
}
