using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartScan.Models;
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
        private string _selectedScanner = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        public bool IsNotLoading => !IsLoading;

        partial void OnIsLoadingChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotLoading));
        }

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        // Paper Source
        [ObservableProperty]
        private ObservableCollection<string> _paperSources = new(new[] { "ADF", "Flatbed" });
        [ObservableProperty]
        private string _selectedPaperSource = "ADF";

        // Color Mode
        [ObservableProperty]
        private ObservableCollection<string> _colorModes = new(new[] { "Color", "Gray", "Black" });
        [ObservableProperty]
        private string _selectedColorMode = "Color";

        // DPI
        [ObservableProperty]
        private ObservableCollection<int> _dpis = new(new[] { 75, 100, 150, 200, 300, 600 });
        [ObservableProperty]
        private int _selectedDpi = 150;

        // Page Size
        [ObservableProperty]
        private ObservableCollection<string> _pageSizes = new(new[] { "A4", "Legal", "Letter", "A5", "Custom" });
        [ObservableProperty]
        private string _selectedPageSize = "A4";

        public HomeViewModel(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        [RelayCommand]
        private async Task LoadScannersAsync()
        {
            IsLoading = true;
            StatusMessage = "Searching for scanners...";
            Scanners.Clear();
            Scanners.Add("Searching...");

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
            StatusMessage = "Ready";
        }

        [RelayCommand]
        private async Task StartScanAsync()
        {
            if (string.IsNullOrEmpty(SelectedScanner) || SelectedScanner.StartsWith("Search") || SelectedScanner.StartsWith("Error") || SelectedScanner.StartsWith("No "))
            {
                StatusMessage = "Please select a valid scanner first.";
                return;
            }

            IsLoading = true;
            StatusMessage = $"Scanning from {SelectedScanner}...";

            var config = new ScanConfiguration
            {
                ScannerName = SelectedScanner,
                PaperSource = SelectedPaperSource,
                ColorMode = SelectedColorMode,
                Dpi = SelectedDpi,
                PageSize = SelectedPageSize
            };

            bool success = await _scannerService.ScanAsync(config);

            IsLoading = false;
            StatusMessage = success ? "Scan completed successfully!" : "Scan failed.";
        }

        partial void OnSelectedScannerChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && !value.StartsWith("Searching") && !value.StartsWith("Error") && !value.StartsWith("No "))
            {
                _scannerService.SetLastUsedScanner(value);
            }
        }
    }
}
