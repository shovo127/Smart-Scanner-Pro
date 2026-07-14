namespace SmartScannerPro.Application.ScanWorkflow.Notifications;

using MediatR;
using SmartScannerPro.Application.ScanWorkflow.Models;

/// <summary>
/// Published when a scanning workflow terminates, regardless of outcome
/// (success, cancellation, or failure).
/// </summary>
/// <param name="Result">The final result of the workflow.</param>
/// <param name="WorkflowId">The unique identifier of the workflow that completed.</param>
public record WorkflowCompletedNotification(
    WorkflowResult Result,
    Guid WorkflowId) : INotification;
