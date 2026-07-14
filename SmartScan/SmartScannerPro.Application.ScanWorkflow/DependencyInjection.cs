namespace SmartScannerPro.Application.ScanWorkflow;

using Microsoft.Extensions.DependencyInjection;
using SmartScannerPro.Application.ScanWorkflow.Services;

/// <summary>
/// Provides dependency injection extension methods for the Scanner Workflow orchestration layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all scanning workflow services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection for chaining.</returns>
    public static IServiceCollection AddScanWorkflow(this IServiceCollection services)
    {
        services.AddSingleton<WorkflowThumbnailService>();
        services.AddSingleton<ScanWorkflowService>();

        return services;
    }
}
