namespace SmartScannerPro.TestAssets;

using System;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Generates synthetic invoice documents.
/// </summary>
public sealed class InvoiceGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public InvoiceGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.Invoice)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        var margin = image.Width / 18;
        var headerHeight = image.Height / 9;
        image.Mutate(ctx =>
        {
            ctx.Fill(Color.FromRgb(30, 80, 160), new RectangularPolygon(0, 0, image.Width, headerHeight));
            ctx.Fill(Color.White, new RectangularPolygon(margin, margin / 2, image.Width / 5, headerHeight - margin));
            DrawRows(ctx, margin, headerHeight + margin, image.Width - (2 * margin), image.Height / 28, 9);
        });
    }
}

/// <summary>
/// Generates synthetic receipt documents.
/// </summary>
public sealed class ReceiptGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiptGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public ReceiptGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.Receipt)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx =>
        {
            var margin = image.Width / 9;
            var rowHeight = image.Height / 45;
            ctx.Fill(Color.Black, new RectangularPolygon(margin, margin, image.Width - (2 * margin), rowHeight));
            DrawRows(ctx, margin, margin * 2, image.Width - (2 * margin), rowHeight / 2, 18);
            ctx.Fill(Color.Black, new RectangularPolygon(margin, image.Height - margin - rowHeight, image.Width - (2 * margin), rowHeight));
        });
    }
}

/// <summary>
/// Generates synthetic contract documents.
/// </summary>
public sealed class ContractGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContractGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public ContractGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.Contract)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx =>
        {
            var margin = image.Width / 12;
            ctx.Fill(Color.Black, new RectangularPolygon(image.Width / 4, margin, image.Width / 2, 8));
            DrawRows(ctx, margin, margin * 2, image.Width - (2 * margin), 5, 31);
            var signatureY = image.Height - (margin * 3);
            ctx.Fill(Color.Gray, new RectangularPolygon(margin, signatureY, image.Width / 3, 2));
            ctx.Fill(Color.Gray, new RectangularPolygon(image.Width - margin - (image.Width / 3), signatureY, image.Width / 3, 2));
        });
    }
}

/// <summary>
/// Generates synthetic passport documents.
/// </summary>
public sealed class PassportGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PassportGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public PassportGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.Passport)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx =>
        {
            var margin = image.Width / 12;
            ctx.Fill(Color.FromRgb(0, 60, 130), new RectangularPolygon(0, 0, image.Width, image.Height / 10));
            ctx.Fill(Color.FromRgb(180, 180, 200), new RectangularPolygon(margin, image.Height / 8, image.Width / 4, image.Height / 3));
            DrawRows(ctx, margin + (image.Width / 3), image.Height / 8, image.Width / 2, 8, 6);
            DrawRows(ctx, margin, image.Height - (image.Height / 8), image.Width - (2 * margin), 9, 2);
        });
    }
}

/// <summary>
/// Generates synthetic QR-code pages.
/// </summary>
public sealed class QRCodeGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QRCodeGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public QRCodeGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.QR)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx =>
        {
            var size = Math.Min(image.Width, image.Height) * 2 / 3;
            var cell = size / 25;
            var x0 = (image.Width - size) / 2;
            var y0 = (image.Height - size) / 2;
            DrawMatrix(ctx, x0, y0, cell);
        });
    }
}

/// <summary>
/// Generates synthetic barcode pages.
/// </summary>
public sealed class BarcodeGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BarcodeGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public BarcodeGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.Barcode)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx =>
        {
            var width = image.Width * 3 / 4;
            var height = image.Height / 5;
            var x = (image.Width - width) / 2;
            var y = (image.Height - height) / 2;
            DrawBarcode(ctx, x, y, width, height);
        });
    }
}

/// <summary>
/// Generates blank synthetic pages.
/// </summary>
public sealed class BlankPageGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlankPageGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public BlankPageGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.BlankPage)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx => ctx.Fill(Color.FromRgb(252, 252, 252)));
    }
}

