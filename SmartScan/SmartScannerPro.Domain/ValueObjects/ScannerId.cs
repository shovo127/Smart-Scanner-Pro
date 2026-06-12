namespace SmartScannerPro.Domain.ValueObjects;

using System;

/// <summary>
/// Represents the strongly-typed identifier for a Scanner.
/// </summary>
public readonly record struct ScannerId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerId"/> struct.
    /// </summary>
    /// <param name="value">The underlying Guid value.</param>
    public ScannerId(Guid value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Gets a new, empty ScannerId.
    /// </summary>
    public static ScannerId Empty => new ScannerId(Guid.Empty);

    /// <summary>
    /// Creates a new ScannerId.
    /// </summary>
    /// <returns>A new ScannerId.</returns>
    public static ScannerId NewId() => new ScannerId(Guid.NewGuid());
}
