namespace SmartScannerPro.TestAssets;

using System;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Renders generated document drawings into scanner-compatible raw images.
/// </summary>
public interface IImageRenderer
{
    /// <summary>
    /// Renders a document drawing into a raw image.
    /// </summary>
    /// <param name="request">The rendering request.</param>
    /// <param name="draw">The drawing operation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The rendered raw image.</returns>
    Task<RawImage> RenderAsync(
        DocumentRenderRequest request,
        Action<Image<Rgba32>> draw,
        CancellationToken cancellationToken = default);
}
