namespace SmartScannerPro.Scanner.Abstractions.Interfaces;

using System;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;

/// <summary>
/// Represents a physical or virtual scanner device.
/// </summary>
public interface IScannerDevice
{
    /// <summary>
    /// Gets the hardware descriptor for this device.
    /// </summary>
    ScannerDescriptor Descriptor { get; }

    /// <summary>
    /// Gets the capabilities exposed by this scanner device.
    /// </summary>
    IScannerCapabilities Capabilities { get; }

    /// <summary>
    /// Gets the configuration interface for this scanner device.
    /// </summary>
    IScannerConfiguration Configuration { get; }

    /// <summary>
    /// Gets the active connection to the device, if any.
    /// </summary>
    IScannerConnection? Connection { get; }
}
