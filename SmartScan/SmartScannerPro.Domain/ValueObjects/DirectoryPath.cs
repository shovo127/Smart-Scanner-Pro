namespace SmartScannerPro.Domain.ValueObjects;

using System;
using System.IO;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents an absolute directory path on the file system.
/// </summary>
public readonly record struct DirectoryPath
{
    /// <summary>
    /// Gets the absolute string path.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryPath"/> struct.
    /// </summary>
    /// <param name="path">The directory path.</param>
    public DirectoryPath(string path)
    {
        Guard.NotNullOrWhiteSpace(path, nameof(path));

        try
        {
            // Normalize path to absolute
            this.Value = Path.GetFullPath(path);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid path format.", nameof(path), ex);
        }
    }

    /// <summary>
    /// Returns the string representation of the path.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value;
}
