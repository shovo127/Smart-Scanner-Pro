namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Domain.Abstractions;

/// <summary>
/// Represents the unique identifier for a Scanner Device.
/// </summary>
public sealed class ScannerId : StronglyTypedId<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerId"/> class.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public ScannerId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a new, unique <see cref="ScannerId"/>.
    /// </summary>
    /// <returns>A new <see cref="ScannerId"/>.</returns>
    public static ScannerId New() => new ScannerId(Guid.NewGuid());
}
