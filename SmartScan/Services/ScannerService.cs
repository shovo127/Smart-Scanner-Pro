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
            // Initialization for NAPS2 SDK goes here
        }

        public async Task<List<string>> GetAvailableScannersAsync()
        {
            var scanners = new List<string>();
            try
            {
                // TODO: Implement actual NAPS2 scanner detection
                await Task.Delay(1000); // Simulate network/device query
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
    }
}
