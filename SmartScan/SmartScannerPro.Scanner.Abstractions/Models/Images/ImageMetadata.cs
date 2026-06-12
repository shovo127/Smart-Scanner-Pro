namespace SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Contains metadata describing a raw image.
/// </summary>
public record ImageMetadata
{
    /// <summary>
    /// Gets the width of the image in pixels.
    /// </summary>
    public int WidthInPixels { get; init; }

    /// <summary>
    /// Gets the height of the image in pixels.
    /// </summary>
    public int HeightInPixels { get; init; }

    /// <summary>
    /// Gets the horizontal resolution (DPI).
    /// </summary>
    public int ResolutionX { get; init; }

    /// <summary>
    /// Gets the vertical resolution (DPI).
    /// </summary>
    public int ResolutionY { get; init; }

    /// <summary>
    /// Gets the bit depth (bits per pixel).
    /// </summary>
    public int BitDepth { get; init; }

    /// <summary>
    /// Gets the color profile of the image.
    /// </summary>
    public ColorProfile ColorProfile { get; init; }

    /// <summary>
    /// Gets the image source from which this image was acquired.
    /// </summary>
    public ImageSource Source { get; init; }

    /// <summary>
    /// Gets the compression type, if any.
    /// </summary>
    public Compression Compression { get; init; }

    /// <summary>
    /// Gets the native driver format used during acquisition.
    /// </summary>
    public ImageFormat Format { get; init; }
}
