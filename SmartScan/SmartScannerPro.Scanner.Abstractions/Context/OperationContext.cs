namespace SmartScannerPro.Scanner.Abstractions.Context;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Logging;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Provides the execution context for scanner operations. This object flows through the entire scanner pipeline.
/// </summary>
public class OperationContext
{
    private readonly ConcurrentDictionary<string, object> properties = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationContext"/> class.
    /// </summary>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="logger">The logger abstraction.</param>
    public OperationContext(Guid correlationId, CancellationToken cancellationToken, ILogger logger)
    {
        this.CorrelationId = correlationId;
        this.CancellationToken = cancellationToken;
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Timestamp = DateTimeOffset.UtcNow;
        this.Culture = CultureInfo.CurrentCulture;
    }

    /// <summary>
    /// Gets the unique correlation identifier for the operation.
    /// </summary>
    public Guid CorrelationId { get; }

    /// <summary>
    /// Gets the cancellation token for the operation.
    /// </summary>
    public CancellationToken CancellationToken { get; }

    /// <summary>
    /// Gets or sets the culture to use for the operation.
    /// </summary>
    public CultureInfo Culture { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the current profile, if applicable.
    /// </summary>
    public ProfileId? CurrentProfile { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the current session, if applicable.
    /// </summary>
    public SessionId? CurrentSession { get; set; }

    /// <summary>
    /// Gets or sets the progress reporter for the operation.
    /// </summary>
    public IProgress<Models.Sessions.ScanProgress>? Progress { get; set; }

    /// <summary>
    /// Gets the time the operation started.
    /// </summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// Gets the logger instance to record operation details.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Gets the dynamic property bag for sharing state across the pipeline.
    /// </summary>
    public IDictionary<string, object> Properties => this.properties;
}
