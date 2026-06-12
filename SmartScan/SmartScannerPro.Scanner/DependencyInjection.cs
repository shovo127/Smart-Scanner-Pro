namespace SmartScannerPro.Scanner;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering scanner services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds scanner layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddScanner(this IServiceCollection services)
    {
        return services;
    }
}
