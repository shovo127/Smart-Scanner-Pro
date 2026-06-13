namespace SmartScannerPro.Scanner.WIA.Extensions;

using System;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.WIA.Diagnostics;
using SmartScannerPro.Scanner.WIA.Discovery;
using SmartScannerPro.Scanner.WIA.Engine;
using SmartScannerPro.Scanner.WIA.Factory;

/// <summary>
/// Provides extension methods for registering WIA scanner services with the Dependency Injection container.
/// </summary>
public static class WiaScannerServiceCollectionExtensions
{
    /// <summary>
    /// Registers all WIA Scanner Provider services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register into.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for method chaining.</returns>
    public static IServiceCollection AddWiaScanner(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // Core infrastructure
        services.AddSingleton<WiaScannerDiscoveryService>();
        services.AddSingleton<WiaScannerProvider>();

        // Session lifecycle
        services.AddSingleton<WiaScannerFactory>();

        // Diagnostics
        services.AddSingleton<WiaScannerDiagnostics>();
        services.AddSingleton<WiaScannerHealthMonitor>();

        // Engine
        services.AddSingleton<WiaScannerEngine>();

        // Register SDK interfaces
        services.AddSingleton<IScannerEngine>(sp => sp.GetRequiredService<WiaScannerEngine>());
        services.AddSingleton<IScannerFactory>(sp => sp.GetRequiredService<WiaScannerFactory>());
        services.AddSingleton<IScannerDiscoveryService>(sp => sp.GetRequiredService<WiaScannerDiscoveryService>());
        services.AddSingleton<IScannerDiagnostics>(sp => sp.GetRequiredService<WiaScannerDiagnostics>());
        services.AddSingleton<IScannerHealthMonitor>(sp => sp.GetRequiredService<WiaScannerHealthMonitor>());
        services.AddSingleton<IScannerProvider>(sp => sp.GetRequiredService<WiaScannerProvider>());

        return services;
    }
}
