namespace SmartScannerPro.Application.Interfaces;

/// <summary>
/// An abstraction for sending notifications to the user.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Shows an informational notification.
    /// </summary>
    /// <param name="message">The message to show.</param>
    void ShowInfo(string message);

    /// <summary>
    /// Shows a success notification.
    /// </summary>
    /// <param name="message">The message to show.</param>
    void ShowSuccess(string message);

    /// <summary>
    /// Shows a warning notification.
    /// </summary>
    /// <param name="message">The message to show.</param>
    void ShowWarning(string message);

    /// <summary>
    /// Shows an error notification.
    /// </summary>
    /// <param name="message">The message to show.</param>
    void ShowError(string message);

    /// <summary>
    /// Shows a progress notification.
    /// </summary>
    /// <param name="message">The message to show.</param>
    void ShowProgress(string message);

    /// <summary>
    /// Shows a dialog notification.
    /// </summary>
    /// <param name="title">The dialog title.</param>
    /// <param name="message">The dialog message.</param>
    void ShowDialog(string title, string message);

    /// <summary>
    /// Shows a snackbar notification.
    /// </summary>
    /// <param name="message">The message to show.</param>
    void ShowSnackbar(string message);
}
