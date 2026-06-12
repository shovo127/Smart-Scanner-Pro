namespace SmartScannerPro.Tests.Unit.Domain;

using System;
using SmartScannerPro.Domain.Abstractions;
using Xunit;

/// <summary>
/// Unit tests for the Entity base class.
/// </summary>
public class EntityTests
{
    /// <summary>
    /// Tests that entities with the same ID are considered equal.
    /// </summary>
    [Fact]
    public void Equals_SameId_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        Assert.True(entity1.Equals(entity2));
        Assert.True(entity1 == entity2);
        Assert.False(entity1 != entity2);
    }

    /// <summary>
    /// Tests that entities with different IDs are not considered equal.
    /// </summary>
    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        Assert.False(entity1.Equals(entity2));
        Assert.False(entity1 == entity2);
        Assert.True(entity1 != entity2);
    }

    /// <summary>
    /// Tests that empty ID throws ArgumentException.
    /// </summary>
    [Fact]
    public void Constructor_EmptyId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new TestEntity(Guid.Empty));
    }
}

/// <summary>
/// A test entity.
/// </summary>
internal class TestEntity : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestEntity"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public TestEntity(Guid id)
        : base(id)
    {
    }
}
