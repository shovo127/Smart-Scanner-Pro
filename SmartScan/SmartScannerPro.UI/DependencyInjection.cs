namespace SmartScannerPro.UI;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.UI.Services;
using SmartScannerPro.UI.Theme;

/// <summary>
/// Provides extension methods for registering UI services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds UI layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with UI services registered.</returns>
    public static IServiceCollection AddUI(this IServiceCollection services)
    {
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IThemeManager, ThemeManager>();
        services.AddSingleton<ViewModels.WorkspaceViewModel>();

        return services;
    }
}
