namespace SmartScannerPro.Scanner;

using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Diagnostics;
using SmartScannerPro.Scanner.Discovery;
using SmartScannerPro.Scanner.Engine;
using SmartScannerPro.Scanner.Factory;

/// <summary>
/// Provides extension methods for registering the unified scanner orchestration layer
/// with the dependency injection container.
/// </summary>
/// <remarks>
/// This method registers only the provider-agnostic orchestration services
/// (<see cref="ScannerEngine"/>, <see cref="ScannerFactory"/>, <see cref="ScannerDiscoveryService"/>,
/// etc.). Individual scanner providers (WIA, Mock, TWAIN) must be registered separately in the
/// application's Composition Root by calling their own extension methods
/// (e.g., <c>AddMockScanner()</c>, <c>AddWiaScanner()</c>) <b>before</b> calling
/// <see cref="AddScanner"/>.
/// </remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the scanner orchestration layer to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddScanner(this IServiceCollection services)
    {
        // Remove any individual provider registrations for the five unified SDK interfaces
        // so that the unified orchestrators are the definitive implementations.
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

        // Register the unified orchestrators as concrete singletons
        services.AddSingleton<ScannerEngine>();
        services.AddSingleton<ScannerDiscoveryService>();
        services.AddSingleton<ScannerFactory>();
        services.AddSingleton<ScannerDiagnostics>();
        services.AddSingleton<ScannerHealthMonitor>();

        // Re-bind the SDK interfaces to the unified implementations
        services.AddSingleton<IScannerEngine>(sp => sp.GetRequiredService<ScannerEngine>());
        services.AddSingleton<IScannerDiscoveryService>(sp => sp.GetRequiredService<ScannerDiscoveryService>());
        services.AddSingleton<IScannerFactory>(sp => sp.GetRequiredService<ScannerFactory>());
        services.AddSingleton<IScannerDiagnostics>(sp => sp.GetRequiredService<ScannerDiagnostics>());
        services.AddSingleton<IScannerHealthMonitor>(sp => sp.GetRequiredService<ScannerHealthMonitor>());

        return services;
    }
}
