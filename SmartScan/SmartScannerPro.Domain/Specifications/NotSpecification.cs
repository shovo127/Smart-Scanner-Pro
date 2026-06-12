namespace SmartScannerPro.Domain.Specifications;

using System;
using System.Linq.Expressions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a specification that negates another specification.
/// </summary>
/// <typeparam name="T">The type of object being evaluated.</typeparam>
public class NotSpecification<T> : CompositeSpecification<T>
{
    private readonly ISpecification<T> specification;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class.
    /// </summary>
    /// <param name="specification">The specification to negate.</param>
    public NotSpecification(ISpecification<T> specification)
    {
        Guard.NotNull(specification, nameof(specification));
        this.specification = specification;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            Expression<Func<T, bool>> expression = this.specification.Criteria;
            ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            UnaryExpression exprBody = Expression.Not(expression.Body);

            exprBody = (UnaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);

            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }
}
