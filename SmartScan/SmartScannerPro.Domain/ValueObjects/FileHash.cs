namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a cryptographic hash for file integrity.
/// </summary>
public record FileHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileHash"/> class.
    /// </summary>
    /// <param name="value">The hash value.</param>
    public FileHash(string value)
    {
        Guard.NotNullOrWhiteSpace(value, nameof(value));
        this.Value = value;
    }

    /// <summary>
    /// Gets the hash value string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the string representation of the hash.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value;

}
