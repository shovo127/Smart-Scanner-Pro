namespace SmartScannerPro.Scanner.WIA.Devices;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Capabilities;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.WIA.Helpers;

/// <summary>
/// Represents a physical scanner device accessed via Windows Image Acquisition (WIA).
/// Uses late-bound dynamic COM calls to eliminate compile-time dependencies on MSBuild COM resolution.
/// </summary>
public sealed class WiaScannerDevice : IScannerDevice, IScannerCapabilities, IScannerConfiguration
{
    private readonly Dictionary<string, object> currentValues = new(StringComparer.OrdinalIgnoreCase);
    private CapabilitySet capabilitySet;

    /// <summary>
    /// Initializes a new instance of the <see cref="WiaScannerDevice"/> class.
    /// </summary>
    /// <param name="descriptor">The scanner hardware descriptor.</param>
    public WiaScannerDevice(ScannerDescriptor descriptor)
    {
        this.Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
        this.capabilitySet = this.BuildDefaultCapabilities();
    }

    /// <inheritdoc/>
    public ScannerDescriptor Descriptor { get; }

    /// <inheritdoc/>
    public IScannerCapabilities Capabilities => this;

    /// <inheritdoc/>
    public IScannerConfiguration Configuration => this;

    /// <inheritdoc/>
    public IScannerConnection? Connection => null;

    /// <inheritdoc/>
    public CapabilitySet SupportedCapabilities => this.capabilitySet;

    /// <inheritdoc/>
    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var caps = await StaThread.RunAsync(() =>
            {
                var deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
                if (deviceManagerType == null)
                {
                    return this.BuildDefaultCapabilities();
                }

                dynamic deviceManager = Activator.CreateInstance(deviceManagerType)!;
                dynamic deviceInfos = deviceManager.DeviceInfos;
                dynamic? targetInfo = null;

                for (int i = 1; i <= deviceInfos.Count; i++)
                {
                    dynamic info = deviceInfos[i];
                    if (string.Equals(info.DeviceID, this.Descriptor.HardwareId, StringComparison.OrdinalIgnoreCase))
                    {
                        targetInfo = info;
                        break;
                    }
                }

                if (targetInfo == null)
                {
                    return this.BuildDefaultCapabilities();
                }

                dynamic? wiaDevice = null;
                try
                {
                    wiaDevice = targetInfo.Connect();
                    var docHandlingCaps = GetProperty<int>(wiaDevice.Properties, 3086, 0); // WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES
                    bool supportsAdf = (docHandlingCaps & 0x1) != 0;
                    bool supportsDuplex = (docHandlingCaps & 0x4) != 0;
                    bool supportsFlatbed = (docHandlingCaps & 0x2) != 0 || !supportsAdf;

                    return this.BuildCapabilities(supportsFlatbed, supportsAdf, supportsDuplex);
                }
                catch
                {
                    return this.BuildDefaultCapabilities();
                }
                finally
                {
                    if (wiaDevice != null && Marshal.IsComObject(wiaDevice))
                    {
                        Marshal.ReleaseComObject(wiaDevice);
                    }
                }
            }).ConfigureAwait(false);

