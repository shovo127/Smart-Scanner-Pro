namespace SmartScannerPro.Application.ScanWorkflow.Notifications;

using MediatR;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;

/// <summary>
/// Published on every progress update received from the underlying scanner driver
/// during an active workflow execution.
/// </summary>
/// <param name="Progress">The raw progress data from the scanner driver.</param>
/// <param name="CurrentPage">The 1-based page number currently being acquired.</param>
/// <param name="WorkflowId">The unique identifier of the active workflow.</param>
public record WorkflowProgressNotification(
    ScanProgress Progress,
    int CurrentPage,
    Guid WorkflowId) : INotification;
