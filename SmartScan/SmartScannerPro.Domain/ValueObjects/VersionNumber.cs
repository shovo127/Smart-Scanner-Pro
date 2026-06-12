namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a semantic version number.
/// </summary>
public readonly record struct VersionNumber : IComparable<VersionNumber>
{
    /// <summary>
    /// Gets the major version.
    /// </summary>
    public int Major { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    public int Minor { get; }

    /// <summary>
    /// Gets the build version.
    /// </summary>
    public int Build { get; }

    /// <summary>
    /// Gets the revision version.
    /// </summary>
    public int Revision { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionNumber"/> struct.
    /// </summary>
    /// <param name="version">The version object.</param>
    public VersionNumber(Version version)
    {
        Guard.NotNull(version, nameof(version));
        this.Major = version.Major;
        this.Minor = version.Minor;
        this.Build = version.Build;
        this.Revision = version.Revision;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionNumber"/> struct.
    /// </summary>
    /// <param name="versionString">The version string.</param>
    public VersionNumber(string versionString)
    {
        Guard.NotNullOrWhiteSpace(versionString, nameof(versionString));
        var version = Version.Parse(versionString);
        this.Major = version.Major;
        this.Minor = version.Minor;
        this.Build = version.Build;
        this.Revision = version.Revision;
    }

    /// <summary>
    /// Returns the string representation of the version.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        var version = new Version(this.Major, this.Minor, Math.Max(0, this.Build), Math.Max(0, this.Revision));
        return version.ToString();
    }

    /// <inheritdoc/>
    public int CompareTo(VersionNumber other)
    {
        var thisVersion = new Version(this.Major, this.Minor, Math.Max(0, this.Build), Math.Max(0, this.Revision));
        var otherVersion = new Version(other.Major, other.Minor, Math.Max(0, other.Build), Math.Max(0, other.Revision));
        return thisVersion.CompareTo(otherVersion);
    }
}
