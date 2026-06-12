namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the logical type of a scanned document.
/// </summary>
public record DocumentType
{
    /// <summary>
    /// Gets the Receipt type.
    /// </summary>
    public static readonly DocumentType Receipt = new DocumentType("Receipt");

    /// <summary>
    /// Gets the Invoice type.
    /// </summary>
    public static readonly DocumentType Invoice = new DocumentType("Invoice");

    /// <summary>
    /// Gets the Photo type.
    /// </summary>
    public static readonly DocumentType Photo = new DocumentType("Photo");

    /// <summary>
    /// Gets the General Document type.
    /// </summary>
    public static readonly DocumentType Document = new DocumentType("Document");

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentType"/> class.
    /// </summary>
    /// <param name="value">The document type name.</param>
    public DocumentType(string value)
    {
        Guard.NotNullOrWhiteSpace(value, nameof(value));
        this.Value = value;
    }

    /// <summary>
    /// Gets the document type value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the string representation of the document type.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value;

}
