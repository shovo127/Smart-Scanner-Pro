namespace SmartScannerPro.Scanner.WIA;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Represents the low-level driver interface for communicating with a physical WIA scanner.
/// </summary>
public sealed class WiaScannerDriver : IScannerDriver
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WiaScannerDriver"/> class.
    /// </summary>
    /// <param name="hardwareId">The unique hardware identifier of the scanner.</param>
    /// <param name="manufacturer">The manufacturer of the scanner.</param>
    /// <param name="driverVersion">The WIA driver version string.</param>
    public WiaScannerDriver(string hardwareId, string manufacturer, string driverVersion)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException(nameof(hardwareId));
        }

        this.Info = new DriverInfo
        {
            Id = Guid.NewGuid(),
            Name = "WIA Native Driver",
            Type = DriverType.WIA,
            Version = ParseVersion(driverVersion),
            Priority = DriverPriority.Normal,
            Status = DriverStatus.Ready,
            Manufacturer = manufacturer ?? string.Empty,
            NativeProvider = "WIA 2.0",
        };
    }

    /// <inheritdoc/>
    public DriverInfo Info { get; }

    /// <inheritdoc/>
    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    private static DriverVersion ParseVersion(string versionStr)
    {
        var major = 1;
        var minor = 0;
        var build = 0;
        var revision = 0;

        if (Version.TryParse(versionStr, out var v))
        {
            major = v.Major;
            minor = v.Minor;
            build = v.Build >= 0 ? v.Build : 0;
            revision = v.Revision >= 0 ? v.Revision : 0;
        }

        return new DriverVersion
        {
            Major = major,
            Minor = minor,
            Build = build,
            Revision = revision,
            OriginalString = versionStr ?? "1.0.0.0",
        };
    }
}
