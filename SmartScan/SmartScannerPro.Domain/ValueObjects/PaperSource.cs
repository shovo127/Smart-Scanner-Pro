namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the source feeding paper into the scanner.
/// </summary>
public sealed class PaperSource : ValueObject
{
    /// <summary>
    /// Gets the Flatbed source.
    /// </summary>
    public static readonly PaperSource Flatbed = new PaperSource("Flatbed");

    /// <summary>
    /// Gets the Automatic Document Feeder (ADF) source.
    /// </summary>
    public static readonly PaperSource Adf = new PaperSource("ADF");

    /// <summary>
    /// Initializes a new instance of the <see cref="PaperSource"/> class.
    /// </summary>
    /// <param name="value">The paper source name.</param>
    public PaperSource(string value)
    {
        Guard.NotNullOrWhiteSpace(value, nameof(value));
        this.Value = value;
    }

    /// <summary>
    /// Gets the paper source value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the string representation of the paper source.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value.ToLowerInvariant();
    }
}
