namespace SmartScannerPro.Application.Pipelines.Behaviors;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartScannerPro.Application.Interfaces.Persistence;

/// <summary>
/// Pipeline behavior for wrapping commands in a transaction (Unit of Work).
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public TransactionBehavior(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // For CQRS, we might only want to save changes for Commands, not Queries.
        // Assuming commands end with "Command".
        var response = await next();

        if (typeof(TRequest).Name.EndsWith("Command"))
        {
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return response;
    }
}
