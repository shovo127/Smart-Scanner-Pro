using Microsoft.Extensions.DependencyInjection;

namespace SmartScannerPro.Application;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the DI container.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register CQRS handlers, Validators, and Application Services here
        return services;
    }
}
