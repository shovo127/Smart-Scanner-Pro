namespace SmartScannerPro.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.Infrastructure.FileSystem;

/// <summary>
/// Provides extension methods for registering infrastructure services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with infrastructure services registered.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDirectoryManager, DirectoryManager>();

        return services;
    }
}
