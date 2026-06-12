namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the unique identifier for a Scanner Device.
/// </summary>
public readonly record struct ScannerId
{
    /// <summary>
    /// Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public ScannerId(Guid value)
    {
        Guard.IsTrue(value != Guid.Empty, "ScannerId cannot be empty.");
        this.Value = value;
    }

    /// <summary>
    /// Creates a new, unique <see cref="ScannerId"/>.
    /// </summary>
    /// <returns>A new <see cref="ScannerId"/>.</returns>
    public static ScannerId New() => new ScannerId(Guid.NewGuid());

    /// <summary>
    /// Returns the string representation of the ID.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString();
}
