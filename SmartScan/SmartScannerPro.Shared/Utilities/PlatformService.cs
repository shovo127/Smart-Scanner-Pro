namespace SmartScannerPro.Shared.Utilities;

using System.Runtime.InteropServices;

/// <summary>
/// Provides information about the underlying operating system and platform.
/// </summary>
public static class PlatformService
{
    /// <summary>
    /// Gets a value indicating whether the current application is running on Windows.
    /// </summary>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Gets the architecture of the operating system.
    /// </summary>
    public static Architecture OSArchitecture => RuntimeInformation.OSArchitecture;

    /// <summary>
    /// Gets the operating system description.
    /// </summary>
    public static string OSDescription => RuntimeInformation.OSDescription;
}
