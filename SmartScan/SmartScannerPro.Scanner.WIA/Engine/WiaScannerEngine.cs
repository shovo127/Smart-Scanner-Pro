namespace SmartScannerPro.Scanner.WIA.Engine;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.WIA.Diagnostics;
using SmartScannerPro.Scanner.WIA.Discovery;
using SmartScannerPro.Scanner.WIA.Factory;

/// <summary>
/// The top-level orchestrator for the WIA Scanner SDK.
/// </summary>
public sealed class WiaScannerEngine : IScannerEngine
{
    private readonly WiaScannerProvider provider;
    private readonly WiaScannerDiscoveryService discoveryService;
    private readonly WiaScannerFactory factory;
    private readonly WiaScannerDiagnostics diagnostics;
    private readonly WiaScannerHealthMonitor healthMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="WiaScannerEngine"/> class.
    /// </summary>
    /// <param name="provider">The WIA provider.</param>
    /// <param name="discoveryService">The WIA discovery service.</param>
    /// <param name="factory">The WIA session factory.</param>
    /// <param name="diagnostics">The WIA diagnostics.</param>
    /// <param name="healthMonitor">The WIA health monitor.</param>
    public WiaScannerEngine(
        WiaScannerProvider provider,
        WiaScannerDiscoveryService discoveryService,
        WiaScannerFactory factory,
        WiaScannerDiagnostics diagnostics,
        WiaScannerHealthMonitor healthMonitor)
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
