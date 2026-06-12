namespace SmartScannerPro.Domain.Interfaces;

/// <summary>
/// Provides a contract for creating application and user settings.
/// </summary>
public interface ISettingsFactory
{
    /// <summary>
    /// Creates default application settings.
    /// </summary>
    /// <returns>A new application settings instance.</returns>
    object CreateDefaultApplicationSettings(); // Replace object with ApplicationSettings

    /// <summary>
    /// Creates default user settings.
    /// </summary>
    /// <returns>A new user settings instance.</returns>
    object CreateDefaultUserSettings(); // Replace object with UserSettings
}
