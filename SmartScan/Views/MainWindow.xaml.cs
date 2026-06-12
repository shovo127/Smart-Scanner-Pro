using SmartScan.Services;
using SmartScan.ViewModels;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace SmartScan.Views
{
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService, IPageService pageService)
        {
            DataContext = viewModel;
            InitializeComponent();
            
            navigationService.SetNavigationControl(RootNavigation);
            
            Loaded += (s, e) => RootNavigation.Navigate(typeof(Pages.HomePage));
        }
    }
}
