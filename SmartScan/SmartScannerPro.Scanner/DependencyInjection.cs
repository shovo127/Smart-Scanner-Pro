namespace SmartScannerPro.Scanner;

using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Diagnostics;
using SmartScannerPro.Scanner.Discovery;
using SmartScannerPro.Scanner.Engine;
using SmartScannerPro.Scanner.Factory;
using SmartScannerPro.Scanner.Mock.Extensions;
using SmartScannerPro.Scanner.WIA.Extensions;

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
        // 1. Add mock scanner provider concrete registrations
        services.AddMockScanner();

        // 2. Add WIA scanner provider concrete registrations
        services.AddWiaScanner();

        // 3. Remove individual registrations for SDK interfaces to ensure the unified implementations are the only ones resolved
        for (int i = services.Count - 1; i >= 0; i--)
        {
            var serviceType = services[i].ServiceType;
            if (serviceType == typeof(IScannerEngine) ||
                serviceType == typeof(IScannerDiscoveryService) ||
                serviceType == typeof(IScannerFactory) ||
                serviceType == typeof(IScannerDiagnostics) ||
                serviceType == typeof(IScannerHealthMonitor))
            {
                services.RemoveAt(i);
            }
        }

        // 4. Register our unified SDK implementations
        services.AddSingleton<ScannerEngine>();
        services.AddSingleton<ScannerDiscoveryService>();
        services.AddSingleton<ScannerFactory>();
        services.AddSingleton<ScannerDiagnostics>();
        services.AddSingleton<ScannerHealthMonitor>();

        // Re-bind high-level interfaces to resolve to the unified orchestrators
        services.AddSingleton<IScannerEngine>(sp => sp.GetRequiredService<ScannerEngine>());
        services.AddSingleton<IScannerDiscoveryService>(sp => sp.GetRequiredService<ScannerDiscoveryService>());
        services.AddSingleton<IScannerFactory>(sp => sp.GetRequiredService<ScannerFactory>());
        services.AddSingleton<IScannerDiagnostics>(sp => sp.GetRequiredService<ScannerDiagnostics>());
        services.AddSingleton<IScannerHealthMonitor>(sp => sp.GetRequiredService<ScannerHealthMonitor>());

        return services;
    }
}
