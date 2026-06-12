namespace SmartScannerPro.Domain.Abstractions;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a strongly-typed identifier.
/// </summary>
/// <typeparam name="TValue">The type of the underlying identifier value.</typeparam>
public abstract class StronglyTypedId<TValue> : ValueObject
    where TValue : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StronglyTypedId{TValue}"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    protected StronglyTypedId(TValue value)
    {
        if (value is Guid guidValue && guidValue == Guid.Empty)
        {
            throw new ArgumentException("Identifier cannot be an empty Guid.", nameof(value));
        }

        this.Value = value;
    }

    /// <summary>
    /// Gets the underlying identifier value.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Returns the string representation of the identifier.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString() ?? string.Empty;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}
