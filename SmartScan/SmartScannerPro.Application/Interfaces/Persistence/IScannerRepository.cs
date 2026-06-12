namespace SmartScannerPro.Application.Interfaces.Persistence;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.Entities;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides a contract for managing scanner devices in persistence.
/// </summary>
public interface IScannerRepository
{
    /// <summary>
    /// Gets a scanner by its identifier.
    /// </summary>
    /// <param name="id">The scanner identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The scanner device, or null if not found.</returns>
    Task<ScannerDevice?> GetByIdAsync(ScannerId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all scanner devices.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of scanner devices.</returns>
    Task<IReadOnlyList<ScannerDevice>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new scanner device to the repository.
    /// </summary>
    /// <param name="scanner">The scanner device.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(ScannerDevice scanner, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing scanner device in the repository.
    /// </summary>
    /// <param name="scanner">The scanner device.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(ScannerDevice scanner, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a scanner device from the repository.
    /// </summary>
    /// <param name="scanner">The scanner device.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(ScannerDevice scanner, CancellationToken cancellationToken = default);
}
