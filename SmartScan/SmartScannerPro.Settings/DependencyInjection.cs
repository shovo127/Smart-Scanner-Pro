namespace SmartScannerPro.Settings;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering Settings layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Settings layer services to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        return services;
    }
}
