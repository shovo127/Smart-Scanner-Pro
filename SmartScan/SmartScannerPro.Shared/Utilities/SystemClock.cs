namespace SmartScannerPro.Shared.Utilities;

/// <summary>
/// A default implementation of <see cref="IClock"/> that uses the system time.
/// </summary>
public sealed class SystemClock : IClock
{
    /// <inheritdoc/>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <inheritdoc/>
    public DateTime Now => DateTime.Now;
}
