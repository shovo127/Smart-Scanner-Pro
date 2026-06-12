namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the type or category of a scanned document.
/// </summary>
public readonly record struct DocumentType
{
    /// <summary>
    /// Gets the document type name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentType"/> struct.
    /// </summary>
    /// <param name="name">The document type name.</param>
    public DocumentType(string name)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        this.Name = name;
    }

    /// <summary>
    /// Predefined standard document type.
    /// </summary>
    public static readonly DocumentType Standard = new DocumentType("Standard Document");

    /// <summary>
    /// Predefined photo type.
    /// </summary>
    public static readonly DocumentType Photo = new DocumentType("Photo");

    /// <summary>
    /// Predefined receipt type.
    /// </summary>
    public static readonly DocumentType Receipt = new DocumentType("Receipt");

    /// <summary>
    /// Predefined ID card type.
    /// </summary>
    public static readonly DocumentType IdCard = new DocumentType("ID Card");

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Name;
}
