namespace SmartScannerPro.Application.Interfaces;

/// <summary>
/// An abstraction for managing application directories.
/// </summary>
public interface IDirectoryManager
{
    /// <summary>
    /// Gets the path to the logs directory.
    /// </summary>
    string LogsDirectory { get; }

    /// <summary>
    /// Gets the path to the cache directory.
    /// </summary>
    string CacheDirectory { get; }

    /// <summary>
    /// Gets the path to the profiles directory.
    /// </summary>
    string ProfilesDirectory { get; }

    /// <summary>
    /// Gets the path to the settings directory.
    /// </summary>
    string SettingsDirectory { get; }

    /// <summary>
    /// Gets the path to the plugins directory.
    /// </summary>
    string PluginsDirectory { get; }

    /// <summary>
    /// Gets the path to the diagnostics directory.
    /// </summary>
    string DiagnosticsDirectory { get; }

    /// <summary>
    /// Gets the path to the temp directory.
    /// </summary>
    string TempDirectory { get; }

    /// <summary>
    /// Gets the path to the exports directory.
    /// </summary>
    string ExportsDirectory { get; }

    /// <summary>
    /// Gets the path to the user data directory.
    /// </summary>
    string UserDataDirectory { get; }

    /// <summary>
    /// Initializes and creates the necessary directories if they do not exist.
    /// </summary>
    void InitializeDirectories();
}
