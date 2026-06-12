namespace SmartScannerPro.Domain.Interfaces;

using SmartScannerPro.Domain.Entities;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides a contract for creating scan documents.
/// </summary>
public interface IScanDocumentFactory
{
    /// <summary>
    /// Creates a new scan document.
    /// </summary>
    /// <param name="name">The document name.</param>
    /// <param name="type">The document type.</param>
    /// <returns>A new scan document instance.</returns>
    ScanDocument Create(string name, DocumentType type);
}
