namespace SmartScannerPro.Domain.Abstractions;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a base class for domain entities.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    protected Entity(TId id)
    {
        if (id == null || id.Equals(default(TId)))
        {
            throw new ArgumentException("Entity ID cannot be default.", nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// Checks if two entities are equal based on their IDs.
    /// </summary>
    /// <param name="left">The left entity.</param>
    /// <param name="right">The right entity.</param>
    /// <returns>True if they are equal; otherwise, false.</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two entities are not equal.
    /// </summary>
    /// <param name="left">The left entity.</param>
    /// <param name="right">The right entity.</param>
    /// <returns>True if they are not equal; otherwise, false.</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        if (obj is not Entity<TId> entity)
        {
            return false;
        }

        return this.Id.Equals(entity.Id);
    }

    /// <inheritdoc/>
    public bool Equals(Entity<TId>? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.GetType() != this.GetType())
        {
            return false;
        }

        return this.Id.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Id.GetHashCode() * 41;
    }
}
