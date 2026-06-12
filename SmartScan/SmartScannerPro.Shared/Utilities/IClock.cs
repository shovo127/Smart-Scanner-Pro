namespace SmartScannerPro.Shared.Utilities;

/// <summary>
/// An abstraction for accessing the system clock.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current Coordinated Universal Time (UTC).
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local time.
    /// </summary>
    DateTime Now { get; }
}
