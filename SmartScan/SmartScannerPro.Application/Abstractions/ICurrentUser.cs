namespace SmartScannerPro.Application.Abstractions;

using System;

/// <summary>
/// Provides access to the currently authenticated user's information.
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Gets the name or username of the current user.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Checks if the current user has the specified permission.
    /// </summary>
    /// <param name="permission">The permission name.</param>
    /// <returns>True if the user has the permission; otherwise, false.</returns>
    bool HasPermission(string permission);
}
