namespace SmartScannerPro.Scanner.Mock.Simulation;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Generates simulated scanner document images programmatically using SixLabors.ImageSharp.
/// No binary assets are required. All images are produced deterministically in memory,
/// applying optional scanning artefacts such as noise, skew, and colour mode conversion.
/// </summary>
public sealed class MockImageGenerator
{
    private readonly Random random = new();

    /// <summary>
    /// Specifies the type of document to generate for a simulated scan page.
    /// </summary>
    public enum DocumentType
    {
        /// <summary>A uniformly white page with minimal noise.</summary>
        BlankPage,

        /// <summary>A structured document resembling a commercial invoice.</summary>
        Invoice,

        /// <summary>A narrow-column receipt document.</summary>
        Receipt,

        /// <summary>A wide book page with justified text blocks.</summary>
        BookPage,

        /// <summary>A formal contract document with header and signature block.</summary>
        Contract,

        /// <summary>A vivid colour photograph with gradient tones.</summary>
        Photo,

        /// <summary>A biometric-layout passport page.</summary>
        Passport,

        /// <summary>A procedurally-generated QR code pattern.</summary>
        QrCode,

        /// <summary>A procedurally-generated barcode pattern.</summary>
        Barcode,
    }

    /// <summary>
    /// Generates a simulated scanned image asynchronously.
    /// </summary>
    /// <param name="pageIndex">The zero-based page index within the scan job.</param>
    /// <param name="colorProfile">The colour mode to apply to the output.</param>
    /// <param name="resolutionDpi">The target resolution in dots per inch.</param>
    /// <param name="addNoise">When <c>true</c>, applies random pixel noise to simulate scanner imperfections.</param>
    /// <param name="addSkew">When <c>true</c>, applies a small rotation to simulate document misalignment.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="RawImage"/> wrapping a PNG-encoded stream and its metadata.</returns>
    public async Task<RawImage> GenerateAsync(
        int pageIndex,
        ColorProfile colorProfile,
        int resolutionDpi,
        bool addNoise = false,
        bool addSkew = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var documentType = SelectDocumentType(pageIndex);
        var (widthPx, heightPx) = ComputeDimensions(resolutionDpi);

        using var image = GenerateDocument(documentType, widthPx, heightPx);

        if (addSkew)
        {
            ApplySkew(image);
        }

        if (addNoise)
        {
            ApplyNoise(image);
        }

        ApplyColorMode(image, colorProfile);

        var stream = new MemoryStream();
        await image.SaveAsync(stream, new PngEncoder(), cancellationToken).ConfigureAwait(false);
        stream.Position = 0;

        var metadata = new ImageMetadata
        {
            WidthInPixels = widthPx,
            HeightInPixels = heightPx,
            ResolutionX = resolutionDpi,
            ResolutionY = resolutionDpi,
            BitDepth = colorProfile == ColorProfile.BlackAndWhite ? 1 : 8,
            ColorProfile = colorProfile,
            Source = ImageSource.AdfFront,
            Compression = Compression.None,
            Format = ImageFormat.Png,
        };

        return new RawImage { Metadata = metadata, DataStream = stream };
    }

    private static DocumentType SelectDocumentType(int pageIndex)
    {
        var types = (DocumentType[])Enum.GetValues(typeof(DocumentType));
        return types[pageIndex % types.Length];
    }

    private static (int width, int height) ComputeDimensions(int dpi)
    {
        // A4 at the given DPI (210mm × 297mm)
        var widthPx = (int)(210.0 / 25.4 * dpi);
        var heightPx = (int)(297.0 / 25.4 * dpi);
        return (widthPx, heightPx);
    }

    private static Image<Rgba32> GenerateDocument(DocumentType type, int width, int height)
    {
        return type switch
        {
            DocumentType.BlankPage => GenerateBlankPage(width, height),
            DocumentType.Invoice => GenerateInvoice(width, height),
            DocumentType.Receipt => GenerateReceipt(width, height),
            DocumentType.BookPage => GenerateBookPage(width, height),
            DocumentType.Contract => GenerateContract(width, height),
            DocumentType.Photo => GeneratePhoto(width, height),
            DocumentType.Passport => GeneratePassport(width, height),
            DocumentType.QrCode => GenerateQrCode(width, height),
            DocumentType.Barcode => GenerateBarcode(width, height),
            _ => GenerateBlankPage(width, height),
        };
    }

