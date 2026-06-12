namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the color depth and mode for scanning or image processing.
/// </summary>
public readonly record struct ColorMode
{
    /// <summary>
    /// Gets the logical scan mode name (e.g., Color, Grayscale).
    /// </summary>
    public string Mode { get; }

    /// <summary>
    /// Gets the bits per pixel (e.g., 1, 8, 24).
    /// </summary>
    public int BitsPerPixel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorMode"/> struct.
    /// </summary>
    /// <param name="mode">The scan mode name.</param>
    /// <param name="bitsPerPixel">The bit depth.</param>
    public ColorMode(string mode, int bitsPerPixel)
    {
        Guard.NotNullOrWhiteSpace(mode, nameof(mode));
        Guard.IsTrue(bitsPerPixel > 0, "Bits per pixel must be greater than zero.");
        this.Mode = mode;
        this.BitsPerPixel = bitsPerPixel;
    }

    /// <summary>
    /// Predefined 24-bit color mode.
    /// </summary>
    public static readonly ColorMode Color24Bit = new ColorMode("Color", 24);

    /// <summary>
    /// Predefined 8-bit grayscale mode.
    /// </summary>
    public static readonly ColorMode Grayscale8Bit = new ColorMode("Grayscale", 8);

    /// <summary>
    /// Predefined 1-bit black and white mode.
    /// </summary>
    public static readonly ColorMode BlackAndWhite1Bit = new ColorMode("BlackAndWhite", 1);

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => $"{this.Mode} ({this.BitsPerPixel}-bit)";
}
