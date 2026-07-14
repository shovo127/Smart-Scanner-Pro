namespace SmartScannerPro.UI;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.Application.ScanWorkflow.Notifications;
using SmartScannerPro.UI.Services;
using SmartScannerPro.UI.Theme;
using SmartScannerPro.UI.ViewModels;

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
        services.AddSingleton<WorkspaceViewModel>();

        // Register WorkspaceViewModel as MediatR notification handlers
        // so it receives real-time page and progress updates from the workflow engine.
        services.AddSingleton<INotificationHandler<PageScannedNotification>>(
            sp => sp.GetRequiredService<WorkspaceViewModel>());
        services.AddSingleton<INotificationHandler<WorkflowProgressNotification>>(
            sp => sp.GetRequiredService<WorkspaceViewModel>());

        return services;
    }
}
