namespace SmartScannerPro.TestAssets;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Renders synthetic documents using SixLabors ImageSharp.
/// </summary>
public sealed class ImageSharpRenderer : IImageRenderer
{
    private const double A4WidthMillimeters = 210.0;
    private const double A4HeightMillimeters = 297.0;
    private readonly Random random;

    /// <summary>
    /// Applies the requested color mode conversion.
    /// </summary>
    /// <param name="image">The image to convert.</param>
    /// <param name="colorProfile">The target color profile.</param>
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

    /// <summary>
    /// Clamps an integer to the byte range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <returns>The clamped byte value.</returns>
    private static byte ClampByte(int value)
    {
        return (byte)Math.Clamp(value, 0, 255);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageSharpRenderer"/> class.
    /// </summary>
    public ImageSharpRenderer()
        : this(new Random(42))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageSharpRenderer"/> class.
    /// </summary>
    /// <param name="random">The deterministic random source.</param>
    public ImageSharpRenderer(Random random)
    {
        this.random = random ?? throw new ArgumentNullException(nameof(random));
    }

    /// <inheritdoc/>
    public async Task<RawImage> RenderAsync(
        DocumentRenderRequest request,
        Action<Image<Rgba32>> draw,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(draw);

        var size = ComputeA4Pixels(request.ResolutionDpi);
        using var image = new Image<Rgba32>(size.Width, size.Height, Color.White);
        draw(image);
        this.ApplyEffects(image, request);

        var stream = new MemoryStream();
        await image.SaveAsync(stream, new PngEncoder(), cancellationToken).ConfigureAwait(false);
        stream.Position = 0;

        return new RawImage
        {
            DataStream = stream,
            Metadata = BuildMetadata(size.Width, size.Height, request),
        };
    }

    private static (int Width, int Height) ComputeA4Pixels(int dpi)
    {
        var width = (int)(A4WidthMillimeters / 25.4 * dpi);
        var height = (int)(A4HeightMillimeters / 25.4 * dpi);
        return (width, height);
    }

    private static ImageMetadata BuildMetadata(int width, int height, DocumentRenderRequest request)
    {
        return new ImageMetadata
        {
            WidthInPixels = width,
            HeightInPixels = height,
            ResolutionX = request.ResolutionDpi,
            ResolutionY = request.ResolutionDpi,
            BitDepth = request.ColorProfile == ColorProfile.BlackAndWhite ? 1 : 8,
            ColorProfile = request.ColorProfile,
            Source = ImageSource.AdfFront,
            Compression = Compression.None,
            Format = ImageFormat.Png,
        };
    }

    private void ApplyEffects(Image<Rgba32> image, DocumentRenderRequest request)
    {
        if (request.AddSkew)
        {
            image.Mutate(ctx => ctx.Rotate((float)(this.random.NextDouble() * 2.0 - 1.0)));
        }

        if (request.AddNoise)
        {
            this.ApplyNoise(image);
        }

        ApplyColorMode(image, request.ColorProfile);
    }

    private void ApplyNoise(Image<Rgba32> image)
    {
        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (var x = 0; x < row.Length; x++)
                {
                    ref var pixel = ref row[x];
                    var delta = this.random.Next(-8, 9);
                    pixel.R = ClampByte(pixel.R + delta);
                    pixel.G = ClampByte(pixel.G + delta);
                    pixel.B = ClampByte(pixel.B + delta);
                }
            }
        });
    }
}
