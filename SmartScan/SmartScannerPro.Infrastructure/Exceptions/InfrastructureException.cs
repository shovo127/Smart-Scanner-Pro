namespace SmartScannerPro.Infrastructure.Exceptions;

/// <summary>
/// The base exception type for infrastructure layer exceptions.
/// </summary>
public class InfrastructureException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InfrastructureException"/> class.
    /// </summary>
    public InfrastructureException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InfrastructureException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InfrastructureException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InfrastructureException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InfrastructureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
