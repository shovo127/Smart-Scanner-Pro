namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents an active communication channel with a scanner device.
/// </summary>
public interface IScannerConnection : IAsyncDisposable
{
    /// <summary>
    /// Gets a value indicating whether the connection is currently open and valid.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Opens the connection to the scanner.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the connection to the scanner.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DisconnectAsync(CancellationToken cancellationToken = default);
}
