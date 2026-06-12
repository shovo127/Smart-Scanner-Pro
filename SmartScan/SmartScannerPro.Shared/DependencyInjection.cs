namespace SmartScannerPro.Shared;

using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Provides extension methods for registering shared services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds shared layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with shared services registered.</returns>
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}
