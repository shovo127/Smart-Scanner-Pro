namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Represents a provider-specific factory that can create scanner sessions
/// for a particular hardware backend (e.g., WIA, Mock, TWAIN).
/// </summary>
/// <remarks>
/// Each scanner provider (WIA, Mock, TWAIN, eSCL) registers its own implementation
/// of this interface. The unified <c>ScannerFactory</c> resolves sessions by iterating
/// all registered provider factories and delegating to the first one that reports it
/// can handle the requested hardware identifier.
/// </remarks>
public interface IScannerProviderFactory
{
    /// <summary>
    /// Gets the human-readable name of this provider (e.g., "WIA", "Mock").
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Determines whether this factory can create a session for the specified hardware identifier.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier for the target scanner device.</param>
    /// <returns><see langword="true"/> if this factory supports the given hardware identifier; otherwise, <see langword="false"/>.</returns>
    bool CanHandle(string hardwareId);

    /// <summary>
    /// Creates a new scanner session for the specified hardware device.
    /// </summary>
    /// <param name="options">The session configuration options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that resolves to the newly created <see cref="IScannerSession"/>.</returns>
    Task<IScannerSession> CreateSessionAsync(ScanSessionOptions options, CancellationToken cancellationToken = default);
}
