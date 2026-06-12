namespace SmartScannerPro.Tests.Unit.Domain;

using System;
using SmartScannerPro.Domain.Abstractions;
using Xunit;

/// <summary>
/// Unit tests for the StronglyTypedId base class.
/// </summary>
public class StronglyTypedIdTests
{
    /// <summary>
    /// Tests that IDs with the same value are equal.
    /// </summary>
    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var value = Guid.NewGuid();
        var id1 = new TestId(value);
        var id2 = new TestId(value);

        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
        Assert.False(id1 != id2);
    }

    /// <summary>
    /// Tests that IDs with different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var id1 = new TestId(Guid.NewGuid());
        var id2 = new TestId(Guid.NewGuid());

        Assert.NotEqual(id1, id2);
        Assert.False(id1 == id2);
        Assert.True(id1 != id2);
    }

    /// <summary>
    /// Tests that empty GUID throws.
    /// </summary>
    [Fact]
    public void Constructor_EmptyGuid_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new TestId(Guid.Empty));
    }
}

/// <summary>
/// A test identifier.
/// </summary>
internal class TestId : StronglyTypedId<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestId"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public TestId(Guid value)
        : base(value)
    {
    }
}
