using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartScan.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SmartScan.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IScannerService _scannerService;

        [ObservableProperty]
        private ObservableCollection<string> _scanners = new();

        [ObservableProperty]
        private string _selectedScanner;

        [ObservableProperty]
        private bool _isLoading = false;

        public HomeViewModel(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        [RelayCommand]
        private async Task LoadScannersAsync()
        {
            IsLoading = true;
            Scanners.Clear();
            Scanners.Add("Searching for scanners...");

            var foundScanners = await _scannerService.GetAvailableScannersAsync();
            
            Scanners.Clear();
            foreach (var scanner in foundScanners)
            {
                Scanners.Add(scanner);
            }

            var last = _scannerService.GetLastUsedScanner();
            if (!string.IsNullOrEmpty(last) && Scanners.Contains(last))
            {
                SelectedScanner = last;
            }
            else if (Scanners.Count > 0)
            {
                SelectedScanner = Scanners[0];
            }

            IsLoading = false;
        }

        partial void OnSelectedScannerChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && !value.StartsWith("Searching") && !value.StartsWith("Error") && !value.StartsWith("No scanner"))
            {
                _scannerService.SetLastUsedScanner(value);
            }
        }
    }
}
