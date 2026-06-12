namespace SmartScannerPro.Domain.ValueObjects;

using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a page number in a document.
/// </summary>
public readonly record struct PageNumber
{
    /// <summary>
    /// Gets the 1-based page number.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageNumber"/> struct.
    /// </summary>
    /// <param name="value">The page number (must be >= 1).</param>
    public PageNumber(int value)
    {
        Guard.IsTrue(value >= 1, "Page number must be 1 or greater.");
        this.Value = value;
    }

    /// <summary>
    /// Returns the string representation of the page number.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value.ToString();
}
