namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a storage location that can be local or remote (Cloud).
/// </summary>
public record StorageUri
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StorageUri"/> class.
    /// </summary>
    /// <param name="uri">The storage URI.</param>
    public StorageUri(string uri)
    {
        Guard.NotNullOrWhiteSpace(uri, nameof(uri));
        if (!Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _))
        {
            throw new ArgumentException("Invalid URI format.", nameof(uri));
        }

        this.Value = uri;
    }

    /// <summary>
    /// Gets the underlying URI string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Determines whether the URI represents a local file path.
    /// </summary>
    /// <returns>True if it is a local path; otherwise, false.</returns>
    public bool IsLocal() => this.Value.StartsWith("file://", StringComparison.OrdinalIgnoreCase) ||
                             this.Value.Contains(":\\", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public override string ToString() => this.Value;
}
