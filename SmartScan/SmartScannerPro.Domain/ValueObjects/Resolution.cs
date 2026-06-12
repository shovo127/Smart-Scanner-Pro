namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the Dots Per Inch (DPI) resolution of a scan.
/// </summary>
public readonly record struct Resolution
{
    /// <summary>
    /// Gets the horizontal resolution in DPI.
    /// </summary>
    public int DpiX { get; }

    /// <summary>
    /// Gets the vertical resolution in DPI.
    /// </summary>
    public int DpiY { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Resolution"/> struct.
    /// </summary>
    /// <param name="dpiX">The horizontal DPI.</param>
    /// <param name="dpiY">The vertical DPI.</param>
    public Resolution(int dpiX, int dpiY)
    {
        Guard.IsTrue(dpiX > 0, "Horizontal DPI must be greater than zero.");
        Guard.IsTrue(dpiY > 0, "Vertical DPI must be greater than zero.");
        this.DpiX = dpiX;
        this.DpiY = dpiY;
    }

    /// <summary>
    /// Creates a symmetrical resolution where DpiX equals DpiY.
    /// </summary>
    /// <param name="dpi">The DPI for both axes.</param>
    /// <returns>A new <see cref="Resolution"/>.</returns>
    public static Resolution CreateSymmetrical(int dpi) => new Resolution(dpi, dpi);

    /// <summary>
    /// Returns the string representation of the resolution.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => $"{this.DpiX}x{this.DpiY} DPI";
}
