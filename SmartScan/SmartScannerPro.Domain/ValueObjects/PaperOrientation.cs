namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the physical orientation of a document page.
/// </summary>
public sealed class PaperOrientation : ValueObject
{
    /// <summary>
    /// Gets the Portrait orientation.
    /// </summary>
    public static readonly PaperOrientation Portrait = new PaperOrientation("Portrait");

    /// <summary>
    /// Gets the Landscape orientation.
    /// </summary>
    public static readonly PaperOrientation Landscape = new PaperOrientation("Landscape");

    /// <summary>
    /// Initializes a new instance of the <see cref="PaperOrientation"/> class.
    /// </summary>
    /// <param name="value">The orientation name.</param>
    public PaperOrientation(string value)
    {
        Guard.NotNullOrWhiteSpace(value, nameof(value));
        this.Value = value;
    }

    /// <summary>
    /// Gets the orientation value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the string representation of the orientation.
    /// </summary>
    /// <returns>The orientation string.</returns>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value.ToLowerInvariant();
    }
}
