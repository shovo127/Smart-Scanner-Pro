namespace SmartScannerPro.Scanner.Mock.Extensions;

using System;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Mock.Configuration;
using SmartScannerPro.Scanner.Mock.Devices;
using SmartScannerPro.Scanner.Mock.Diagnostics;
using SmartScannerPro.Scanner.Mock.Discovery;
using SmartScannerPro.Scanner.Mock.Engine;
using SmartScannerPro.Scanner.Mock.Factory;
using SmartScannerPro.Scanner.Mock.Provider;

/// <summary>
/// Provides the <see cref="AddMockScanner"/> extension method for registering the
/// complete Mock Scanner Provider with the dependency injection container.
/// Use this instead of real scanner services to enable Demo Mode or automated testing
/// without physical hardware.
/// </summary>
public static class MockScannerServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Mock Scanner Provider services with the dependency injection container.
    /// After calling this method, <see cref="IScannerEngine"/> resolves to <see cref="MockScannerEngine"/>.
    /// </summary>
    /// <param name="services">The service collection to register into.</param>
    /// <param name="configure">
    /// An optional delegate to customise <see cref="MockScannerConfiguration"/>.
    /// When not provided, <see cref="MockScannerConfiguration.CreateDefault"/> is used.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddMockScanner(
        this IServiceCollection services,
        Action<MockScannerConfiguration>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Build configuration
        var configuration = MockScannerConfiguration.CreateDefault();
        configure?.Invoke(configuration);

        // Register configuration as singleton
        services.AddSingleton(configuration);

        // Core infrastructure
        services.AddSingleton<MockScannerDeviceRegistry>();
        services.AddSingleton<MockScannerDiscoveryService>();
        services.AddSingleton<MockScannerProvider>();

        // Session lifecycle
        services.AddSingleton<MockScannerFactory>();

        // Diagnostics
        services.AddSingleton<MockScannerDiagnostics>();
        services.AddSingleton<MockScannerHealthMonitor>();

        // Engine (top-level entry point)
        services.AddSingleton<MockScannerEngine>();

        // Register SDK interfaces so callers never reference Mock types directly
        services.AddSingleton<IScannerEngine>(sp => sp.GetRequiredService<MockScannerEngine>());
        services.AddSingleton<IScannerFactory>(sp => sp.GetRequiredService<MockScannerFactory>());
        services.AddSingleton<IScannerDiscoveryService>(sp => sp.GetRequiredService<MockScannerDiscoveryService>());
        services.AddSingleton<IScannerDiagnostics>(sp => sp.GetRequiredService<MockScannerDiagnostics>());
        services.AddSingleton<IScannerHealthMonitor>(sp => sp.GetRequiredService<MockScannerHealthMonitor>());
        services.AddSingleton<IScannerProvider>(sp => sp.GetRequiredService<MockScannerProvider>());

        return services;
    }
}
