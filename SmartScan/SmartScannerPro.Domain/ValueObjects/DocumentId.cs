namespace SmartScannerPro.Domain.ValueObjects;

using System;
using SmartScannerPro.Domain.Abstractions;

/// <summary>
/// Represents the unique identifier for a scanned document.
/// </summary>
public sealed class DocumentId : StronglyTypedId<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentId"/> class.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public DocumentId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a new, unique <see cref="DocumentId"/>.
    /// </summary>
    /// <returns>A new <see cref="DocumentId"/>.</returns>
    public static DocumentId New() => new DocumentId(Guid.NewGuid());
}
