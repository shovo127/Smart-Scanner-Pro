namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the rotation applied to an image.
/// </summary>
public enum Rotation
{
    /// <summary>
    /// No rotation.
    /// </summary>
    None = 0,

    /// <summary>
    /// Rotated 90 degrees clockwise.
    /// </summary>
    Clockwise90 = 90,

    /// <summary>
    /// Rotated 180 degrees.
    /// </summary>
    Clockwise180 = 180,

    /// <summary>
    /// Rotated 270 degrees clockwise.
    /// </summary>
    Clockwise270 = 270,
}
