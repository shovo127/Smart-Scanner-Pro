using Microsoft.Extensions.DependencyInjection;

namespace SmartScannerPro.Settings;

/// <summary>
/// Extension methods for registering Settings layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Settings layer services to the DI container.
    /// </summary>
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        return services;
    }
}
