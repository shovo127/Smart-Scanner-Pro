namespace SmartScannerPro.Domain.Entities;

using System;
using System.Collections.Generic;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Domain.Enums;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a physical or simulated scanner device.
/// </summary>
public class ScannerDevice
{
    private readonly List<ScannerCapability> capabilities = new List<ScannerCapability>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerDevice"/> class.
    /// </summary>
    /// <param name="id">The scanner ID.</param>
    /// <param name="name">The scanner name.</param>
    /// <param name="driver">The driver type.</param>
    public ScannerDevice(ScannerId id, string name, DriverType driver)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));

        this.Id = id;
        this.Name = name;
        this.Driver = driver;
        this.Status = HealthStatus.Healthy;
    }

    /// <summary>
    /// Gets the unique identifier for the scanner.
    /// </summary>
    public ScannerId Id { get; }

    /// <summary>
    /// Gets the display name of the scanner.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the driver type used to communicate with the scanner.
    /// </summary>
    public DriverType Driver { get; }

    /// <summary>
    /// Gets the health status of the scanner.
    /// </summary>
    public HealthStatus Status { get; private set; }

    /// <summary>
    /// Gets a read-only collection of the scanner's capabilities.
    /// </summary>
    public IReadOnlyCollection<ScannerCapability> Capabilities => this.capabilities.AsReadOnly();

    /// <summary>
    /// Updates the name of the scanner.
    /// </summary>
    /// <param name="newName">The new name.</param>
    public void UpdateName(string newName)
    {
        Guard.NotNullOrWhiteSpace(newName, nameof(newName));
        this.Name = newName;
    }

    /// <summary>
    /// Updates the health status of the scanner.
    /// </summary>
    /// <param name="status">The new status.</param>
    public void UpdateStatus(HealthStatus status)
    {
        this.Status = status;
    }

    /// <summary>
    /// Adds a capability to the scanner.
    /// </summary>
    /// <param name="capability">The capability to add.</param>
    public void AddCapability(ScannerCapability capability)
    {
        Guard.NotNull(capability, nameof(capability));
        this.capabilities.Add(capability);
    }
}
