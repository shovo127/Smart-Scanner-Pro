namespace SmartScannerPro.Application.Interfaces.Configuration;

/// <summary>
/// Represents the global application settings.
/// </summary>
public class AppSettings
{
    public string Theme { get; set; } = "System";
    public string Language { get; set; } = "en-US";
    public string DefaultScannerId { get; set; } = string.Empty;
    public string OutputDirectory { get; set; } = string.Empty;
}

/// <summary>
/// Abstraction for managing application settings.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Retrieves the current application settings.
    /// </summary>
    Task<AppSettings> GetSettingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the application settings.
    /// </summary>
    Task SaveSettingsAsync(AppSettings settings, CancellationToken cancellationToken = default);
}
