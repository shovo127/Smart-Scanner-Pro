namespace SmartScannerPro.Scanner.Factory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Routes scan session creation to the appropriate provider factory based on the hardware identifier.
/// Resolves the correct provider by iterating all registered <see cref="IScannerProviderFactory"/>
/// implementations and delegating to the first one that reports it can handle the hardware ID.
/// This implementation is completely decoupled from concrete provider types.
/// </summary>
public sealed class ScannerFactory : IScannerFactory
{
    private readonly IReadOnlyList<IScannerProviderFactory> providerFactories;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerFactory"/> class.
    /// </summary>
    /// <param name="providerFactories">
    /// The collection of provider factories registered by individual scanner backends
    /// (e.g., WIA, Mock, TWAIN). Each factory self-declares which hardware IDs it supports.
    /// </param>
    public ScannerFactory(IEnumerable<IScannerProviderFactory> providerFactories)
    {
        if (providerFactories == null)
        {
            throw new ArgumentNullException(nameof(providerFactories));
        }

        this.providerFactories = providerFactories.ToList().AsReadOnly();
    }

    /// <inheritdoc/>
    public Task<IScannerSession> CreateSessionAsync(
        ScanSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var factory = this.providerFactories.FirstOrDefault(f => f.CanHandle(options.HardwareId));

        if (factory == null)
        {
            throw new InvalidOperationException(
                $"No registered scanner provider factory can handle hardware ID '{options.HardwareId}'. " +
                "Ensure the appropriate provider (e.g., AddMockScanner, AddWiaScanner) has been registered " +
                "in the application's dependency injection composition root.");
        }

        return factory.CreateSessionAsync(options, cancellationToken);
    }
}
