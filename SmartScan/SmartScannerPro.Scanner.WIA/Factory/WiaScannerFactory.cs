namespace SmartScannerPro.Scanner.WIA.Factory;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.WIA.Devices;
using SmartScannerPro.Scanner.WIA.Helpers;
using SmartScannerPro.Scanner.WIA.Sessions;

/// <summary>
/// Creates and binds scanner sessions to physical WIA scanner hardware.
/// Uses late-bound dynamic COM calls to eliminate compile-time dependencies on MSBuild COM resolution.
/// </summary>
public sealed class WiaScannerFactory : IScannerFactory
{
    /// <inheritdoc/>
    public async Task<IScannerSession> CreateSessionAsync(
        ScanSessionOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var descriptor = await StaThread.RunAsync(() =>
        {
            var deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
            if (deviceManagerType != null)
            {
                dynamic deviceManager = Activator.CreateInstance(deviceManagerType)!;
                dynamic deviceInfos = deviceManager.DeviceInfos;

                for (int i = 1; i <= deviceInfos.Count; i++)
                {
                    dynamic info = deviceInfos[i];
                    if (string.Equals(info.DeviceID, options.HardwareId, StringComparison.OrdinalIgnoreCase))
                    {
                        dynamic props = info.Properties;
                        var name = GetProperty<string>(props, 7, "WIA Scanner");
                        var manufacturer = GetProperty<string>(props, 3, "Generic");

                        return new ScannerDescriptor
                        {
                            HardwareId = info.DeviceID,
                            Name = name,
                            Manufacturer = manufacturer,
                            Driver = new DriverInfo
                            {
                                Id = Guid.NewGuid(),
                                Name = "WIA Driver",
                                Type = DriverType.WIA,
                                Version = new DriverVersion { Major = 1, Minor = 0, Build = 0, Revision = 0, OriginalString = "1.0.0.0" },
                                Status = DriverStatus.Ready,
                                Manufacturer = manufacturer,
                                NativeProvider = "WIA 2.0",
                            },
                        };
                    }
                }
            }

            // Fallback descriptor when offline/simulated
            return new ScannerDescriptor
            {
                HardwareId = options.HardwareId,
                Name = "Offline WIA Scanner",
                Manufacturer = "Unknown",
                Driver = new DriverInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "WIA Driver",
                    Type = DriverType.WIA,
                    Version = new DriverVersion { Major = 1, Minor = 0, Build = 0, Revision = 0, OriginalString = "1.0.0.0" },
                    Status = DriverStatus.Ready,
                    Manufacturer = "Unknown",
                    NativeProvider = "WIA 2.0",
                },
            };
        }).ConfigureAwait(false);

        var device = new WiaScannerDevice(descriptor);
        await device.RefreshAsync(cancellationToken).ConfigureAwait(false);

        var session = new WiaScannerSession(device);
        return session;
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
}
