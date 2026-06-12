namespace SmartScannerPro.Application.Pipelines.Behaviors;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// Pipeline behavior for monitoring performance and identifying slow requests.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();

        var response = await next();

        timer.Stop();

        if (timer.ElapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            this.logger.LogWarning("Long Running Request: {RequestName} ({ElapsedMilliseconds} ms)", requestName, timer.ElapsedMilliseconds);
        }

        return response;
    }
}
