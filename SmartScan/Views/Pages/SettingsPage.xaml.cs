using SmartScan.ViewModels;
using System.Windows.Controls;

namespace SmartScan.Views.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
