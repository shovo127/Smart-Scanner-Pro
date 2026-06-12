namespace SmartScan.Services
{
    using SmartScan.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IScannerService
    {
        Task<List<string>> GetAvailableScannersAsync();
        string GetLastUsedScanner();
        void SetLastUsedScanner(string scannerName);
        Task<List<string>> ScanAsync(ScanConfiguration config, Func<Task<bool>>? onPromptFlip = null);
    }
}
