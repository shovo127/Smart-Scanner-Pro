namespace SmartScannerPro.Application.Interfaces.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides operations for managing scan profiles.
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Gets all active profiles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of profiles.</returns>
    Task<IEnumerable<object>> GetProfilesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a profile by its identifier.
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The profile, or null if not found.</returns>
    Task<object?> GetProfileAsync(ProfileId profileId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a profile as the default.
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetAsDefaultAsync(ProfileId profileId, CancellationToken cancellationToken = default);
}
