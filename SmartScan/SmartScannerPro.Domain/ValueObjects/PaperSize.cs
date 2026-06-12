namespace SmartScannerPro.Domain.ValueObjects;

using System.Collections.Generic;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the physical dimensions of a document.
/// </summary>
public record PaperSize
{
    /// <summary>
    /// Gets the A4 size.
    /// </summary>
    public static readonly PaperSize A4 = new PaperSize("A4", 210, 297);

    /// <summary>
    /// Gets the Letter size.
    /// </summary>
    public static readonly PaperSize Letter = new PaperSize("Letter", 215.9, 279.4);

    /// <summary>
    /// Gets the Legal size.
    /// </summary>
    public static readonly PaperSize Legal = new PaperSize("Legal", 215.9, 355.6);

    /// <summary>
    /// Initializes a new instance of the <see cref="PaperSize"/> class.
    /// </summary>
    /// <param name="name">The common name.</param>
    /// <param name="widthMm">The width in millimeters.</param>
    /// <param name="heightMm">The height in millimeters.</param>
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
    /// Gets the common name of the paper size (e.g., A4).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the physical width in millimeters.
    /// </summary>
    public double WidthMm { get; }

    /// <summary>
    /// Gets the physical height in millimeters.
    /// </summary>
    public double HeightMm { get; }

    /// <summary>
    /// Returns the string representation of the paper size.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => $"{this.Name} ({this.WidthMm}x{this.HeightMm} mm)";

}
