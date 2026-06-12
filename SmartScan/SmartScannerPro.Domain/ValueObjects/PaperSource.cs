namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents the physical source of the paper in the scanner.
/// </summary>
public readonly record struct PaperSource
{
    /// <summary>
    /// Gets the source name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaperSource"/> struct.
    /// </summary>
    /// <param name="name">The source name.</param>
    private PaperSource(string name)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        this.Name = name;
    }

    /// <summary>
    /// Predefined Flatbed source.
    /// </summary>
    public static readonly PaperSource Flatbed = new PaperSource("Flatbed");

    /// <summary>
    /// Predefined Automatic Document Feeder (ADF) source.
    /// </summary>
    public static readonly PaperSource AutomaticDocumentFeeder = new PaperSource("ADF");

    /// <summary>
    /// Predefined Duplex Automatic Document Feeder source.
    /// </summary>
    public static readonly PaperSource Duplex = new PaperSource("Duplex");

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Name;
}
