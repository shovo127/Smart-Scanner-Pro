namespace SmartScannerPro.Shared.Results;

/// <summary>
/// Defines the semantic category of an error.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A general failure.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// A validation error.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// A resource not found error.
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// A resource conflict error.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// A configuration error.
    /// </summary>
    Configuration = 4,
}
