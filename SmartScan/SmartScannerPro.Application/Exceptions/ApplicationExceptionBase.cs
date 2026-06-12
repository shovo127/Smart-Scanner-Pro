namespace SmartScannerPro.Application.Exceptions;

/// <summary>
/// The base exception type for application layer exceptions.
/// </summary>
public abstract class ApplicationExceptionBase : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionBase"/> class.
    /// </summary>
    protected ApplicationExceptionBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionBase"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected ApplicationExceptionBase(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionBase"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    protected ApplicationExceptionBase(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
