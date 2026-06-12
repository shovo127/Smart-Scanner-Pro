namespace SmartScannerPro.TestAssets;

using SmartScannerPro.Scanner.Abstractions.Models.Images;

/// <summary>
/// Defines how a synthetic document should be rendered.
/// </summary>
public sealed record DocumentRenderRequest
{
    /// <summary>
    /// Gets the target resolution in dots per inch.
    /// </summary>
    public int ResolutionDpi { get; init; } = 300;

    /// <summary>
    /// Gets the target color profile.
    /// </summary>
    public ColorProfile ColorProfile { get; init; } = ColorProfile.Color;

    /// <summary>
    /// Gets a value indicating whether scanner noise should be applied.
    /// </summary>
    public bool AddNoise { get; init; }

    /// <summary>
    /// Gets a value indicating whether slight page skew should be applied.
    /// </summary>
    public bool AddSkew { get; init; }
}
