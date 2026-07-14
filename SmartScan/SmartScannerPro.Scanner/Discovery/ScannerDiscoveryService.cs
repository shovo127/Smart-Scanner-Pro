namespace SmartScannerPro.Scanner.Discovery;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;

/// <summary>
/// Aggregates discovery services across multiple scanner providers.
/// </summary>
public sealed class ScannerDiscoveryService : IScannerDiscoveryService
{
    private readonly IEnumerable<IScannerProvider> providers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerDiscoveryService"/> class.
    /// </summary>
    /// <param name="providers">The registered scanner providers.</param>
    public ScannerDiscoveryService(IEnumerable<IScannerProvider> providers)
    {
        this.providers = providers ?? throw new ArgumentNullException(nameof(providers));

        foreach (var provider in this.providers)
        {
            if (provider.IsSupported)
            {
                var svc = provider.GetDiscoveryService();
                if (svc != null)
                {
                    svc.ScannerDiscovered += (s, e) => this.ScannerDiscovered?.Invoke(this, e);
                    svc.ScannerLost += (s, e) => this.ScannerLost?.Invoke(this, e);
                }
            }
        }
    }

    /// <inheritdoc/>
    public event EventHandler<ScannerDescriptor>? ScannerDiscovered;

    /// <inheritdoc/>
    public event EventHandler<ScannerDescriptor>? ScannerLost;

    /// <inheritdoc/>
    public async Task<DiscoveryResult> DiscoverAsync(DiscoveryRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var stopwatch = Stopwatch.StartNew();
        var tasks = new List<Task<DiscoveryResult>>();
        
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(request.Timeout);

        try
        {
            foreach (var provider in this.providers)
            {
                if (provider.IsSupported)
                {
                    // If specific driver types are requested, check if this provider type is allowed
                    if (request.AllowedDriverTypes != null && 
                        request.AllowedDriverTypes.Count > 0 && 
                        !request.AllowedDriverTypes.Contains(provider.ProviderType))
                    {
                        continue;
                    }

                    var svc = provider.GetDiscoveryService();
                    if (svc != null)
                    {
                        tasks.Add(svc.DiscoverAsync(request, cts.Token));
                    }
                }
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            stopwatch.Stop();

            var scanners = results.SelectMany(r => r.Scanners).ToList();
            var timedOut = results.Any(r => r.TimedOut) || cts.IsCancellationRequested;

            return new DiscoveryResult
            {
                Scanners = scanners.AsReadOnly(),
                Duration = stopwatch.Elapsed,
                TimedOut = timedOut
            };
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            stopwatch.Stop();
            return new DiscoveryResult
            {
                Scanners = Array.Empty<ScannerDescriptor>(),
                Duration = stopwatch.Elapsed,
                TimedOut = true
            };
        }
    }
}
