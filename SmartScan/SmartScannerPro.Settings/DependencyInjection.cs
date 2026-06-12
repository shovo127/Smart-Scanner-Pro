namespace SmartScannerPro.Settings;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.Interfaces.Configuration;
using SmartScannerPro.Settings.Store;

/// <summary>
/// Provides extension methods for registering settings services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds settings layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with settings services registered.</returns>
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<SettingsStore>();
        services.AddSingleton<ISettingsService, SettingsManager>();

        return services;
    }
}
