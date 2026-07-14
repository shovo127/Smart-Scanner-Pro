namespace SmartScannerPro.Application.ScanWorkflow.Services;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

/// <summary>
/// Generates downscaled JPEG thumbnail images from full-resolution scanned PNG files.
/// </summary>
public sealed class WorkflowThumbnailService
{
    /// <summary>
    /// The maximum width of a generated thumbnail in pixels.
    /// </summary>
    public const int ThumbnailMaxWidth = 200;

    /// <summary>
    /// The JPEG quality level for thumbnails (0–100).
    /// </summary>
    public const int ThumbnailJpegQuality = 85;

    private readonly ILogger<WorkflowThumbnailService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowThumbnailService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public WorkflowThumbnailService(ILogger<WorkflowThumbnailService> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generates a JPEG thumbnail from the specified source image file.
    /// </summary>
    /// <param name="sourcePath">The absolute path to the full-resolution source image (PNG).</param>
    /// <param name="outputPath">
    /// The absolute path where the generated thumbnail JPEG will be written.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when <paramref name="sourcePath"/> does not exist.
    /// </exception>
    public async Task GenerateAsync(
        string sourcePath,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            throw new ArgumentNullException(nameof(sourcePath));
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentNullException(nameof(outputPath));
        }

        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException($"Source image file not found: '{sourcePath}'", sourcePath);
        }

        this.logger.LogDebug("Generating thumbnail: {Source} -> {Output}", sourcePath, outputPath);

        try
        {
            using var image = await Image.LoadAsync(sourcePath, cancellationToken).ConfigureAwait(false);

            // Compute target size preserving aspect ratio
            var (targetWidth, targetHeight) = ComputeSize(image.Width, image.Height, ThumbnailMaxWidth);

            image.Mutate(ctx => ctx.Resize(targetWidth, targetHeight));

            var encoder = new JpegEncoder { Quality = ThumbnailJpegQuality };
            await image.SaveAsJpegAsync(outputPath, encoder, cancellationToken).ConfigureAwait(false);

            this.logger.LogDebug("Thumbnail generated successfully: {Output}", outputPath);
        }
        catch (Exception ex) when (ex is not OperationCanceledException && ex is not FileNotFoundException)
        {
            this.logger.LogError(ex, "Failed to generate thumbnail for source: {Source}", sourcePath);
            throw;
        }
    }

    private static (int Width, int Height) ComputeSize(int originalWidth, int originalHeight, int maxWidth)
    {
        if (originalWidth <= maxWidth)
        {
            return (originalWidth, originalHeight);
        }

        var ratio = (double)maxWidth / originalWidth;
        return (maxWidth, (int)(originalHeight * ratio));
    }
}
