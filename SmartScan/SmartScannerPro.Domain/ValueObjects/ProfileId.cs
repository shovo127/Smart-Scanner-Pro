namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Domain.Abstractions;

/// <summary>
/// Represents the unique identifier for a scan profile.
/// </summary>
public sealed class ProfileId : StronglyTypedId<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileId"/> class.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public ProfileId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a new, unique <see cref="ProfileId"/>.
    /// </summary>
    /// <returns>A new <see cref="ProfileId"/>.</returns>
    public static ProfileId New() => new ProfileId(Guid.NewGuid());
}
