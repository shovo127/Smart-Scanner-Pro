namespace SmartScannerPro.Application.Interfaces.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.Entities;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides operations for managing scanned documents.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Gets a document by its identifier.
    /// </summary>
    /// <param name="documentId">The document identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The document, or null if not found.</returns>
    Task<ScanDocument?> GetDocumentAsync(DocumentId documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all recent documents.
    /// </summary>
    /// <param name="limit">The maximum number of documents to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of recent documents.</returns>
    Task<IEnumerable<ScanDocument>> GetRecentDocumentsAsync(int limit, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document permanently.
    /// </summary>
    /// <param name="documentId">The document identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteDocumentAsync(DocumentId documentId, CancellationToken cancellationToken = default);
}
