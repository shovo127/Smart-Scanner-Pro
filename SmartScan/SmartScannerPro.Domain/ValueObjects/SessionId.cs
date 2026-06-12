namespace SmartScannerPro.Domain.ValueObjects;

using System;

/// <summary>
/// Represents the strongly-typed identifier for a Session.
/// </summary>
public readonly record struct SessionId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionId"/> struct.
    /// </summary>
    /// <param name="value">The underlying Guid value.</param>
    public SessionId(Guid value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Gets a new, empty SessionId.
    /// </summary>
    public static SessionId Empty => new SessionId(Guid.Empty);

    /// <summary>
    /// Creates a new SessionId.
    /// </summary>
    /// <returns>A new SessionId.</returns>
    public static SessionId NewId() => new SessionId(Guid.NewGuid());
}
