namespace SmartScannerPro.Scanner.Abstractions.Events;

using System;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;

/// <summary>
/// Event triggered when a scanner is successfully connected and initialized.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="timestamp">The time the connection occurred.</param>
public record ScannerConnected(string hardwareId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when a scanner is disconnected or becomes offline.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="timestamp">The time the disconnection occurred.</param>
public record ScannerDisconnected(string hardwareId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when a scan session or job begins.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="timestamp">The time the scan started.</param>
public record ScanStarted(string hardwareId, SessionId sessionId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when a scan session or job completes successfully.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="timestamp">The time the scan completed.</param>
public record ScanCompleted(string hardwareId, SessionId sessionId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when a scan session or job is cancelled.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="timestamp">The time the scan was cancelled.</param>
public record ScanCancelled(string hardwareId, SessionId sessionId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when a paper jam is detected in the document feeder.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="timestamp">The time the jam was detected.</param>
public record PaperJamDetected(string hardwareId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when the document feeder runs out of paper.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="timestamp">The time the ADF became empty.</param>
public record OutOfPaper(string hardwareId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when the scanner cover is opened during an operation.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="timestamp">The time the cover was opened.</param>
public record CoverOpened(string hardwareId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when the active driver for a scanner changes.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="newDriverId">The identifier of the new driver.</param>
/// <param name="timestamp">The time the driver was changed.</param>
public record DriverChanged(string hardwareId, Guid newDriverId, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when the health status of a driver or scanner changes.
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="newHealth">The new health state.</param>
/// <param name="timestamp">The time the health changed.</param>
public record HealthChanged(string hardwareId, DriverHealth newHealth, DateTimeOffset timestamp);

/// <summary>
/// Event triggered when the capabilities of a scanner change (e.g., due to a hardware attachment like ADF).
/// </summary>
/// <param name="hardwareId">The hardware identifier of the scanner.</param>
/// <param name="timestamp">The time the capabilities changed.</param>
public record CapabilityChanged(string hardwareId, DateTimeOffset timestamp);