    private static Image<Rgba32> GenerateBlankPage(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, new Rgba32(252, 252, 252));
        return img;
    }

    private static Image<Rgba32> GenerateInvoice(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.White);
        img.Mutate(ctx =>
        {
            var margin = width / 20;
            var headerH = height / 10;

            // Header band
            ctx.Fill(new SolidBrush(Color.FromRgb(30, 80, 160)), new RectangularPolygon(0, 0, width, headerH));

            // Logo placeholder
            ctx.Fill(new SolidBrush(Color.White), new RectangularPolygon(margin, margin / 2, width / 5, headerH - margin));

            // Invoice title bar
            ctx.Fill(new SolidBrush(Color.FromRgb(220, 230, 245)), new RectangularPolygon(margin, headerH + margin, width - 2 * margin, height / 20));

            // Table rows
            int rowY = headerH + margin + height / 20 + margin;
            int rowHeight = height / 25;
            for (int i = 0; i < 8; i++)
            {
                var rowColor = i % 2 == 0 ? Color.FromRgb(245, 248, 255) : Color.White;
                ctx.Fill(new SolidBrush(rowColor), new RectangularPolygon(margin, rowY + i * (rowHeight + 4), width - 2 * margin, rowHeight));
            }

            // Footer line
            ctx.Fill(new SolidBrush(Color.FromRgb(200, 200, 200)), new RectangularPolygon(margin, height - height / 8, width - 2 * margin, 2));
        });
        return img;
    }

    private static Image<Rgba32> GenerateReceipt(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.White);
        img.Mutate(ctx =>
        {
            int margin = width / 10;
            int rowH = height / 40;
            // Header block
            ctx.Fill(new SolidBrush(Color.Black), new RectangularPolygon(margin, margin, width - 2 * margin, rowH));
            // Items
            for (int i = 0; i < 15; i++)
            {
                ctx.Fill(new SolidBrush(Color.FromRgb(220, 220, 220)),
                    new RectangularPolygon(margin, margin + rowH + margin + i * (rowH + 6), (width - 2 * margin) * 2 / 3, rowH / 2));
                ctx.Fill(new SolidBrush(Color.FromRgb(200, 200, 200)),
                    new RectangularPolygon(width - margin - width / 6, margin + rowH + margin + i * (rowH + 6), width / 6, rowH / 2));
            }
            // Total bar
            ctx.Fill(new SolidBrush(Color.Black), new RectangularPolygon(margin, height - margin - rowH, width - 2 * margin, rowH));
        });
        return img;
    }

    private static Image<Rgba32> GenerateBookPage(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.FromRgb(252, 248, 240));
        img.Mutate(ctx =>
        {
            int margin = width / 8;
            int lineH = height / 60;
            int lineSpacing = lineH + 4;
            for (int i = 0; i < 40; i++)
            {
                var lineWidth = i % 10 == 9 ? (width - 2 * margin) * 2 / 3 : width - 2 * margin;
                ctx.Fill(new SolidBrush(Color.FromRgb(50, 50, 50)),
                    new RectangularPolygon(margin, margin + i * lineSpacing, lineWidth, lineH / 2));
            }
            // Page number at bottom
            ctx.Fill(new SolidBrush(Color.FromRgb(120, 120, 120)),
                new RectangularPolygon(width / 2 - 20, height - margin, 40, lineH / 2));
        });
        return img;
    }

    private static Image<Rgba32> GenerateContract(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.White);
        img.Mutate(ctx =>
        {
            int margin = width / 12;
            int lineH = 4;
            int lineSpacing = height / 50;

            // Title block
            ctx.Fill(new SolidBrush(Color.Black), new RectangularPolygon(width / 4, margin, width / 2, lineH * 2));
            ctx.Fill(new SolidBrush(Color.FromRgb(100, 100, 100)), new RectangularPolygon(width / 3, margin + lineSpacing, width / 3, lineH));

            // Body text lines
            for (int i = 0; i < 30; i++)
            {
                var lineWidth = i % 8 == 7 ? (width - 2 * margin) / 2 : width - 2 * margin;
                ctx.Fill(new SolidBrush(Color.FromRgb(60, 60, 60)),
                    new RectangularPolygon(margin, margin + 3 * lineSpacing + i * (lineH + lineSpacing), lineWidth, lineH));
            }

            // Signature block
            int sigY = height - margin * 4;
            ctx.Fill(new SolidBrush(Color.FromRgb(180, 180, 180)), new RectangularPolygon(margin, sigY, width / 3, 1));
            ctx.Fill(new SolidBrush(Color.FromRgb(180, 180, 180)), new RectangularPolygon(width - margin - width / 3, sigY, width / 3, 1));
        });
        return img;
    }

    private static Image<Rgba32> GeneratePhoto(int width, int height)
    {
        var img = new Image<Rgba32>(width, height);
        img.Mutate(ctx =>
        {
            // Gradient background
            for (int y = 0; y < height; y++)
            {
                float t = (float)y / height;
                var color = Color.FromRgb(
                    (byte)(30 + 180 * t),
                    (byte)(100 - 60 * t),
                    (byte)(200 - 150 * t));
                ctx.Fill(new SolidBrush(color), new RectangularPolygon(0, y, width, 1));
            }

            // Decorative shapes
            ctx.Fill(new SolidBrush(Color.FromRgba(255, 200, 0, 120)),
                new EllipsePolygon(width / 3, height / 3, width / 4));
            ctx.Fill(new SolidBrush(Color.FromRgba(0, 200, 255, 100)),
                new EllipsePolygon(width * 2 / 3, height * 2 / 3, width / 5));
        });
        return img;
    }

    private static Image<Rgba32> GeneratePassport(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.FromRgb(240, 245, 255));
        img.Mutate(ctx =>
        {
            int margin = width / 12;

            // Header band
            ctx.Fill(new SolidBrush(Color.FromRgb(0, 60, 130)), new RectangularPolygon(0, 0, width, height / 10));

            // Photo placeholder
            ctx.Fill(new SolidBrush(Color.FromRgb(180, 180, 200)),
                new RectangularPolygon(margin, height / 8, width / 4, height / 3));

            // MRZ (Machine Readable Zone) — two rows at the bottom
            int mrzY = height - height / 8;
            ctx.Fill(new SolidBrush(Color.White), new RectangularPolygon(margin, mrzY, width - 2 * margin, height / 12));
            for (int i = 0; i < 40; i++)
            {
                ctx.Fill(new SolidBrush(Color.Black),
                    new RectangularPolygon(margin + i * ((width - 2 * margin) / 40), mrzY + 4, (width - 2 * margin) / 44, height / 30));
            }

            // Data fields
            int fieldY = height / 8;
            int fieldX = margin + width / 4 + margin;
            for (int i = 0; i < 6; i++)
            {
                ctx.Fill(new SolidBrush(Color.FromRgb(100, 100, 120)),
                    new RectangularPolygon(fieldX, fieldY + i * height / 18, width - fieldX - margin, height / 50));
            }
        });
        return img;
    }

    private static Image<Rgba32> GenerateQrCode(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.White);
        img.Mutate(ctx =>
        {
            int qrSize = Math.Min(width, height) * 2 / 3;
            int startX = (width - qrSize) / 2;
            int startY = (height - qrSize) / 2;
            int cellSize = qrSize / 25;

            // Simple deterministic QR-like pattern
            for (int row = 0; row < 25; row++)
            {
                for (int col = 0; col < 25; col++)
                {
                    bool fill = (row + col) % 3 == 0 || row < 7 && col < 7 || row < 7 && col > 17 || row > 17 && col < 7;
                    if (fill)
                    {
                        ctx.Fill(new SolidBrush(Color.Black),
                            new RectangularPolygon(startX + col * cellSize, startY + row * cellSize, cellSize - 1, cellSize - 1));
                    }
                }
            }

            // Quiet zone border
            ctx.Draw(Pens.Solid(Color.Black, 2), new RectangularPolygon(startX - 4, startY - 4, qrSize + 8, qrSize + 8));
        });
        return img;
    }

    private static Image<Rgba32> GenerateBarcode(int width, int height)
    {
        var img = new Image<Rgba32>(width, height, Color.White);
        img.Mutate(ctx =>
        {
            int barcodeW = width * 3 / 4;
            int barcodeH = height / 5;
            int startX = (width - barcodeW) / 2;
            int startY = (height - barcodeH) / 2;

            // Procedural barcode bars (Code-39 style pattern)
            int barCount = 60;
            float barUnitW = (float)barcodeW / (barCount * 2 + barCount);
            float x = startX;
            for (int i = 0; i < barCount; i++)
            {
                bool isWide = i % 5 == 0;
                float barW = isWide ? barUnitW * 3 : barUnitW;
                if (i % 2 == 0)
                {
                    ctx.Fill(new SolidBrush(Color.Black), new RectangularPolygon(x, startY, barW, barcodeH));
                }

                x += barW + barUnitW;
            }

            // Number line below barcode
            ctx.Fill(new SolidBrush(Color.FromRgb(80, 80, 80)),
                new RectangularPolygon(startX + barcodeW / 8, startY + barcodeH + 4, barcodeW * 3 / 4, height / 60));
        });
        return img;
    }

    private void ApplyNoise(Image<Rgba32> image)
    {
        var intensity = 12;
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < row.Length; x++)
                {
                    var delta = (byte)(this.random.Next(-intensity, intensity + 1) + 128);
                    ref var pixel = ref row[x];
                    pixel.R = ClampByte(pixel.R + delta - 128);
                    pixel.G = ClampByte(pixel.G + delta - 128);
                    pixel.B = ClampByte(pixel.B + delta - 128);
                }
            }
        });
    }

    private void ApplySkew(Image<Rgba32> image)
    {
        var skewDegrees = (float)(this.random.NextDouble() * 3.0 - 1.5);
        image.Mutate(ctx => ctx.Rotate(skewDegrees));
    }

    private static void ApplyColorMode(Image<Rgba32> image, ColorProfile colorProfile)
    {
        if (colorProfile == ColorProfile.Grayscale)
        {
            image.Mutate(ctx => ctx.Grayscale());
        }
        else if (colorProfile == ColorProfile.BlackAndWhite)
        {
            image.Mutate(ctx => ctx.Grayscale().BinaryThreshold(0.5f));
        }
    }

    private static byte ClampByte(int value)
    {
        return (byte)Math.Clamp(value, 0, 255);
    }
}
