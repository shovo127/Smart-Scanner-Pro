namespace SmartScannerPro.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a plugin version.
/// </summary>
public sealed class PluginVersion : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginVersion"/> class.
    /// </summary>
    /// <param name="version">The version object.</param>
    public PluginVersion(VersionNumber version)
    {
        Guard.NotNull(version, nameof(version));
        this.Version = version;
    }

    /// <summary>
    /// Gets the version number.
    /// </summary>
    public VersionNumber Version { get; }

    /// <summary>
    /// Returns the string representation of the plugin version.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Version.ToString();

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Version;
    }
}
