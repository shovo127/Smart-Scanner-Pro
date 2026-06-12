namespace SmartScannerPro.Application.Exceptions;

using System;

/// <summary>
/// Exception thrown when an operation is forbidden due to authorization constraints.
/// </summary>
public class AuthorizationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    public AuthorizationException()
        : base("Access denied to the requested resource or operation.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AuthorizationException(string message)
        : base(message)
    {
    }
}
