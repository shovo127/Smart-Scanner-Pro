namespace SmartScannerPro.Shared.Results;

/// <summary>
/// Standard error codes used across the application.
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Error codes related to the file system.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// File or directory not found.
        /// </summary>
        public const string NotFound = "FileSystem.NotFound";

        /// <summary>
        /// Access denied to file or directory.
        /// </summary>
        public const string AccessDenied = "FileSystem.AccessDenied";

        /// <summary>
        /// Invalid path format.
        /// </summary>
        public const string InvalidPath = "FileSystem.InvalidPath";
    }

    /// <summary>
    /// Error codes related to scanner hardware.
    /// </summary>
    public static class Scanner
    {
        /// <summary>
        /// Scanner device not found.
        /// </summary>
        public const string DeviceNotFound = "Scanner.DeviceNotFound";

        /// <summary>
        /// Scanner disconnected during operation.
        /// </summary>
        public const string Disconnected = "Scanner.Disconnected";

        /// <summary>
        /// Paper jam detected.
        /// </summary>
        public const string PaperJam = "Scanner.PaperJam";

        /// <summary>
        /// Scanner initialization failed.
        /// </summary>
        public const string InitializationFailed = "Scanner.InitializationFailed";
    }
}
