namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the version of a plugin.
/// </summary>
public readonly record struct PluginVersion
{
    /// <summary>
    /// Gets the semantic version number.
    /// </summary>
    public VersionNumber Version { get; }

    /// <summary>
    /// Gets the optional release status (e.g., alpha, beta, rc1).
    /// </summary>
    public string PreReleaseTag { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginVersion"/> struct.
    /// </summary>
    /// <param name="version">The base version.</param>
    /// <param name="preReleaseTag">The optional pre-release tag.</param>
    public PluginVersion(VersionNumber version, string preReleaseTag = "")
    {
        this.Version = version;
        this.PreReleaseTag = preReleaseTag;
    }

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(this.PreReleaseTag)
            ? this.Version.ToString()
            : $"{this.Version}-{this.PreReleaseTag}";
    }
}
