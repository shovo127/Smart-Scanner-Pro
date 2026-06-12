namespace SmartScannerPro.Scanner.Drivers;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering scanner driver services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds scanner driver layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddScannerDrivers(this IServiceCollection services)
    {
        return services;
    }
}
