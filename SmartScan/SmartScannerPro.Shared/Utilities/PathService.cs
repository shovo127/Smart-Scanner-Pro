namespace SmartScannerPro.Shared.Utilities;

/// <summary>
/// A utility service for manipulating paths.
/// </summary>
public static class PathService
{
    /// <summary>
    /// Combines an array of strings into a path.
    /// </summary>
    /// <param name="paths">An array of parts of the path.</param>
    /// <returns>The combined paths.</returns>
    public static string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    /// <summary>
    /// Gets the local application data path for the application.
    /// </summary>
    /// <returns>The path to the app data folder.</returns>
    public static string GetAppDataPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SmartScannerPro");
    }
}
