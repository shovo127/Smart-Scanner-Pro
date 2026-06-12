namespace SmartScannerPro.Domain.Abstractions;

using System.Collections.Generic;

/// <summary>
/// Represents the root of an aggregate.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    protected AggregateRoot(TId id)
        : base(id)
    {
    }

    /// <summary>
    /// Gets the read-only collection of domain events.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the aggregate.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent != null)
        {
            this.domainEvents.Add(domainEvent);
        }
    }

    /// <summary>
    /// Removes a domain event from the aggregate.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent != null)
        {
            this.domainEvents.Remove(domainEvent);
        }
    }

    /// <summary>
    /// Clears all domain events from the aggregate.
    /// </summary>
    public void ClearDomainEvents()
    {
        this.domainEvents.Clear();
    }
}
