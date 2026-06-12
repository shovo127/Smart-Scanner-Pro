namespace SmartScannerPro.ImageProcessing;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering image processing services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds image processing layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddImageProcessing(this IServiceCollection services)
    {
        return services;
    }
}
