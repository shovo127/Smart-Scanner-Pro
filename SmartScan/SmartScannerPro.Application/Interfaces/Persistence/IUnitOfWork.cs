namespace SmartScannerPro.Application.Interfaces.Persistence;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents a unit of work for managing transactions and tracking changes.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Saves all made changes within the unit of work to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
