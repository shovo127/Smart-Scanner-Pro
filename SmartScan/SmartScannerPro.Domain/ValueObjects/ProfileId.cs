namespace SmartScannerPro.Domain.ValueObjects;

using System;

/// <summary>
/// Represents the strongly-typed identifier for a Scanner Profile.
/// </summary>
public readonly record struct ProfileId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileId"/> struct.
    /// </summary>
    /// <param name="value">The underlying Guid value.</param>
    public ProfileId(Guid value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Gets a new, empty ProfileId.
    /// </summary>
    public static ProfileId Empty => new ProfileId(Guid.Empty);

    /// <summary>
    /// Creates a new ProfileId.
    /// </summary>
    /// <returns>A new ProfileId.</returns>
    public static ProfileId NewId() => new ProfileId(Guid.NewGuid());
}
