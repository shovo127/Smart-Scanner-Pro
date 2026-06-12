namespace SmartScannerPro.Domain.Abstractions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a base implementation for a domain event.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class.
    /// </summary>
    /// <param name="aggregateId">The identifier of the aggregate publishing the event.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="metadata">Optional metadata for the event.</param>
    protected DomainEvent(
        Guid aggregateId,
        Guid correlationId = default,
        IDictionary<string, object>? metadata = null)
    {
        Guard.IsTrue(aggregateId != Guid.Empty, "AggregateId cannot be empty.");

        this.EventId = Guid.NewGuid();
        this.OccurredOnUtc = DateTime.UtcNow;
        this.AggregateId = aggregateId;
        this.CorrelationId = correlationId == default ? Guid.NewGuid() : correlationId;

        Dictionary<string, object> metaDict = metadata != null ? new Dictionary<string, object>(metadata) : new Dictionary<string, object>();
        this.Metadata = new ReadOnlyDictionary<string, object>(metaDict);
    }

    /// <inheritdoc/>
    public Guid EventId { get; init; }

    /// <inheritdoc/>
    public DateTime OccurredOnUtc { get; init; }

    /// <inheritdoc/>
    public Guid CorrelationId { get; init; }

    /// <inheritdoc/>
    public Guid AggregateId { get; init; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, object> Metadata { get; init; }
}
