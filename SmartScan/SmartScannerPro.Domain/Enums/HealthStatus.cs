namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the health status of a hardware or software component.
/// </summary>
public enum HealthStatus
{
    /// <summary>
    /// The component is operating normally.
    /// </summary>
    Healthy = 0,

    /// <summary>
    /// The component is functioning but experiencing issues or delays.
    /// </summary>
    Degraded = 1,

    /// <summary>
    /// The component has failed or is disconnected.
    /// </summary>
    Unhealthy = 2,
}
