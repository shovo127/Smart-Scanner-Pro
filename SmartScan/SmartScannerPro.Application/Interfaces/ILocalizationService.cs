namespace SmartScannerPro.Application.Interfaces;
using System.Globalization;

/// <summary>
/// An abstraction for managing application localization.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Gets the current culture.
    /// </summary>
    CultureInfo CurrentCulture { get; }

    /// <summary>
    /// Sets the application culture.
    /// </summary>
    /// <param name="cultureInfo">The culture info.</param>
    void SetCulture(CultureInfo cultureInfo);

    /// <summary>
    /// Gets a localized string by key.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <returns>The localized string.</returns>
    string GetString(string key);
}
