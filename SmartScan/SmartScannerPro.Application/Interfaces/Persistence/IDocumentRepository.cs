namespace SmartScannerPro.Application.Interfaces.Persistence;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.Entities;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides a contract for managing scanned documents in persistence.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Gets a document by its identifier.
    /// </summary>
    /// <param name="id">The document identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The scan document, or null if not found.</returns>
    Task<ScanDocument?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all scanned documents.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of scanned documents.</returns>
    Task<IReadOnlyList<ScanDocument>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new document to the repository.
    /// </summary>
    /// <param name="document">The scan document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(ScanDocument document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing document in the repository.
    /// </summary>
    /// <param name="document">The scan document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(ScanDocument document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a document from the repository.
    /// </summary>
    /// <param name="document">The scan document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(ScanDocument document, CancellationToken cancellationToken = default);
}
