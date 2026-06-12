namespace SmartScannerPro.UI.Services;
using System.Windows;
using SmartScannerPro.Application.Interfaces;

/// <summary>
/// Implements notifications for the WPF UI.
/// </summary>
public class NotificationService : INotificationService
{
    /// <inheritdoc/>
    public void ShowInfo(string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <inheritdoc/>
    public void ShowSuccess(string message)
    {
        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <inheritdoc/>
    public void ShowWarning(string message)
    {
        MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <inheritdoc/>
    public void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <inheritdoc/>
    public void ShowProgress(string message)
    {
        // Placeholder for progress reporting.
    }

    /// <inheritdoc/>
    public void ShowDialog(string title, string message)
    {
        MessageBox.Show(message, title, MessageBoxButton.OK);
    }

    /// <inheritdoc/>
    public void ShowSnackbar(string message)
    {
        // Placeholder for snackbar notification.
    }
}
