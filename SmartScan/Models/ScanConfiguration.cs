namespace SmartScan.Models
{
    public class ScanConfiguration
    {
        public string ScannerName { get; set; } = string.Empty;
        public string PaperSource { get; set; } = "ADF";
        public string ColorMode { get; set; } = "Color";
        public int Dpi { get; set; } = 300;
        public string PageSize { get; set; } = "A4";
    }
}
