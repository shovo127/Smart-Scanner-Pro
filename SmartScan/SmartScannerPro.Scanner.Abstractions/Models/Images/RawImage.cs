namespace SmartScannerPro.Scanner.Abstractions.Models.Images;

using System;
using System.IO;

/// <summary>
/// Represents the raw image data acquired from the scanner.
/// </summary>
public record RawImage : IDisposable
{
    /// <summary>
    /// Gets the metadata associated with the image.
    /// </summary>
    public required ImageMetadata Metadata { get; init; }

    /// <summary>
    /// Gets the stream containing the raw image data.
    /// </summary>
    public required Stream DataStream { get; init; }

    /// <summary>
    /// Disposes the underlying data stream.
    /// </summary>
    public void Dispose()
    {
        this.DataStream?.Dispose();
        GC.SuppressFinalize(this);
    }
}
