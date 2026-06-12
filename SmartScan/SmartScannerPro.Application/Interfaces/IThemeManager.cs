namespace SmartScannerPro.Application.Interfaces;

/// <summary>
/// Defines the available application themes.
/// </summary>
public enum AppTheme
{
    /// <summary>
    /// Uses the system's default theme.
    /// </summary>
    System,

    /// <summary>
    /// Light theme.
    /// </summary>
    Light,

    /// <summary>
    /// Dark theme.
    /// </summary>
    Dark,
}

/// <summary>
/// An abstraction for managing the application theme.
/// </summary>
public interface IThemeManager
{
    /// <summary>
    /// Gets the current theme.
    /// </summary>
    AppTheme CurrentTheme { get; }

    /// <summary>
    /// Sets the application theme.
    /// </summary>
    /// <param name="theme">The theme to set.</param>
    void SetTheme(AppTheme theme);
}
