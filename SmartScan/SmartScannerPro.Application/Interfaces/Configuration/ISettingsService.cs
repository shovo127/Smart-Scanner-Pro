namespace SmartScannerPro.Application.Interfaces.Configuration;

/// <summary>
/// Represents the global application settings.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Gets or sets the application theme.
    /// </summary>
    public string Theme { get; set; } = "System";

    /// <summary>
    /// Gets or sets the application language.
    /// </summary>
    public string Language { get; set; } = "en-US";

    /// <summary>
    /// Gets or sets the default scanner ID.
    /// </summary>
    public string DefaultScannerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default output directory.
    /// </summary>
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
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the application settings.</returns>
    Task<AppSettings> GetSettingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the application settings.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveSettingsAsync(AppSettings settings, CancellationToken cancellationToken = default);
}
