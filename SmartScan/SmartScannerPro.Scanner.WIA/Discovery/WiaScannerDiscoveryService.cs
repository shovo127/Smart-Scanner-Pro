namespace SmartScannerPro.Scanner.WIA.Discovery;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.WIA.Helpers;

/// <summary>
/// Discovers local and network scanners using Windows Image Acquisition (WIA 2.0).
/// Uses late-bound dynamic COM calls to eliminate compile-time dependencies on MSBuild COM resolution.
/// </summary>
public sealed class WiaScannerDiscoveryService : IScannerDiscoveryService
{
    /// <inheritdoc/>
    public event EventHandler<ScannerDescriptor>? ScannerDiscovered;

#pragma warning disable CS0067 // ScannerLost event is required by the interface but not raised in this basic PnP implementation.
    /// <inheritdoc/>
    public event EventHandler<ScannerDescriptor>? ScannerLost;
#pragma warning restore CS0067

    /// <inheritdoc/>
    public async Task<DiscoveryResult> DiscoverAsync(
        DiscoveryRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var started = DateTimeOffset.UtcNow;
        var discovered = new List<ScannerDescriptor>();
        var timedOut = false;

        try
        {
            var scanners = await StaThread.RunAsync(() =>
            {
                var list = new List<ScannerDescriptor>();
                var deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
                if (deviceManagerType == null)
                {
                    return list;
                }

                dynamic deviceManager = Activator.CreateInstance(deviceManagerType)!;
                dynamic deviceInfos = deviceManager.DeviceInfos;

                for (int i = 1; i <= deviceInfos.Count; i++)
                {
                    dynamic info = deviceInfos[i];
                    int type = info.Type;

                    // 1 represents a scanner device in WIA
                    if (type != 1)
                    {
                        continue;
                    }

                    dynamic props = info.Properties;
                    string hardwareId = info.DeviceID;
                    var name = GetProperty<string>(props, 7, "WIA Scanner"); // WIA_DIP_DEV_NAME
                    var manufacturer = GetProperty<string>(props, 3, "Generic"); // WIA_DIP_DEV_MANUFACTURER
                    var port = GetProperty<string>(props, 6, string.Empty); // WIA_DIP_PORT_NAME
                    var driverVersionStr = GetProperty<string>(props, 15, "1.0.0.0"); // WIA_DIP_DRIVER_VERSION

                    var connectionType = ConnectionType.Usb;
                    if (port.Contains("IP_", StringComparison.OrdinalIgnoreCase) ||
                        port.Contains("net", StringComparison.OrdinalIgnoreCase) ||
                        port.Contains(":", StringComparison.OrdinalIgnoreCase))
                    {
                        connectionType = ConnectionType.Network;
                    }

                    var driverVersion = ParseDriverVersion(driverVersionStr);

                    var descriptor = new ScannerDescriptor
                    {
                        HardwareId = hardwareId,
                        Name = name,
                        Manufacturer = manufacturer,
                        ConnectionType = connectionType,
                        ConnectionString = $"wia://{hardwareId}",
                        Driver = new DriverInfo
                        {
                            Id = Guid.NewGuid(),
                            Name = "WIA Driver",
                            Type = DriverType.WIA,
                            Version = driverVersion,
                            Priority = DriverPriority.Normal,
                            Status = DriverStatus.Ready,
                            Manufacturer = manufacturer,
                            NativeProvider = "WIA 2.0",
                        },
                    };

                    list.Add(descriptor);
                }

                return list;
            }).ConfigureAwait(false);

            foreach (var desc in scanners)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (this.IsAllowed(desc, request))
                {
                    discovered.Add(desc);
                    this.ScannerDiscovered?.Invoke(this, desc);
                }
            }
        }
        catch (OperationCanceledException)
        {
            timedOut = !cancellationToken.IsCancellationRequested;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"WIA Discovery failed: {ex.Message}");
        }

        return new DiscoveryResult
        {
            Scanners = discovered.AsReadOnly(),
            Duration = DateTimeOffset.UtcNow - started,
            TimedOut = timedOut,
        };
    }

    private static T GetProperty<T>(dynamic properties, int propertyId, T defaultValue)
    {
        try
        {
            object? propertyObj = properties[propertyId];
            if (propertyObj != null)
            {
                dynamic property = propertyObj;
                var val = property.Value;
                if (val is T typedVal)
                {
                    return typedVal;
                }
                if (val != null)
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }
            }
        }
        catch
        {
            // Suppress and fallback
        }
        return defaultValue;
    }

    private static DriverVersion ParseDriverVersion(string versionStr)
    {
        var major = 1;
        var minor = 0;
        var build = 0;
        var revision = 0;

        if (Version.TryParse(versionStr, out var v))
        {
            major = v.Major;
            minor = v.Minor;
            build = v.Build >= 0 ? v.Build : 0;
            revision = v.Revision >= 0 ? v.Revision : 0;
        }

        return new DriverVersion
        {
            Major = major,
            Minor = minor,
            Build = build,
            Revision = revision,
            OriginalString = versionStr,
        };
    }

    private bool IsAllowed(ScannerDescriptor descriptor, DiscoveryRequest request)
    {
        if (request.AllowedConnectionTypes != null && request.AllowedConnectionTypes.Count > 0)
        {
            var found = false;
            foreach (var ct in request.AllowedConnectionTypes)
            {
                if (ct == descriptor.ConnectionType)
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

        if (request.AllowedDriverTypes != null && request.AllowedDriverTypes.Count > 0)
        {
            var found = false;
            foreach (var dt in request.AllowedDriverTypes)
            {
                if (dt == DriverType.WIA)
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
}
