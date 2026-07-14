namespace SmartScannerPro.Scanner.Mock.Provider;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// A minimal mock driver implementation that carries metadata only.
/// No hardware communication is performed.
/// </summary>
public sealed class MockScannerDriver : IScannerDriver
{
    private readonly string hardwareId;

    /// <summary>
    /// Initializes a new instance of <see cref="MockScannerDriver"/>.
    /// </summary>
    /// <param name="hardwareId">The hardware identifier this driver is bound to.</param>
    /// <param name="manufacturer">The manufacturer name for the device.</param>
    /// <param name="firmwareVersion">The simulated firmware version string.</param>
    public MockScannerDriver(string hardwareId, string manufacturer, string firmwareVersion)
    {
        this.hardwareId = hardwareId ?? throw new ArgumentNullException(nameof(hardwareId));

        this.Info = new DriverInfo
        {
            Id = Guid.NewGuid(),
            Name = $"Mock Driver – {hardwareId}",
            Type = DriverType.Mock,
            Version = ParseVersion(firmwareVersion),
            Priority = DriverPriority.Preferred,
            Status = DriverStatus.Ready,
            Manufacturer = manufacturer ?? string.Empty,
            NativeProvider = "SmartScannerPro.Scanner.Mock",
        };
    }

    /// <inheritdoc/>
    public DriverInfo Info { get; }

    /// <inheritdoc/>
    public Task InitializeAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    /// <inheritdoc/>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private static DriverVersion ParseVersion(string version)
    {
        var parts = (version ?? "1.0.0.0").Split('.');
        return new DriverVersion
        {
            Major = parts.Length > 0 && int.TryParse(parts[0], out var maj) ? maj : 1,
            Minor = parts.Length > 1 && int.TryParse(parts[1], out var min) ? min : 0,
            Build = parts.Length > 2 && int.TryParse(parts[2], out var bld) ? bld : 0,
            Revision = parts.Length > 3 && int.TryParse(parts[3], out var rev) ? rev : 0,
            OriginalString = version ?? "1.0.0.0",
        };
    }
}