/// <summary>
/// Generates synthetic book pages.
/// </summary>
public sealed class BookGenerator : ShapeDocumentGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    public BookGenerator(IImageRenderer renderer)
        : base(renderer, DocumentKind.Book)
    {
    }

    /// <inheritdoc/>
    protected override void Draw(Image<Rgba32> image)
    {
        image.Mutate(ctx =>
        {
            ctx.Fill(Color.FromRgb(252, 248, 240));
            DrawRows(ctx, image.Width / 8, image.Height / 10, image.Width * 3 / 4, 7, 38);
        });
    }
}

/// <summary>
/// Provides shared shape-drawing behavior for document generators.
/// </summary>
public abstract class ShapeDocumentGenerator : IDocumentGenerator
{
    private readonly IImageRenderer renderer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShapeDocumentGenerator"/> class.
    /// </summary>
    /// <param name="renderer">The image renderer.</param>
    /// <param name="kind">The generated document kind.</param>
    protected ShapeDocumentGenerator(IImageRenderer renderer, DocumentKind kind)
    {
        this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        this.Kind = kind;
    }

    /// <inheritdoc/>
    public DocumentKind Kind { get; }

    /// <inheritdoc/>
    public Task<RawImage> GenerateAsync(DocumentRenderRequest request, CancellationToken cancellationToken = default)
    {
        return this.renderer.RenderAsync(request, this.Draw, cancellationToken);
    }

    /// <summary>
    /// Draws a QR matrix.
    /// </summary>
    /// <param name="ctx">The drawing context.</param>
    /// <param name="x0">The x origin.</param>
    /// <param name="y0">The y origin.</param>
    /// <param name="cell">The cell size.</param>
    protected static void DrawMatrix(IImageProcessingContext ctx, int x0, int y0, int cell)
    {
        for (var row = 0; row < 25; row++)
        {
            for (var column = 0; column < 25; column++)
            {
                if ((row + column) % 3 == 0 || row < 7 && column < 7 || row > 17 && column < 7 || row < 7 && column > 17)
                {
                    ctx.Fill(Color.Black, new RectangularPolygon(x0 + (column * cell), y0 + (row * cell), cell - 1, cell - 1));
                }
            }
        }
    }

    /// <summary>
    /// Draws a barcode.
    /// </summary>
    /// <param name="ctx">The drawing context.</param>
    /// <param name="x">The x origin.</param>
    /// <param name="y">The y origin.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="height">The barcode height.</param>
    protected static void DrawBarcode(IImageProcessingContext ctx, int x, int y, int width, int height)
    {
        var unit = (float)width / 160;
        var cursor = (float)x;
        for (var i = 0; i < 60; i++)
        {
            var barWidth = i % 5 == 0 ? unit * 3 : unit;
            if (i % 2 == 0)
            {
                ctx.Fill(Color.Black, new RectangularPolygon(cursor, y, barWidth, height));
            }

            cursor += barWidth + unit;
        }
    }

    /// <summary>
    /// Draws deterministic text-like rows.
    /// </summary>
    /// <param name="ctx">The processing context.</param>
    /// <param name="x">The left coordinate.</param>
    /// <param name="y">The top coordinate.</param>
    /// <param name="width">The maximum row width.</param>
    /// <param name="height">The row height.</param>
    /// <param name="count">The row count.</param>
    protected static void DrawRows(IImageProcessingContext ctx, int x, int y, int width, int height, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var rowWidth = i % 7 == 6 ? width * 2 / 3 : width;
            var rowY = y + (i * (height + 14));
            ctx.Fill(Color.FromRgb(70, 70, 70), new RectangularPolygon(x, rowY, rowWidth, height));
        }
    }

    /// <summary>
    /// Draws the generated document into the provided image.
    /// </summary>
    /// <param name="image">The image to draw into.</param>
    protected abstract void Draw(Image<Rgba32> image);
}
