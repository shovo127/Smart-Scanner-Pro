namespace SmartScannerPro.UI.Theme;
using SmartScannerPro.Application.Interfaces;

/// <summary>
/// Manages the application theme for the WPF UI.
/// </summary>
public class ThemeManager : IThemeManager
{
    /// <inheritdoc/>
    public AppTheme CurrentTheme { get; private set; } = AppTheme.System;

    /// <inheritdoc/>
    public void SetTheme(AppTheme theme)
    {
        this.CurrentTheme = theme;

        // In a complete implementation, this would switch WPF ResourceDictionaries.
    }
}
