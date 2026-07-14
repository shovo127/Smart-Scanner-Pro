namespace SmartScannerPro.Application.ScanWorkflow.Notifications;

using MediatR;
using SmartScannerPro.Application.ScanWorkflow.Models;

/// <summary>
/// Published by the workflow engine immediately after a page has been successfully
/// acquired, saved to the staging folder, and thumbnailed.
/// </summary>
/// <param name="Page">The page that was scanned.</param>
/// <param name="WorkflowId">The unique identifier of the workflow that produced this page.</param>
public record PageScannedNotification(
    WorkflowPage Page,
    Guid WorkflowId) : INotification;
