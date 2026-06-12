namespace SmartScannerPro.Domain.ValueObjects;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a capability supported by a scanner device.
/// </summary>
public record ScannerCapability
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerCapability"/> class.
    /// </summary>
    /// <param name="supportsDuplex">True if duplex is supported; otherwise, false.</param>
    /// <param name="supportedResolutions">The supported resolutions.</param>
    /// <param name="supportedPaperSizes">The supported paper sizes.</param>
    /// <param name="supportedColorModes">The supported color modes.</param>
    /// <param name="supportedSources">The supported sources.</param>
    public ScannerCapability(
        bool supportsDuplex,
        IReadOnlyCollection<Resolution> supportedResolutions,
        IReadOnlyCollection<PaperSize> supportedPaperSizes,
        IReadOnlyCollection<ColorMode> supportedColorModes,
        IReadOnlyCollection<PaperSource> supportedSources)
    {
        this.SupportsDuplex = supportsDuplex;
        this.SupportedResolutions = supportedResolutions ?? Array.Empty<Resolution>();
        this.SupportedPaperSizes = supportedPaperSizes ?? Array.Empty<PaperSize>();
        this.SupportedColorModes = supportedColorModes ?? Array.Empty<ColorMode>();
        this.SupportedSources = supportedSources ?? Array.Empty<PaperSource>();
    }

    /// <summary>
    /// Gets a value indicating whether duplex scanning is supported.
    /// </summary>
    public bool SupportsDuplex { get; }

    /// <summary>
    /// Gets the supported resolutions.
    /// </summary>
    public IReadOnlyCollection<Resolution> SupportedResolutions { get; }

    /// <summary>
    /// Gets the supported paper sizes.
    /// </summary>
    public IReadOnlyCollection<PaperSize> SupportedPaperSizes { get; }

    /// <summary>
    /// Gets the supported color modes.
    /// </summary>
    public IReadOnlyCollection<ColorMode> SupportedColorModes { get; }

    /// <summary>
    /// Gets the supported paper sources (e.g., Flatbed, ADF).
    /// </summary>
    public IReadOnlyCollection<PaperSource> SupportedSources { get; }
}
