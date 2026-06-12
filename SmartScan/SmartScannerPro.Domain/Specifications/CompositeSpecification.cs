namespace SmartScannerPro.Domain.Specifications;

using System;
using System.Linq.Expressions;

/// <summary>
/// Represents a base class for composite specifications.
/// </summary>
/// <typeparam name="T">The type of object being evaluated.</typeparam>
public abstract class CompositeSpecification<T> : ISpecification<T>
{
    /// <inheritdoc/>
    public abstract Expression<Func<T, bool>> Criteria { get; }

    /// <inheritdoc/>
    public bool IsSatisfiedBy(T item)
    {
        Func<T, bool> predicate = this.Criteria.Compile();
        return predicate(item);
    }

    /// <inheritdoc/>
    public ISpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    /// <inheritdoc/>
    public ISpecification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    /// <inheritdoc/>
    public ISpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}
