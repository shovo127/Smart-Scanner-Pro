namespace SmartScannerPro.Application.Interfaces.Persistence;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;

// Note: ScannerProfile entity will be fully defined soon, but we define the repository contract now.
// using SmartScannerPro.Domain.Entities;

/// <summary>
/// Provides a contract for managing scanner profiles in persistence.
/// </summary>
public interface IProfileRepository
{
    // These signatures use object as a placeholder until ScannerProfile is implemented.

    /// <summary>
    /// Gets a profile by its identifier.
    /// </summary>
    /// <param name="id">The profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The profile, or null if not found.</returns>
    Task<object?> GetByIdAsync(ProfileId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all scanner profiles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of profiles.</returns>
    Task<IReadOnlyList<object>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new profile to the repository.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(object profile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing profile in the repository.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(object profile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a profile from the repository.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(object profile, CancellationToken cancellationToken = default);
}
