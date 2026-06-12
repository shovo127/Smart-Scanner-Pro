namespace SmartScannerPro.Scanner.Abstractions.Models.Drivers;

using System;

/// <summary>
/// Represents the version information of a scanner driver.
/// </summary>
public record DriverVersion
{
    /// <summary>
    /// Gets the major version number.
    /// </summary>
    public int Major { get; init; }

    /// <summary>
    /// Gets the minor version number.
    /// </summary>
    public int Minor { get; init; }

    /// <summary>
    /// Gets the build number.
    /// </summary>
    public int Build { get; init; }

    /// <summary>
    /// Gets the revision number.
    /// </summary>
    public int Revision { get; init; }

    /// <summary>
    /// Gets the original version string provided by the driver.
    /// </summary>
    public string OriginalString { get; init; } = string.Empty;

    /// <summary>
    /// Returns the string representation of the version.
    /// </summary>
    /// <returns>A string in the format Major.Minor.Build.Revision.</returns>
    public override string ToString() => $"{this.Major}.{this.Minor}.{this.Build}.{this.Revision}";
}
