namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the size of a file in bytes.
/// </summary>
public sealed class FileSize : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileSize"/> class.
    /// </summary>
    /// <param name="bytes">The size in bytes (must be >= 0).</param>
    public FileSize(long bytes)
    {
        Guard.IsTrue(bytes >= 0, "File size cannot be negative.");
        this.Bytes = bytes;
    }

    /// <summary>
    /// Gets the file size in bytes.
    /// </summary>
    public long Bytes { get; }

    /// <summary>
    /// Gets the file size in Kilobytes (KB).
    /// </summary>
    public double Kilobytes => this.Bytes / 1024.0;

    /// <summary>
    /// Gets the file size in Megabytes (MB).
    /// </summary>
    public double Megabytes => this.Kilobytes / 1024.0;

    /// <summary>
    /// Returns a human-readable representation of the file size.
    /// </summary>
    /// <returns>A formatted string (e.g., 1.5 MB).</returns>
    public override string ToString()
    {
        if (this.Megabytes >= 1.0)
        {
            return $"{this.Megabytes:F2} MB";
        }
        else if (this.Kilobytes >= 1.0)
        {
            return $"{this.Kilobytes:F2} KB";
        }

        return $"{this.Bytes} Bytes";
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Bytes;
    }
}
