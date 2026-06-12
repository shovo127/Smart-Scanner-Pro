namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Domain.Abstractions;

/// <summary>
/// Represents the unique identifier for an active scanning session.
/// </summary>
public sealed class SessionId : StronglyTypedId<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionId"/> class.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public SessionId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a new, unique <see cref="SessionId"/>.
    /// </summary>
    /// <returns>A new <see cref="SessionId"/>.</returns>
    public static SessionId New() => new SessionId(Guid.NewGuid());
}
