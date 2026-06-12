namespace SmartScannerPro.Scanner.Abstractions.Models.Discovery;

using System;
using System.Collections.Generic;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Represents a request to discover scanners.
/// </summary>
public record DiscoveryRequest
{
    /// <summary>
    /// Gets the allowed connection types. If empty, all connection types are allowed.
    /// </summary>
    public IReadOnlyList<ConnectionType>? AllowedConnectionTypes { get; init; }

    /// <summary>
    /// Gets the allowed driver types. If empty, all driver types are allowed.
    /// </summary>
    public IReadOnlyList<DriverType>? AllowedDriverTypes { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include offline devices.
    /// </summary>
    public bool IncludeOffline { get; init; }

    /// <summary>
    /// Gets the maximum time to wait for network scanners to respond.
    /// </summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(10);
}
