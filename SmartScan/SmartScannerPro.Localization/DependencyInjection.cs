namespace SmartScannerPro.Localization;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.Localization.Core;

/// <summary>
/// Provides extension methods for registering localization services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds localization layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with localization services registered.</returns>
    public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizationService, LanguageManager>();

        return services;
    }
}
