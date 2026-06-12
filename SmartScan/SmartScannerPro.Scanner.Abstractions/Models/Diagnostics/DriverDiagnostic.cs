namespace SmartScannerPro.Scanner.Abstractions.Models.Diagnostics;

using System.Collections.Generic;

/// <summary>
/// Contains low-level diagnostic information specific to a driver.
/// </summary>
public record DriverDiagnostic
{
    /// <summary>
    /// Gets the unique identifier of the driver.
    /// </summary>
    public required string DriverId { get; init; }

    /// <summary>
    /// Gets a bag of diagnostic properties exposed by the driver.
    /// </summary>
    public IDictionary<string, string> Properties { get; init; } = new Dictionary<string, string>();
}
