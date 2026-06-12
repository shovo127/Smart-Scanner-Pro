namespace SmartScannerPro.Domain.Specifications;

using System;
using System.Linq.Expressions;

/// <summary>
/// Provides a contract for the specification pattern to encapsulate business rules.
/// </summary>
/// <typeparam name="T">The type of object being evaluated.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the expression representing the specification criteria.
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Evaluates if the given item satisfies the specification.
    /// </summary>
    /// <param name="item">The item to evaluate.</param>
    /// <returns>True if the item satisfies the specification; otherwise, false.</returns>
    bool IsSatisfiedBy(T item);

    /// <summary>
    /// Combines this specification with another using an AND logical operator.
    /// </summary>
    /// <param name="specification">The other specification.</param>
    /// <returns>A new composite specification.</returns>
    ISpecification<T> And(ISpecification<T> specification);

    /// <summary>
    /// Combines this specification with another using an OR logical operator.
    /// </summary>
    /// <param name="specification">The other specification.</param>
    /// <returns>A new composite specification.</returns>
    ISpecification<T> Or(ISpecification<T> specification);

    /// <summary>
    /// Negates this specification using a NOT logical operator.
    /// </summary>
    /// <returns>A new negated specification.</returns>
    ISpecification<T> Not();
}
