namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the orientation of a page.
/// </summary>
public readonly record struct PaperOrientation
{
    /// <summary>
    /// Gets the orientation name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaperOrientation"/> struct.
    /// </summary>
    /// <param name="name">The orientation name.</param>
    private PaperOrientation(string name)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        this.Name = name;
    }

    /// <summary>
    /// Predefined Portrait orientation.
    /// </summary>
    public static readonly PaperOrientation Portrait = new PaperOrientation("Portrait");

    /// <summary>
    /// Predefined Landscape orientation.
    /// </summary>
    public static readonly PaperOrientation Landscape = new PaperOrientation("Landscape");

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Name;
}
