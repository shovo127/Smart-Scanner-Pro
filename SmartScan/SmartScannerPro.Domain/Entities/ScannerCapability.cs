namespace SmartScannerPro.Domain.Entities;

using System.Collections.Generic;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a capability supported by a scanner device.
/// </summary>
public class ScannerCapability
{
    private readonly List<Resolution> supportedResolutions = new List<Resolution>();
    private readonly List<PaperSize> supportedPaperSizes = new List<PaperSize>();
    private readonly List<ColorMode> supportedColorModes = new List<ColorMode>();
    private readonly List<PaperSource> supportedSources = new List<PaperSource>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerCapability"/> class.
    /// </summary>
    /// <param name="supportsDuplex">True if duplex is supported; otherwise, false.</param>
    public ScannerCapability(bool supportsDuplex)
    {
        this.SupportsDuplex = supportsDuplex;
    }

    /// <summary>
    /// Gets a value indicating whether duplex scanning is supported.
    /// </summary>
    public bool SupportsDuplex { get; }

    /// <summary>
    /// Gets the supported resolutions.
    /// </summary>
    public IReadOnlyCollection<Resolution> SupportedResolutions => this.supportedResolutions.AsReadOnly();

    /// <summary>
    /// Gets the supported paper sizes.
    /// </summary>
    public IReadOnlyCollection<PaperSize> SupportedPaperSizes => this.supportedPaperSizes.AsReadOnly();

    /// <summary>
    /// Gets the supported color modes.
    /// </summary>
    public IReadOnlyCollection<ColorMode> SupportedColorModes => this.supportedColorModes.AsReadOnly();

    /// <summary>
    /// Gets the supported paper sources (e.g., Flatbed, ADF).
    /// </summary>
    public IReadOnlyCollection<PaperSource> SupportedSources => this.supportedSources.AsReadOnly();

    /// <summary>
    /// Adds a supported resolution.
    /// </summary>
    /// <param name="resolution">The resolution.</param>
    public void AddResolution(Resolution resolution)
    {
        this.supportedResolutions.Add(resolution);
    }

    /// <summary>
    /// Adds a supported paper size.
    /// </summary>
    /// <param name="paperSize">The paper size.</param>
    public void AddPaperSize(PaperSize paperSize)
    {
        this.supportedPaperSizes.Add(paperSize);
    }

    /// <summary>
    /// Adds a supported color mode.
    /// </summary>
    /// <param name="colorMode">The color mode.</param>
    public void AddColorMode(ColorMode colorMode)
    {
        this.supportedColorModes.Add(colorMode);
    }

    /// <summary>
    /// Adds a supported paper source.
    /// </summary>
    /// <param name="source">The paper source.</param>
    public void AddPaperSource(PaperSource source)
    {
        this.supportedSources.Add(source);
    }
}
