namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a page number in a document.
/// </summary>
public sealed class PageNumber : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageNumber"/> class.
    /// </summary>
    /// <param name="value">The page number (must be >= 1).</param>
    public PageNumber(int value)
    {
        Guard.IsTrue(value >= 1, "Page number must be 1 or greater.");
        this.Value = value;
    }

    /// <summary>
    /// Gets the 1-based page number.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Returns the string representation of the page number.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString();

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}
