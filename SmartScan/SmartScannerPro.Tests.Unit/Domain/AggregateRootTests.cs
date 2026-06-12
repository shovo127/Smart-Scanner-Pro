namespace SmartScannerPro.Tests.Unit.Domain;

using System;
using System.Linq;
using SmartScannerPro.Domain.Abstractions;
using Xunit;

/// <summary>
/// Unit tests for the AggregateRoot base class.
/// </summary>
public class AggregateRootTests
{
    /// <summary>
    /// Tests that domain events are correctly added.
    /// </summary>
    [Fact]
    public void AddDomainEvent_AddsEventToList()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.DoSomething();

        Assert.Single(aggregate.DomainEvents);
        Assert.IsType<TestEvent>(aggregate.DomainEvents.First());
    }

    /// <summary>
    /// Tests that domain events are correctly cleared.
    /// </summary>
    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());
        aggregate.DoSomething();

        aggregate.ClearDomainEvents();

        Assert.Empty(aggregate.DomainEvents);
    }
}

/// <summary>
/// A test domain event.
/// </summary>
internal record TestEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestEvent"/> class.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    public TestEvent(Guid aggregateId)
        : base(aggregateId)
    {
    }
}

/// <summary>
/// A test aggregate root.
/// </summary>
internal class TestAggregate : AggregateRoot<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAggregate"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public TestAggregate(Guid id)
        : base(id)
    {
    }

    /// <summary>
    /// Performs an action and adds a domain event.
    /// </summary>
    public void DoSomething()
    {
        this.AddDomainEvent(new TestEvent(this.Id));
    }
}
