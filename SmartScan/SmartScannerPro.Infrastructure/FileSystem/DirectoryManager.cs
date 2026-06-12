namespace SmartScannerPro.Infrastructure.FileSystem;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Manages application directories.
/// </summary>
public sealed class DirectoryManager : IDirectoryManager
{
    private readonly string basePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryManager"/> class.
    /// </summary>
    public DirectoryManager()
    {
        this.basePath = PathService.GetAppDataPath();
        this.LogsDirectory = PathService.Combine(this.basePath, "Logs");
        this.CacheDirectory = PathService.Combine(this.basePath, "Cache");
        this.ProfilesDirectory = PathService.Combine(this.basePath, "Profiles");
        this.SettingsDirectory = PathService.Combine(this.basePath, "Settings");
        this.PluginsDirectory = PathService.Combine(this.basePath, "Plugins");
        this.DiagnosticsDirectory = PathService.Combine(this.basePath, "Diagnostics");
        this.TempDirectory = PathService.Combine(this.basePath, "Temp");
        this.ExportsDirectory = PathService.Combine(this.basePath, "Exports");
        this.UserDataDirectory = PathService.Combine(this.basePath, "UserData");
    }

    /// <inheritdoc/>
    public string LogsDirectory { get; }

    /// <inheritdoc/>
    public string CacheDirectory { get; }

    /// <inheritdoc/>
    public string ProfilesDirectory { get; }

    /// <inheritdoc/>
    public string SettingsDirectory { get; }

    /// <inheritdoc/>
    public string PluginsDirectory { get; }

    /// <inheritdoc/>
    public string DiagnosticsDirectory { get; }

    /// <inheritdoc/>
    public string TempDirectory { get; }

    /// <inheritdoc/>
    public string ExportsDirectory { get; }

    /// <inheritdoc/>
    public string UserDataDirectory { get; }

    /// <inheritdoc/>
    public void InitializeDirectories()
    {
        var directories = new[]
        {
            this.basePath,
            this.LogsDirectory,
            this.CacheDirectory,
            this.ProfilesDirectory,
            this.SettingsDirectory,
            this.PluginsDirectory,
            this.DiagnosticsDirectory,
            this.TempDirectory,
            this.ExportsDirectory,
            this.UserDataDirectory,
        };

        foreach (var dir in directories)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
