namespace SmartScannerPro.Domain.ValueObjects;

using System;

/// <summary>
/// Represents the strongly-typed identifier for a Document.
/// </summary>
public readonly record struct DocumentId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentId"/> struct.
    /// </summary>
    /// <param name="value">The underlying Guid value.</param>
    public DocumentId(Guid value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Gets a new, empty DocumentId.
    /// </summary>
    public static DocumentId Empty => new DocumentId(Guid.Empty);

    /// <summary>
    /// Creates a new DocumentId.
    /// </summary>
    /// <returns>A new DocumentId.</returns>
    public static DocumentId NewId() => new DocumentId(Guid.NewGuid());
}
