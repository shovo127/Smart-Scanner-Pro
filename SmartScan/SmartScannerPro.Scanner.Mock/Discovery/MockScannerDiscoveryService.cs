namespace SmartScannerPro.Scanner.Mock.Discovery;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Simulation;

/// <summary>
/// Simulates scanner discovery by enumerating the configured device fleet.
/// Discovery delays are calibrated per performance profile to mimic real hardware
/// negotiation timing. Plug-and-play events are fired during enumeration.
/// </summary>
public sealed class MockScannerDiscoveryService : IScannerDiscoveryService
{
    private readonly MockScannerDeviceRegistry registry;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerDiscoveryService"/>.
    /// </summary>
    /// <param name="registry">The device registry to enumerate.</param>
    public MockScannerDiscoveryService(MockScannerDeviceRegistry registry)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public event EventHandler<ScannerDescriptor>? ScannerDiscovered;

#pragma warning disable CS0067 // ScannerLost is required by the interface; not raised in the mock
    /// <inheritdoc/>
    public event EventHandler<ScannerDescriptor>? ScannerLost;
#pragma warning restore CS0067

    /// <inheritdoc/>
    public async Task<DiscoveryResult> DiscoverAsync(
        DiscoveryRequest request,
        CancellationToken cancellationToken = default)
    {
        var started = DateTimeOffset.UtcNow;
        var discovered = new List<ScannerDescriptor>();
        var timedOut = false;

        using var timeoutCts = request.Timeout > TimeSpan.Zero
            ? new CancellationTokenSource(request.Timeout)
            : new CancellationTokenSource();

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, timeoutCts.Token);

        foreach (var profile in this.registry.GetAllDevices())
        {
            if (linked.IsCancellationRequested)
            {
                timedOut = !cancellationToken.IsCancellationRequested;
                break;
            }

            if (!this.IsAllowed(profile, request))
            {
                continue;
            }

            var perfProfile = MockPerformanceProfile.For(profile.PerformanceProfile);

            try
            {
                await Task.Delay(perfProfile.DiscoveryDelay, linked.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                timedOut = !cancellationToken.IsCancellationRequested;
                break;
            }

            var descriptor = BuildDescriptor(profile);
            discovered.Add(descriptor);
            this.ScannerDiscovered?.Invoke(this, descriptor);
        }

        return new DiscoveryResult
        {
            Scanners = discovered.AsReadOnly(),
            Duration = DateTimeOffset.UtcNow - started,
            TimedOut = timedOut,
        };
    }

    private bool IsAllowed(MockDeviceProfile profile, DiscoveryRequest request)
    {
        if (request.AllowedConnectionTypes is { Count: > 0 })
        {
            var found = false;
            foreach (var ct in request.AllowedConnectionTypes)
            {
                if (ct == profile.ConnectionType)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                return false;
            }
        }

        if (request.AllowedDriverTypes is { Count: > 0 })
        {
            var found = false;
            foreach (var dt in request.AllowedDriverTypes)
            {
                if (dt == DriverType.Mock)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                return false;
            }
        }

        return true;
    }

    private static ScannerDescriptor BuildDescriptor(MockDeviceProfile profile)
    {
        return new ScannerDescriptor
        {
            HardwareId = profile.HardwareId,
            Name = profile.Name,
            Manufacturer = profile.Manufacturer,
            ConnectionType = profile.ConnectionType,
            ConnectionString = profile.ConnectionString,
            Driver = new DriverInfo
            {
                Id = Guid.NewGuid(),
                Name = "Mock Driver",
                Type = DriverType.Mock,
                Version = new DriverVersion { Major = 1, Minor = 0, Build = 0, Revision = 0, OriginalString = "1.0.0.0" },
                Priority = DriverPriority.Preferred,
                Status = DriverStatus.Ready,
                Manufacturer = profile.Manufacturer,
                NativeProvider = "SmartScannerPro.Scanner.Mock",
            },
        };
    }
}
