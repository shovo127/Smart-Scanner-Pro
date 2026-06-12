namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the image output format.
/// </summary>
public sealed class ImageFormat : ValueObject
{
    /// <summary>
    /// Gets the JPEG format.
    /// </summary>
    public static readonly ImageFormat Jpeg = new ImageFormat("JPEG", ".jpg");

    /// <summary>
    /// Gets the PNG format.
    /// </summary>
    public static readonly ImageFormat Png = new ImageFormat("PNG", ".png");

    /// <summary>
    /// Gets the TIFF format.
    /// </summary>
    public static readonly ImageFormat Tiff = new ImageFormat("TIFF", ".tiff");

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageFormat"/> class.
    /// </summary>
    /// <param name="name">The format name.</param>
    /// <param name="extension">The format extension.</param>
    public ImageFormat(string name, string extension)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        Guard.NotNullOrWhiteSpace(extension, nameof(extension));

        this.Name = name;
        this.Extension = extension.StartsWith(".") ? extension : $".{extension}";
    }

    /// <summary>
    /// Gets the common name of the format (e.g., JPEG).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the file extension for the format (e.g., .jpg).
    /// </summary>
    public string Extension { get; }

    /// <summary>
    /// Returns the string representation of the format.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => $"{this.Name} ({this.Extension})";

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Name.ToLowerInvariant();
        yield return this.Extension.ToLowerInvariant();
    }
}
