namespace SmartScannerPro.Application.Abstractions;

using System;

/// <summary>
/// Represents the execution context for the current user, storing preferences and tenant boundaries.
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// Gets the current user.
    /// </summary>
    ICurrentUser CurrentUser { get; }

    /// <summary>
    /// Gets the correlation identifier for the current request context.
    /// </summary>
    Guid CorrelationId { get; }
}
