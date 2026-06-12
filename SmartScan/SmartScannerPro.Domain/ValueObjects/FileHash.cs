namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a cryptographic hash for file integrity.
/// </summary>
public sealed class FileHash : ValueObject
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

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value.ToLowerInvariant();
    }
}
