namespace SmartScannerPro.Shared.Utilities;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides utility methods for argument validation.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures that the specified argument is not null.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="argumentValue">The value of the argument.</param>
    /// <param name="argumentName">The name of the argument.</param>
    /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
    public static void NotNull<T>([NotNull] T? argumentValue, [CallerArgumentExpression("argumentValue")] string argumentName = "")
    {
        if (argumentValue is null)
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    /// <summary>
    /// Ensures that the specified string is not null or whitespace.
    /// </summary>
    /// <param name="argumentValue">The string value.</param>
    /// <param name="argumentName">The name of the argument.</param>
    /// <exception cref="ArgumentException">Thrown when the string is null, empty, or consists only of white-space characters.</exception>
    public static void NotNullOrWhiteSpace([NotNull] string? argumentValue, [CallerArgumentExpression("argumentValue")] string argumentName = "")
    {
        if (string.IsNullOrWhiteSpace(argumentValue))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", argumentName);
        }
    }

    /// <summary>
    /// Ensures that the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="message">The exception message if the condition is false.</param>
    /// <exception cref="InvalidOperationException">Thrown when the condition is false.</exception>
    public static void IsTrue(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
