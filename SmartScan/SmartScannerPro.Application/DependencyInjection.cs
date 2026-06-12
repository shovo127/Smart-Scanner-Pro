namespace SmartScannerPro.Application;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering application layer services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with application services registered.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
