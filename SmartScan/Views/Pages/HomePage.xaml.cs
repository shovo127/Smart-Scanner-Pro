using SmartScan.ViewModels;
using System.Windows.Controls;

namespace SmartScan.Views.Pages
{
    public partial class HomePage : Page
    {
        public HomePage(HomeViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
