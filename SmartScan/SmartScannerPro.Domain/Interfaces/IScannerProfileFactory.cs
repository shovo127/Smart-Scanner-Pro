namespace SmartScannerPro.Domain.Interfaces;

/// <summary>
/// Provides a contract for creating scanner profiles.
/// </summary>
public interface IScannerProfileFactory
{
    /// <summary>
    /// Creates a new default scanner profile.
    /// </summary>
    /// <param name="name">The profile name.</param>
    /// <returns>A new profile instance.</returns>
    object CreateDefault(string name); // Replace object with ScannerProfile when implemented
}
