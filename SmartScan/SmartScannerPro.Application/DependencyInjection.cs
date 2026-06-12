namespace SmartScannerPro.Application;

using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.Pipelines.Behaviors;

/// <summary>
/// Provides dependency injection extensions for the Application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register AutoMapper
        services.AddAutoMapper(assembly);

        // Register FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Register MediatR and Pipeline Behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            
            // Order of execution: Exception -> Logging -> Performance -> Validation -> Transaction -> Handler
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });

        return services;
    }
}
