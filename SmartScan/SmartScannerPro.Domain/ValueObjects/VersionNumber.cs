namespace SmartScannerPro.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a semantic version number.
/// </summary>
public sealed class VersionNumber : ValueObject, IComparable<VersionNumber>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VersionNumber"/> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <param name="patch">The patch version.</param>
    public VersionNumber(int major, int minor, int patch)
    {
        Guard.IsTrue(major >= 0, "Major version cannot be negative.");
        Guard.IsTrue(minor >= 0, "Minor version cannot be negative.");
        Guard.IsTrue(patch >= 0, "Patch version cannot be negative.");

        this.Major = major;
        this.Minor = minor;
        this.Patch = patch;
    }

    /// <summary>
    /// Gets the major version.
    /// </summary>
    public int Major { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    public int Minor { get; }

    /// <summary>
    /// Gets the patch version.
    /// </summary>
    public int Patch { get; }

    /// <summary>
    /// Returns the string representation of the version.
    /// </summary>
    /// <returns>A semantic version string (e.g., 1.0.0).</returns>
    public override string ToString() => $"{this.Major}.{this.Minor}.{this.Patch}";

    /// <inheritdoc/>
    public int CompareTo(VersionNumber? other)
    {
        if (other is null)
        {
            return 1;
        }

        int majorCompare = this.Major.CompareTo(other.Major);
        if (majorCompare != 0)
        {
            return majorCompare;
        }

        int minorCompare = this.Minor.CompareTo(other.Minor);
        if (minorCompare != 0)
        {
            return minorCompare;
        }

        return this.Patch.CompareTo(other.Patch);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Major;
        yield return this.Minor;
        yield return this.Patch;
    }
}
