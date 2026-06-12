namespace SmartScannerPro.Domain.Enums;

/// <summary>
/// Specifies the lifecycle state of a plugin.
/// </summary>
public enum PluginState
{
    /// <summary>
    /// The plugin is not loaded into memory.
    /// </summary>
    Unloaded = 0,

    /// <summary>
    /// The plugin assembly is loaded but not initialized.
    /// </summary>
    Loaded = 1,

    /// <summary>
    /// The plugin is successfully initialized and running.
    /// </summary>
    Initialized = 2,

    /// <summary>
    /// The plugin failed to load or initialize.
    /// </summary>
    Failed = 3,
}
