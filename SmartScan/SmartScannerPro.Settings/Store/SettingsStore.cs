namespace SmartScannerPro.Settings.Store;
using System.Text.Json;
using SmartScannerPro.Application.Exceptions;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.Application.Interfaces.Configuration;

/// <summary>
/// Handles the persistence of application settings.
/// </summary>
public class SettingsStore
{
    private readonly IDirectoryManager directoryManager;
    private readonly string settingsFilePath;
    private readonly JsonSerializerOptions jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsStore"/> class.
    /// </summary>
    /// <param name="directoryManager">The directory manager.</param>
    public SettingsStore(IDirectoryManager directoryManager)
    {
        this.directoryManager = directoryManager;
        this.settingsFilePath = Path.Combine(this.directoryManager.SettingsDirectory, "appsettings.json");
        this.jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };
    }

    /// <summary>
    /// Loads the settings from disk.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The loaded application settings.</returns>
    public async Task<AppSettings> LoadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(this.settingsFilePath))
        {
            return new AppSettings();
        }

        try
        {
            using var stream = new FileStream(this.settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var settings = await JsonSerializer.DeserializeAsync<AppSettings>(stream, this.jsonOptions, cancellationToken);
            return settings ?? new AppSettings();
        }
        catch (JsonException ex)
        {
            throw new ConfigurationException("Failed to parse settings file.", ex);
        }
        catch (IOException ex)
        {
            throw new ConfigurationException("Failed to read settings file.", ex);
        }
    }

    /// <summary>
    /// Saves the settings to disk.
    /// </summary>
    /// <param name="settings">The application settings to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveAsync(AppSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var tempFile = this.settingsFilePath + ".tmp";
            using (var stream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await JsonSerializer.SerializeAsync(stream, settings, this.jsonOptions, cancellationToken);
            }

            File.Move(tempFile, this.settingsFilePath, true);
        }
        catch (IOException ex)
        {
            throw new ConfigurationException("Failed to write settings file.", ex);
        }
    }
}
