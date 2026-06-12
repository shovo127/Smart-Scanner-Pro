namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents an image file format.
/// </summary>
public readonly record struct ImageFormat
{
    /// <summary>
    /// Gets the file extension (e.g. .jpg).
    /// </summary>
    public string Extension { get; }

    /// <summary>
    /// Gets the MIME type (e.g. image/jpeg).
    /// </summary>
    public string MimeType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageFormat"/> struct.
    /// </summary>
    /// <param name="extension">The file extension.</param>
    /// <param name="mimeType">The MIME type.</param>
    public ImageFormat(string extension, string mimeType)
    {
        Guard.NotNullOrWhiteSpace(extension, nameof(extension));
        Guard.NotNullOrWhiteSpace(mimeType, nameof(mimeType));
        this.Extension = extension.ToLowerInvariant();
        this.MimeType = mimeType.ToLowerInvariant();
    }

    /// <summary>
    /// Predefined JPEG format.
    /// </summary>
    public static readonly ImageFormat Jpeg = new ImageFormat(".jpg", "image/jpeg");

    /// <summary>
    /// Predefined PNG format.
    /// </summary>
    public static readonly ImageFormat Png = new ImageFormat(".png", "image/png");

    /// <summary>
    /// Predefined TIFF format.
    /// </summary>
    public static readonly ImageFormat Tiff = new ImageFormat(".tiff", "image/tiff");

    /// <summary>
    /// Predefined BMP format.
    /// </summary>
    public static readonly ImageFormat Bmp = new ImageFormat(".bmp", "image/bmp");

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Extension;
}
