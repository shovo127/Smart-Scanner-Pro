using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Appearance;

namespace SmartScan.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isDarkMode = true;

        public SettingsViewModel()
        {
            var currentTheme = ApplicationThemeManager.GetAppTheme();
            IsDarkMode = currentTheme == ApplicationTheme.Dark;
        }

        partial void OnIsDarkModeChanged(bool value)
        {
            ApplicationThemeManager.Apply(value ? ApplicationTheme.Dark : ApplicationTheme.Light);
        }
    }
}
