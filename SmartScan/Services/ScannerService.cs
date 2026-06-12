using SmartScan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmartScan.Services
{
    public class ScannerService : IScannerService
    {
        private const string LastScannerFile = "last_scanner.txt";

        public ScannerService()
        {
        }

        public async Task<List<string>> GetAvailableScannersAsync()
        {
            var scanners = new List<string>();
            try
            {
                await Task.Delay(1000); 
                scanners.Add("[WIA] Pantum M6550NW");
                scanners.Add("[eSCL] Pantum M6550NW Network");
            }
            catch (Exception ex)
            {
                scanners.Add($"Error: {ex.Message}");
            }

            if (scanners.Count == 0)
            {
                scanners.Add("No scanners found.");
            }

            return scanners;
        }

        public string GetLastUsedScanner()
        {
            if (File.Exists(LastScannerFile))
            {
                return File.ReadAllText(LastScannerFile);
            }
            return string.Empty;
        }

        public void SetLastUsedScanner(string scannerName)
        {
            if (!string.IsNullOrEmpty(scannerName))
            {
                File.WriteAllText(LastScannerFile, scannerName);
            }
        }

        public async Task<bool> ScanAsync(ScanConfiguration config)
        {
            try
            {
                // TODO: Wire up actual NAPS2.Sdk Scan call once scanner hardware is connected
                // Example pseudo-code for NAPS2 SDK:
                // var options = new ScanOptions {
                //     Device = new Device(config.ScannerName),
                //     PaperSource = config.PaperSource == "ADF" ? PaperSource.Feeder : PaperSource.Flatbed,
                //     PageSize = ParsePageSize(config.PageSize),
                //     Dpi = config.Dpi,
                //     BitDepth = ParseColorMode(config.ColorMode)
                // };
                // await foreach(var image in scanningContext.ScanAsync(options)) { ... }
                
                await Task.Delay(2000); // Simulate scanning delay
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
