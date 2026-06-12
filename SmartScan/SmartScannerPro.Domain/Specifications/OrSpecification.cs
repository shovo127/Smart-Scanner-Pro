namespace SmartScannerPro.Domain.Specifications;

using System;
using System.Linq.Expressions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a specification combined with an OR logical operator.
/// </summary>
/// <typeparam name="T">The type of object being evaluated.</typeparam>
public class OrSpecification<T> : CompositeSpecification<T>
{
    private readonly ISpecification<T> left;
    private readonly ISpecification<T> right;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Guard.NotNull(left, nameof(left));
        Guard.NotNull(right, nameof(right));

        this.left = left;
        this.right = right;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            Expression<Func<T, bool>> leftExpression = this.left.Criteria;
            Expression<Func<T, bool>> rightExpression = this.right.Criteria;

            ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            BinaryExpression exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);

            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);

            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }
}
