namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the physical dimensions of a paper.
/// </summary>
public readonly record struct PaperSize
{
    /// <summary>
    /// Gets the name of the paper size (e.g., A4, Letter).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the width in millimeters.
    /// </summary>
    public double WidthMm { get; }

    /// <summary>
    /// Gets the height in millimeters.
    /// </summary>
    public double HeightMm { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaperSize"/> struct.
    /// </summary>
    /// <param name="name">The name of the size.</param>
    /// <param name="widthMm">The width.</param>
    /// <param name="heightMm">The height.</param>
    public PaperSize(string name, double widthMm, double heightMm)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        Guard.IsTrue(widthMm > 0, "Width must be greater than zero.");
        Guard.IsTrue(heightMm > 0, "Height must be greater than zero.");

        this.Name = name;
        this.WidthMm = widthMm;
        this.HeightMm = heightMm;
    }

    /// <summary>
    /// Predefined A4 size.
    /// </summary>
    public static readonly PaperSize A4 = new PaperSize("A4", 210, 297);

    /// <summary>
    /// Predefined Letter size.
    /// </summary>
    public static readonly PaperSize Letter = new PaperSize("Letter", 215.9, 279.4);

    /// <summary>
    /// Predefined Legal size.
    /// </summary>
    public static readonly PaperSize Legal = new PaperSize("Legal", 215.9, 355.6);

    /// <summary>
    /// Predefined A5 size.
    /// </summary>
    public static readonly PaperSize A5 = new PaperSize("A5", 148, 210);

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => $"{this.Name} ({this.WidthMm}x{this.HeightMm} mm)";
}
