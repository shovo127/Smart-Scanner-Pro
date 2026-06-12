namespace SmartScannerPro.Application;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register CQRS handlers, Validators, and Application Services here
        return services;
    }
}
