namespace SmartScannerPro.Application.Exceptions;

using System;

/// <summary>
/// Exception thrown when an operation conflicts with the current state of the application.
/// </summary>
public class ConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    public ConflictException()
        : base("A conflict occurred during the operation.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ConflictException(string message)
        : base(message)
    {
    }
}
