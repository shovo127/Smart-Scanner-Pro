namespace SmartScannerPro.Settings.Store;
using SmartScannerPro.Application.Interfaces.Configuration;

/// <summary>
/// Manages application settings via the <see cref="SettingsStore"/>.
/// </summary>
public class SettingsManager : ISettingsService
{
    private readonly SettingsStore store;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsManager"/> class.
    /// </summary>
    /// <param name="store">The settings store.</param>
    public SettingsManager(SettingsStore store)
    {
        this.store = store;
    }

    /// <inheritdoc/>
    public Task<AppSettings> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        return this.store.LoadAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task SaveSettingsAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        return this.store.SaveAsync(settings, cancellationToken);
    }
}
