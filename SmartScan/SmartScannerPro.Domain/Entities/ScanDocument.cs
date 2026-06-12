namespace SmartScannerPro.Domain.Entities;

using System;
using System.Collections.Generic;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a document consisting of one or more scanned pages.
/// </summary>
public class ScanDocument
{
    private readonly List<ScanPage> pages = new List<ScanPage>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScanDocument"/> class.
    /// </summary>
    /// <param name="id">The document ID.</param>
    /// <param name="name">The document name.</param>
    /// <param name="type">The document type.</param>
    public ScanDocument(DocumentId id, string name, DocumentType type)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));

        this.Id = id;
        this.Name = name;
        this.Type = type;
        this.CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier for the document.
    /// </summary>
    public DocumentId Id { get; }

    /// <summary>
    /// Gets the name of the document.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the creation timestamp.
    /// </summary>
    public DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Gets the document type.
    /// </summary>
    public DocumentType Type { get; private set; }

    /// <summary>
    /// Gets a read-only collection of the pages in this document.
    /// </summary>
    public IReadOnlyCollection<ScanPage> Pages => this.pages.AsReadOnly();

    /// <summary>
    /// Updates the document name.
    /// </summary>
    /// <param name="newName">The new name.</param>
    public void Rename(string newName)
    {
        Guard.NotNullOrWhiteSpace(newName, nameof(newName));
        this.Name = newName;
    }

    /// <summary>
    /// Adds a page to the document.
    /// </summary>
    /// <param name="page">The page to add.</param>
    public void AddPage(ScanPage page)
    {
        Guard.NotNull(page, nameof(page));
        this.pages.Add(page);
    }

    /// <summary>
    /// Removes a page from the document.
    /// </summary>
    /// <param name="page">The page to remove.</param>
    public void RemovePage(ScanPage page)
    {
        Guard.NotNull(page, nameof(page));
        this.pages.Remove(page);
    }
}
