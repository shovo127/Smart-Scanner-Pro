namespace SmartScannerPro.Domain.Abstractions;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a domain event that occurred within the system.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier for this specific event instance.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets the date and time when the event occurred in UTC.
    /// </summary>
    DateTime OccurredOnUtc { get; }

    /// <summary>
    /// Gets the identifier used to correlate multiple events related to the same operation.
    /// </summary>
    Guid CorrelationId { get; }

    /// <summary>
    /// Gets the identifier of the aggregate that published this event.
    /// </summary>
    Guid AggregateId { get; }

    /// <summary>
    /// Gets a collection of key-value pairs representing additional metadata for the event.
    /// </summary>
    IReadOnlyDictionary<string, object> Metadata { get; }
}
