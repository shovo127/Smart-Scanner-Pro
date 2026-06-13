namespace SmartScannerPro.Scanner.Engine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;

/// <summary>
/// The central orchestrator that coordinates multiple scanner providers.
/// </summary>
public sealed class ScannerEngine : IScannerEngine
{
    private readonly IEnumerable<IScannerProvider> providers;
    private readonly IScannerDiscoveryService discovery;
    private readonly IScannerFactory factory;
    private readonly IScannerDiagnostics diagnostics;
    private readonly IScannerHealthMonitor healthMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerEngine"/> class.
    /// </summary>
    /// <param name="providers">The registered scanner providers.</param>
    /// <param name="discovery">The consolidated discovery service.</param>
    /// <param name="factory">The consolidated session factory.</param>
    /// <param name="diagnostics">The consolidated diagnostics service.</param>
    /// <param name="healthMonitor">The consolidated health monitor.</param>
    public ScannerEngine(
        IEnumerable<IScannerProvider> providers,
        IScannerDiscoveryService discovery,
        IScannerFactory factory,
        IScannerDiagnostics diagnostics,
        IScannerHealthMonitor healthMonitor)
    {
        this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
        this.discovery = discovery ?? throw new ArgumentNullException(nameof(discovery));
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        this.healthMonitor = healthMonitor ?? throw new ArgumentNullException(nameof(healthMonitor));
    }

    /// <inheritdoc/>
    public IScannerDiscoveryService Discovery => this.discovery;

    /// <inheritdoc/>
    public IScannerFactory Factory => this.factory;

    /// <inheritdoc/>
    public IScannerDiagnostics Diagnostics => this.diagnostics;

    /// <inheritdoc/>
    public IReadOnlyList<IScannerProvider> Providers => this.providers.ToList().AsReadOnly();

    /// <inheritdoc/>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        foreach (var provider in this.providers)
        {
            if (provider.IsSupported)
            {
                await provider.InitializeAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // Trigger health monitoring start if it's supported by the concrete health monitor
        if (this.healthMonitor is Diagnostics.ScannerHealthMonitor concreteHealthMonitor)
        {
            concreteHealthMonitor.StartMonitoring();
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (this.healthMonitor is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else if (this.healthMonitor is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
