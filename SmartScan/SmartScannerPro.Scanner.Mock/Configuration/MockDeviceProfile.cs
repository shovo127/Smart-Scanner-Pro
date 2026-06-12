namespace SmartScannerPro.Scanner.Mock.Configuration;

using System;
using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Models.Capabilities;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Images;
using SmartScannerPro.TestAssets;
using System.Text.Json.Serialization;

/// <summary>
/// Describes a mock scanner device loaded from profile data.
/// </summary>
public sealed record MockDeviceProfile
{
    /// <summary>Gets the hardware identifier.</summary>
    public required string HardwareId { get; init; }

    /// <summary>Gets the display name.</summary>
    public required string Name { get; init; }

    /// <summary>Gets the manufacturer name.</summary>
    public required string Manufacturer { get; init; }

    /// <summary>Gets the connection type.</summary>
    public ConnectionType ConnectionType { get; init; }

    /// <summary>Gets the connection string.</summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>Gets the firmware version.</summary>
    public string FirmwareVersion { get; init; } = "1.0.0.0";

    /// <summary>Gets the current performance model identifier.</summary>
    public string PerformanceModel { get; init; } = "Fast";

    /// <summary>
    /// Gets the legacy performance profile value used by existing mock services.
    /// </summary>
    public MockPerformanceProfileType PerformanceProfile
        => Enum.TryParse<MockPerformanceProfileType>(this.PerformanceModel, ignoreCase: true, out var profile)
            ? profile
            : MockPerformanceProfileType.Fast;

    /// <summary>Gets the supported capabilities.</summary>
    [JsonIgnore]
    public CapabilitySet Capabilities { get; init; } = new(Array.Empty<SmartScannerPro.Scanner.Abstractions.Models.Capabilities.Capability>());

    /// <summary>Gets the supported document generators.</summary>
    public IReadOnlyList<DocumentKind> DocumentKinds { get; init; } = Array.Empty<DocumentKind>();

    /// <summary>Gets the failure injector names applied to this profile.</summary>
    public IReadOnlyList<string> FailureInjectors { get; init; } = Array.Empty<string>();

    /// <summary>Gets the supported paper sizes.</summary>
    public IReadOnlyList<string> SupportedPaperSizes { get; init; } = Array.Empty<string>();

    /// <summary>Gets the supported color modes.</summary>
    public IReadOnlyList<ColorProfile> SupportedColorModes { get; init; } = Array.Empty<ColorProfile>();

    /// <summary>Gets the supported DPI values.</summary>
    public IReadOnlyList<int> SupportedDpis { get; init; } = Array.Empty<int>();

    /// <summary>Gets a value indicating whether duplex is supported.</summary>
    public bool SupportsDuplex { get; init; }

    /// <summary>Gets a value indicating whether the profile supports a flatbed source.</summary>
    public bool SupportsFlatbed { get; init; }

    /// <summary>Gets a value indicating whether the profile supports an ADF source.</summary>
    public bool SupportsAdf { get; init; }

    /// <summary>Gets optional failure override settings for the profile.</summary>
    public FailureSimulationOptions? FailureOverride { get; init; }
}
