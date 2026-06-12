namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides access to scan profiles.
/// </summary>
public interface IScanProfileProvider
{
    /// <summary>
    /// Loads the configuration values from a specified profile.
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dictionary of capability keys and their configured values.</returns>
    Task<System.Collections.Generic.IReadOnlyDictionary<string, object>> LoadProfileAsync(ProfileId profileId, CancellationToken cancellationToken = default);
}
