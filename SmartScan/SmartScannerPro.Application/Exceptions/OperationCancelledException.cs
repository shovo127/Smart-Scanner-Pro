namespace SmartScannerPro.Application.Exceptions;

using System;

/// <summary>
/// Exception thrown when an operation is cancelled by the user or system.
/// </summary>
public class OperationCancelledException : OperationCanceledException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperationCancelledException"/> class.
    /// </summary>
    public OperationCancelledException()
        : base("The operation was cancelled.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationCancelledException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public OperationCancelledException(string message)
        : base(message)
    {
    }
}
