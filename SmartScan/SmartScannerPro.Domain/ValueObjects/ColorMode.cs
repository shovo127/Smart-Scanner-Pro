namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the color mode of a scan (e.g., Color, Grayscale, BlackAndWhite).
/// </summary>
public record ColorMode
{
    /// <summary>
    /// Gets the Color mode.
    /// </summary>
    public static readonly ColorMode Color = new ColorMode("Color");

    /// <summary>
    /// Gets the Grayscale mode.
    /// </summary>
    public static readonly ColorMode Grayscale = new ColorMode("Grayscale");

    /// <summary>
    /// Gets the Black &amp; White mode.
    /// </summary>
    public static readonly ColorMode BlackAndWhite = new ColorMode("BlackAndWhite");

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorMode"/> class.
    /// </summary>
    /// <param name="value">The color mode name.</param>
    public ColorMode(string value)
    {
        Guard.NotNullOrWhiteSpace(value, nameof(value));
        this.Value = value;
    }

    /// <summary>
    /// Gets the color mode value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the string representation of the color mode.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value;

}
