namespace SmartScannerPro.Scanner.Mock.Capabilities;

using System;
using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Models.Capabilities;
using SmartScannerPro.Scanner.Abstractions.Models.Images;
using SmartScannerPro.Scanner.Mock.Configuration;

/// <summary>
/// Builds a <see cref="CapabilitySet"/> populated with the full set of capabilities
/// advertised by a simulated scanner device.
/// </summary>
public static class MockCapabilityFactory
{
    // ────────────────────────────────────────────────────────────────────────
    // Capability identifier constants – shared across the SDK and test code
    // ────────────────────────────────────────────────────────────────────────

    /// <summary>Document-source capability identifier.</summary>
    public const string CapIdSource = "document-source";

    /// <summary>Duplex scanning capability identifier.</summary>
    public const string CapIdDuplex = "duplex";

    /// <summary>Color mode capability identifier.</summary>
    public const string CapIdColorMode = "color-mode";

    /// <summary>Resolution (DPI) capability identifier.</summary>
    public const string CapIdResolution = "resolution";

    /// <summary>Brightness adjustment capability identifier.</summary>
    public const string CapIdBrightness = "brightness";

    /// <summary>Contrast adjustment capability identifier.</summary>
    public const string CapIdContrast = "contrast";

    /// <summary>Output image format capability identifier.</summary>
    public const string CapIdFormat = "output-format";

    /// <summary>Paper size capability identifier.</summary>
    public const string CapIdPaperSize = "paper-size";

    /// <summary>
    /// Builds the complete <see cref="CapabilitySet"/> for the given device profile.
    /// </summary>
    /// <param name="profile">The mock device profile to build capabilities for.</param>
    /// <returns>A fully populated <see cref="CapabilitySet"/>.</returns>
    public static CapabilitySet Build(MockDeviceProfile profile)
    {
        var caps = new List<Capability>
        {
            BuildSourceCapability(profile),
            BuildDuplexCapability(profile),
            BuildColorModeCapability(profile),
            BuildResolutionCapability(profile),
            BuildBrightnessCapability(),
            BuildContrastCapability(),
            BuildFormatCapability(),
            BuildPaperSizeCapability(profile),
        };

        return new CapabilitySet(caps);
    }

    private static Capability<string> BuildSourceCapability(MockDeviceProfile profile)
    {
        var sources = new List<string>();
        if (profile.SupportsFlatbed)
        {
            sources.Add("Flatbed");
        }

        if (profile.SupportsAdf)
        {
            sources.Add("AdfFront");
            if (profile.SupportsDuplex)
            {
                sources.Add("AdfBack");
            }
        }

        return new Capability<string>
        {
            Id = CapIdSource,
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
        };
    }

    private static Capability<bool> BuildDuplexCapability(MockDeviceProfile profile)
    {
        return new Capability<bool>
        {
            Id = CapIdDuplex,
            Name = "Duplex Scanning",
            Category = CapabilityCategory.DocumentHandling,
            IsReadOnly = !profile.SupportsDuplex,
            ValueType = typeof(bool),
            Value = new CapabilityValue<bool>
            {
                DefaultValue = false,
                CurrentValue = false,
                SupportedValues = profile.SupportsDuplex
                    ? new List<bool> { false, true }.AsReadOnly()
                    : new List<bool> { false }.AsReadOnly(),
            },
        };
    }

    private static Capability<string> BuildColorModeCapability(MockDeviceProfile profile)
    {
        var modes = new List<string>();
        foreach (var mode in profile.SupportedColorModes)
        {
            modes.Add(mode.ToString());
        }

        return new Capability<string>
        {
            Id = CapIdColorMode,
            Name = "Color Mode",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(string),
            Value = new CapabilityValue<string>
            {
                DefaultValue = ColorProfile.Color.ToString(),
                CurrentValue = ColorProfile.Color.ToString(),
                SupportedValues = modes.AsReadOnly(),
            },
        };
    }

    private static Capability<int> BuildResolutionCapability(MockDeviceProfile profile)
    {
        return new Capability<int>
        {
            Id = CapIdResolution,
            Name = "Resolution (DPI)",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(int),
            Value = new CapabilityValue<int>
            {
                DefaultValue = 300,
                CurrentValue = 300,
                SupportedValues = new List<int>(profile.SupportedDpis).AsReadOnly(),
                MinValue = profile.SupportedDpis[0],
                MaxValue = profile.SupportedDpis[profile.SupportedDpis.Count - 1],
            },
        };
    }

    private static Capability<int> BuildBrightnessCapability()
    {
        return new Capability<int>
        {
            Id = CapIdBrightness,
            Name = "Brightness",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(int),
            Value = new CapabilityValue<int>
            {
                DefaultValue = 0,
                CurrentValue = 0,
                MinValue = -100,
                MaxValue = 100,
                Step = 1,
            },
        };
    }

    private static Capability<int> BuildContrastCapability()
    {
        return new Capability<int>
        {
            Id = CapIdContrast,
            Name = "Contrast",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(int),
            Value = new CapabilityValue<int>
            {
                DefaultValue = 0,
                CurrentValue = 0,
                MinValue = -100,
                MaxValue = 100,
                Step = 1,
            },
        };
    }

    private static Capability<string> BuildFormatCapability()
    {
        return new Capability<string>
        {
            Id = CapIdFormat,
            Name = "Output Format",
            Category = CapabilityCategory.Image,
            IsReadOnly = false,
            ValueType = typeof(string),
            Value = new CapabilityValue<string>
            {
                DefaultValue = "PNG",
                CurrentValue = "PNG",
                SupportedValues = new List<string> { "JPEG", "PNG", "TIFF" }.AsReadOnly(),
            },
        };
    }

    private static Capability<string> BuildPaperSizeCapability(MockDeviceProfile profile)
    {
        var sizes = new List<string> { "A4", "A5", "Letter", "Legal" };

        return new Capability<string>
        {
            Id = CapIdPaperSize,
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
        };
    }
}
