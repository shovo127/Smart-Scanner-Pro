namespace SmartScannerPro.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides operations for exporting scanned documents.
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Exports a document to the specified destination path.
    /// </summary>
    /// <param name="documentId">The document identifier.</param>
    /// <param name="destinationPath">The destination directory or file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The URI of the exported file.</returns>
    Task<StorageUri> ExportDocumentAsync(DocumentId documentId, string destinationPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports a single page as an image.
    /// </summary>
    /// <param name="documentId">The document identifier.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="destinationPath">The destination file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The URI of the exported image.</returns>
    Task<StorageUri> ExportPageAsync(DocumentId documentId, PageNumber pageNumber, string destinationPath, CancellationToken cancellationToken = default);
}
