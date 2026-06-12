namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the unique identifier for an active scanning session.
/// </summary>
public readonly record struct SessionId
{
    /// <summary>
    /// Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public SessionId(Guid value)
    {
        Guard.IsTrue(value != Guid.Empty, "SessionId cannot be empty.");
        this.Value = value;
    }

    /// <summary>
    /// Creates a new, unique <see cref="SessionId"/>.
    /// </summary>
    /// <returns>A new <see cref="SessionId"/>.</returns>
    public static SessionId New() => new SessionId(Guid.NewGuid());

    /// <summary>
    /// Returns the string representation of the ID.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString();
}
