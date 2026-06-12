namespace SmartScannerPro.Domain.Specifications;

using System;
using System.Linq.Expressions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a specification combined with an AND logical operator.
/// </summary>
/// <typeparam name="T">The type of object being evaluated.</typeparam>
public class AndSpecification<T> : CompositeSpecification<T>
{
    private readonly ISpecification<T> left;
    private readonly ISpecification<T> right;

    /// <summary>
    /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
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
            var leftExpression = this.left.Criteria;
            var rightExpression = this.right.Criteria;

            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

            // Replace parameters to match
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);

            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }
}

/// <summary>
/// Utility class to replace expression parameters.
/// </summary>
internal class ParameterReplacer : ExpressionVisitor
{
    /// <summary>
    /// The parameter expression.
    /// </summary>
    private readonly ParameterExpression parameter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterReplacer"/> class.
    /// </summary>
    /// <param name="parameter">The parameter expression.</param>
    internal ParameterReplacer(ParameterExpression parameter)
    {
        this.parameter = parameter;
    }

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return base.VisitParameter(this.parameter);
    }
}
