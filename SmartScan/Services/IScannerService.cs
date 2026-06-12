namespace SmartScan.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IScannerService
    {
        Task<List<string>> GetAvailableScannersAsync();
        string GetLastUsedScanner();
        void SetLastUsedScanner(string scannerName);
    }
}
