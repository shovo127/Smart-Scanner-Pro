namespace SmartScannerPro.Domain.Abstractions;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents a dispatcher capable of publishing domain events.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches a single domain event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event to dispatch.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches multiple domain events asynchronously.
    /// </summary>
    /// <param name="domainEvents">An array of domain events to dispatch.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchAsync(IDomainEvent[] domainEvents, CancellationToken cancellationToken = default);
}
