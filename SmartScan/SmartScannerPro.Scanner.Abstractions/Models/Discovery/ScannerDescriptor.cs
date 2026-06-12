namespace SmartScannerPro.Scanner.Abstractions.Models.Discovery;

using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Describes a scanner discovered on the system or network.
/// </summary>
public record ScannerDescriptor
{
    /// <summary>
    /// Gets the unique identifier for the scanner hardware.
    /// </summary>
    public required string HardwareId { get; init; }

    /// <summary>
    /// Gets the display name of the scanner.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the manufacturer of the scanner.
    /// </summary>
    public string Manufacturer { get; init; } = string.Empty;

    /// <summary>
    /// Gets the connection type of the scanner.
    /// </summary>
    public ConnectionType ConnectionType { get; init; }

    /// <summary>
    /// Gets the driver information associated with this discovered scanner.
    /// </summary>
    public required DriverInfo Driver { get; init; }

    /// <summary>
    /// Gets the connection string or path required to communicate with the scanner.
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;
}
