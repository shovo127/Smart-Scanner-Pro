namespace SmartScannerPro.Scanner.Abstractions.Models.Drivers;

using System;

/// <summary>
/// Provides metadata and runtime information about a loaded scanner driver.
/// </summary>
public record DriverInfo
{
    /// <summary>
    /// Gets the unique identifier for this driver instance.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the driver.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the type of the driver.
    /// </summary>
    public DriverType Type { get; init; }

    /// <summary>
    /// Gets the version of the driver.
    /// </summary>
    public required DriverVersion Version { get; init; }

    /// <summary>
    /// Gets the priority of the driver.
    /// </summary>
    public DriverPriority Priority { get; init; }

    /// <summary>
    /// Gets the current status of the driver.
    /// </summary>
    public DriverStatus Status { get; init; }

    /// <summary>
    /// Gets the manufacturer of the driver.
    /// </summary>
    public string Manufacturer { get; init; } = string.Empty;

    /// <summary>
    /// Gets the underlying native implementation reference (if any).
    /// </summary>
    public string NativeProvider { get; init; } = string.Empty;
}
