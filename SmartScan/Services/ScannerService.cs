using SmartScan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NAPS2.Scan;
using NAPS2.Images;
using NAPS2.Images.Gdi;
using NAPS2.Ocr;
using NAPS2.Pdf;

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
                using var context = new ScanningContext(new GdiImageContext());
                var controller = new ScanController(context);
                var devices = await controller.GetDeviceList();
                foreach(var device in devices)
                {
                    scanners.Add(device.Name);
                }
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

        public async Task<List<string>> ScanAsync(ScanConfiguration config, Func<Task<bool>>? onPromptFlip = null)
        {
            var resultFiles = new List<string>();
            try
            {
                var ocrEngine = TesseractOcrEngine.Bundled(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"));
                using var context = new ScanningContext(new GdiImageContext()) { OcrEngine = ocrEngine };
                var controller = new ScanController(context);
                
                var device = (await controller.GetDeviceList()).FirstOrDefault(d => d.Name == config.ScannerName);
                if (device == null) throw new Exception("Scanner not found");

                var options = new ScanOptions
                {
                    Device = device,
                    PaperSource = config.PaperSource == "Flatbed" ? PaperSource.Flatbed : PaperSource.Feeder,
                    PageSize = ParsePageSize(config.PageSize),
                    Dpi = config.Dpi,
                    BitDepth = ParseColorMode(config.ColorMode)
                };

                if (config.PaperSource == "Manual Duplex")
                {
                    // Scan Front
                    var frontImages = await PerformScanAsync(controller, options);
                    
                    if (frontImages.Count > 0 && onPromptFlip != null)
                    {
                        bool userContinued = await onPromptFlip();
                        if (userContinued)
                        {
                            // Scan Back
                            var backImages = await PerformScanAsync(controller, options);
                            
                            // Reverse second scan
                            backImages.Reverse();

                            // Alternate Interleave
                            var finalImages = new List<ProcessedImage>();
                            int maxCount = Math.Max(frontImages.Count, backImages.Count);
                            for(int i = 0; i < maxCount; i++)
                            {
                                if (i < frontImages.Count) finalImages.Add(frontImages[i]);
                                if (i < backImages.Count) finalImages.Add(backImages[i]);
                            }
                            
                            var pdfPath = await SaveAsSearchablePdfAsync(context, finalImages);
                            resultFiles.Add(pdfPath);
                        }
                    }
                }
                else
                {
                    var images = await PerformScanAsync(controller, options);
                    var pdfPath = await SaveAsSearchablePdfAsync(context, images);
                    resultFiles.Add(pdfPath);
                }

                return resultFiles;
            }
            catch
            {
                return new List<string>();
            }
        }

        private async Task<string> SaveAsSearchablePdfAsync(ScanningContext context, List<ProcessedImage> images)
        {
            if (images.Count == 0) return string.Empty;

            string outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SmartScan");
            Directory.CreateDirectory(outputDir);
            string filePath = Path.Combine(outputDir, $"Document_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            var pdfExporter = new PdfExporter(context);
            var ocrParams = new OcrParams("eng+deu+ben+ara", OcrMode.Fast);

            await pdfExporter.Export(filePath, images, null, ocrParams);

            return filePath;
        }

        private async Task<List<ProcessedImage>> PerformScanAsync(ScanController controller, ScanOptions options)
        {
            var images = new List<ProcessedImage>();
            await foreach (var image in controller.Scan(options))
            {
                images.Add(image);
            }
            return images;
        }

        private PageSize ParsePageSize(string pageSize)
        {
            return pageSize switch
            {
                "A4" => PageSize.A4,
                "Legal" => PageSize.Legal,
                "Letter" => PageSize.Letter,
                "A5" => PageSize.A5,
                "Custom" => PageSize.A4, // Fallback to A4 for Custom since it might not be supported directly by enum
                _ => PageSize.A4
            };
        }

        private BitDepth ParseColorMode(string colorMode)
        {
            return colorMode switch
            {
                "Color" => BitDepth.Color,
                "Gray" => BitDepth.Grayscale,
                "Black" => BitDepth.BlackAndWhite,
                _ => BitDepth.Color
            };
        }
    }
}
