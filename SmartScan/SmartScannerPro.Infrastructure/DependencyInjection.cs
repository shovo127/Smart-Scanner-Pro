using Microsoft.Extensions.DependencyInjection;

namespace SmartScannerPro.Infrastructure;

/// <summary>
/// Extension methods for registering Infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure layer services to the DI container.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register generic logging, file system managers, etc.
        return services;
    }
}
