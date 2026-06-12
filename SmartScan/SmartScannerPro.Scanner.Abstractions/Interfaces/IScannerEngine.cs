namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides the top-level entry point for all scanner operations.
/// </summary>
public interface IScannerEngine : IAsyncDisposable
{
    /// <summary>
    /// Gets the discovery service for finding scanners.
    /// </summary>
    IScannerDiscoveryService Discovery { get; }

    /// <summary>
    /// Gets the factory for creating scanner sessions.
    /// </summary>
    IScannerFactory Factory { get; }

    /// <summary>
    /// Gets the diagnostics service for monitoring scanner health.
    /// </summary>
    IScannerDiagnostics Diagnostics { get; }

    /// <summary>
    /// Gets the active scanner providers registered with the engine.
    /// </summary>
    IReadOnlyList<IScannerProvider> Providers { get; }

    /// <summary>
    /// Initializes the scanner engine and its providers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
