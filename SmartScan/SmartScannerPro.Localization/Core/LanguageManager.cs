namespace SmartScannerPro.Localization.Core;
using System.Globalization;
using SmartScannerPro.Application.Interfaces;

/// <summary>
/// Manages the application's localization and culture settings.
/// </summary>
public class LanguageManager : ILocalizationService
{
    private CultureInfo currentCulture = new CultureInfo("en-US");

    /// <inheritdoc/>
    public CultureInfo CurrentCulture => this.currentCulture;

    /// <inheritdoc/>
    public void SetCulture(CultureInfo cultureInfo)
    {
        this.currentCulture = cultureInfo;
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    /// <inheritdoc/>
    public string GetString(string key)
    {
        // Placeholder for resource manager lookup.
        return key;
    }
}
