namespace SmartScannerPro.Shared.Utilities;

/// <summary>
/// A utility service for accessing environment information.
/// </summary>
public static class EnvironmentService
{
    /// <summary>
    /// Gets the current machine name.
    /// </summary>
    public static string MachineName => Environment.MachineName;

    /// <summary>
    /// Gets the current user name.
    /// </summary>
    public static string UserName => Environment.UserName;

    /// <summary>
    /// Gets the current process architecture.
    /// </summary>
    public static string Architecture => System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();

    /// <summary>
    /// Gets the current OS version.
    /// </summary>
    public static string OSVersion => Environment.OSVersion.VersionString;
}
