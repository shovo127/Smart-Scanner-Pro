namespace SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Specifies the priority or preference level of a driver when multiple drivers are available for the same device.
/// </summary>
public enum DriverPriority
{
    /// <summary>
    /// Lowest priority, used only as a fallback.
    /// </summary>
    Fallback = 0,

    /// <summary>
    /// Normal priority.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Preferred priority.
    /// </summary>
    Preferred = 2,

    /// <summary>
    /// Highest priority, specifically requested by the user.
    /// </summary>
    UserSelected = 3
}
