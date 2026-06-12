namespace SmartScannerPro.Shared.Utilities;

using System.Reflection;

/// <summary>
/// Provides information about the application version.
/// </summary>
public static class VersionService
{
    /// <summary>
    /// Gets the current application version.
    /// </summary>
    /// <returns>The application version as a string.</returns>
    public static string GetApplicationVersion()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        return version?.ToString() ?? "1.0.0.0";
    }
}
