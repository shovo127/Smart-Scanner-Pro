namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Factory for creating scanner sessions and orchestrating dependency resolution.
/// </summary>
public interface IScannerFactory
{
    /// <summary>
    /// Creates a new scanning session for the specified hardware.
    /// </summary>
    /// <param name="options">The session options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A new, unstarted scanner session.</returns>
    Task<IScannerSession> CreateSessionAsync(ScanSessionOptions options, CancellationToken cancellationToken = default);
}
