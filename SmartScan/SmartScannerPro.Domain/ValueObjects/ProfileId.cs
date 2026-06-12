namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the unique identifier for a scan profile.
/// </summary>
public readonly record struct ProfileId
{
    /// <summary>
    /// Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public ProfileId(Guid value)
    {
        Guard.IsTrue(value != Guid.Empty, "ProfileId cannot be empty.");
        this.Value = value;
    }

    /// <summary>
    /// Creates a new, unique <see cref="ProfileId"/>.
    /// </summary>
    /// <returns>A new <see cref="ProfileId"/>.</returns>
    public static ProfileId New() => new ProfileId(Guid.NewGuid());

    /// <summary>
    /// Returns the string representation of the ID.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString();
}
