namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the unique identifier for a scanned document.
/// </summary>
public readonly record struct DocumentId
{
    /// <summary>
    /// Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public DocumentId(Guid value)
    {
        Guard.IsTrue(value != Guid.Empty, "DocumentId cannot be empty.");
        this.Value = value;
    }

    /// <summary>
    /// Creates a new, unique <see cref="DocumentId"/>.
    /// </summary>
    /// <returns>A new <see cref="DocumentId"/>.</returns>
    public static DocumentId New() => new DocumentId(Guid.NewGuid());

    /// <summary>
    /// Returns the string representation of the ID.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString();
}