            this.capabilitySet = caps;
        }
        catch
        {
            this.capabilitySet = this.BuildDefaultCapabilities();
        }
    }

    /// <inheritdoc/>
    public Task<bool> SetCapabilityValueAsync<T>(string capabilityId, T value, CancellationToken cancellationToken = default)
    {
        var cap = this.capabilitySet.GetCapability<T>(capabilityId);
        if (cap == null)
        {
            return Task.FromResult(false);
        }

        if (value != null && !cap.Value.IsSupported(value))
        {
            return Task.FromResult(false);
        }

        this.currentValues[capabilityId] = value!;
        return Task.FromResult(true);
    }

    /// <inheritdoc/>
    public Task ApplyConfigurationAsync(
        IReadOnlyDictionary<string, object> settings,
        CancellationToken cancellationToken = default)
    {
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        foreach (var kvp in settings)
        {
            this.currentValues[kvp.Key] = kvp.Value;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task ResetToDefaultsAsync(CancellationToken cancellationToken = default)
    {
        this.currentValues.Clear();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the current value for a given capability, falling back to its default value if not customized.
    /// </summary>
    /// <typeparam name="T">The type of the capability value.</typeparam>
    /// <param name="capabilityId">The capability identifier.</param>
    /// <returns>The current value.</returns>
    public T? GetCurrentValue<T>(string capabilityId)
    {
        if (this.currentValues.TryGetValue(capabilityId, out var obj) && obj is T typed)
        {
            return typed;
        }

        var cap = this.capabilitySet.GetCapability<T>(capabilityId);
        return cap != null ? cap.Value.CurrentValue : default;
    }

    private static int GetProperty<T>(dynamic properties, int propertyId, int defaultValue)
    {
        try
        {
            dynamic property = properties[propertyId];
            if (property != null)
            {
                var val = property.Value;
                if (val != null)
                {
                    return Convert.ToInt32(val);
                }
            }
        }
        catch
        {
            // Suppress and fallback
        }
        return defaultValue;
    }

    private CapabilitySet BuildDefaultCapabilities()
    {
        return this.BuildCapabilities(supportsFlatbed: true, supportsAdf: true, supportsDuplex: true);
    }

    private CapabilitySet BuildCapabilities(bool supportsFlatbed, bool supportsAdf, bool supportsDuplex)
    {
        var caps = new List<Capability>();

        // 1. Document Source
        var sources = new List<string>();
        if (supportsFlatbed)
        {
            sources.Add("Flatbed");
        }
        if (supportsAdf)
        {
            sources.Add("AdfFront");
            if (supportsDuplex)
            {
                sources.Add("AdfBack");
            }
        }

        caps.Add(new Capability<string>
        {
            Id = "document-source",
            Name = "Document Source",
            Category = CapabilityCategory.DocumentHandling,
            IsReadOnly = false,
            ValueType = typeof(string),
            Value = new CapabilityValue<string>
            {
                DefaultValue = sources.Count > 0 ? sources[0] : "Flatbed",
                CurrentValue = sources.Count > 0 ? sources[0] : "Flatbed",
                SupportedValues = sources.AsReadOnly(),
            },
        });

        // 2. Duplex Support
        caps.Add(new Capability<bool>
        {
            Id = "duplex",
            Name = "Duplex Scanning",
            Category = CapabilityCategory.DocumentHandling,
            IsReadOnly = !supportsDuplex,
            ValueType = typeof(bool),
            Value = new CapabilityValue<bool>
            {
                DefaultValue = false,
                CurrentValue = false,
                SupportedValues = supportsDuplex
                    ? new List<bool> { false, true }.AsReadOnly()
                    : new List<bool> { false }.AsReadOnly(),
            },
        });

        // 3. Color Mode
        var modes = new List<string> { "Color", "Grayscale", "BlackAndWhite" };
        caps.Add(new Capability<string>
        {
            Id = "color-mode",
            Name = "Color Mode",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(string),
            Value = new CapabilityValue<string>
            {
                DefaultValue = "Color",
                CurrentValue = "Color",
                SupportedValues = modes.AsReadOnly(),
            },
        });

        // 4. Resolution
        var dpis = new List<int> { 75, 100, 150, 200, 300, 600, 1200 };
        caps.Add(new Capability<int>
        {
            Id = "resolution",
            Name = "Resolution (DPI)",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(int),
            Value = new CapabilityValue<int>
            {
                DefaultValue = 300,
                CurrentValue = 300,
                SupportedValues = dpis.AsReadOnly(),
                MinValue = 75,
                MaxValue = 1200,
            },
        });

        // 5. Paper Size
        var sizes = new List<string> { "A4", "Letter", "Legal" };
        caps.Add(new Capability<string>
        {
            Id = "paper-size",
            Name = "Paper Size",
            Category = CapabilityCategory.Hardware,
            IsReadOnly = false,
            ValueType = typeof(string),
            Value = new CapabilityValue<string>
            {
                DefaultValue = "A4",
                CurrentValue = "A4",
                SupportedValues = sizes.AsReadOnly(),
            },
        });

        return new CapabilitySet(caps);
    }
}
