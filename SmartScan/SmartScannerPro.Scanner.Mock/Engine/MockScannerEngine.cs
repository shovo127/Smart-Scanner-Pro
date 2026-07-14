namespace SmartScannerPro.Scanner.Mock.Engine;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Mock.Diagnostics;
using SmartScannerPro.Scanner.Mock.Discovery;
using SmartScannerPro.Scanner.Mock.Factory;
using SmartScannerPro.Scanner.Mock.Provider;

/// <summary>
/// The top-level orchestrator for the Mock Scanner SDK.
/// Aggregates all services into a single entry point, implementing <see cref="IScannerEngine"/>.
/// </summary>
public sealed class MockScannerEngine : IScannerEngine
{
    private readonly MockScannerProvider provider;
    private readonly MockScannerDiscoveryService discoveryService;
    private readonly MockScannerFactory factory;
    private readonly MockScannerDiagnostics diagnostics;
    private readonly MockScannerHealthMonitor healthMonitor;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerEngine"/>.
    /// </summary>
    /// <param name="provider">The mock scanner provider.</param>
    /// <param name="discoveryService">The mock discovery service.</param>
    /// <param name="factory">The mock session factory.</param>
    /// <param name="diagnostics">The mock diagnostics service.</param>
    /// <param name="healthMonitor">The mock health monitor.</param>
    public MockScannerEngine(
        MockScannerProvider provider,
        MockScannerDiscoveryService discoveryService,
        MockScannerFactory factory,
        MockScannerDiagnostics diagnostics,
        MockScannerHealthMonitor healthMonitor)
    {
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        this.discoveryService = discoveryService ?? throw new ArgumentNullException(nameof(discoveryService));
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        this.healthMonitor = healthMonitor ?? throw new ArgumentNullException(nameof(healthMonitor));
    }

    /// <inheritdoc/>
    public IScannerDiscoveryService Discovery => this.discoveryService;

    /// <inheritdoc/>
    public IScannerFactory Factory => this.factory;

    /// <inheritdoc/>
    public IScannerDiagnostics Diagnostics => this.diagnostics;

    /// <inheritdoc/>
    public IReadOnlyList<IScannerProvider> Providers => new List<IScannerProvider> { this.provider }.AsReadOnly();

    /// <inheritdoc/>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await this.provider.InitializeAsync(cancellationToken).ConfigureAwait(false);
        this.healthMonitor.StartMonitoring();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await this.healthMonitor.DisposeAsync().ConfigureAwait(false);
    }
}
