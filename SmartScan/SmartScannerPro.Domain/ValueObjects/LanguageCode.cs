namespace SmartScannerPro.Domain.ValueObjects;

using System;
using System.Globalization;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a standardized language code (e.g., en-US).
/// </summary>
public readonly record struct LanguageCode
{
    /// <summary>
    /// Gets the language code string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageCode"/> struct.
    /// </summary>
    /// <param name="code">The language code (e.g. 'en', 'de', 'bn', 'ar').</param>
    public LanguageCode(string code)
    {
        Guard.NotNullOrWhiteSpace(code, nameof(code));

        try
        {
            var cultureInfo = new CultureInfo(code);
            this.Value = cultureInfo.Name;
        }
        catch (CultureNotFoundException ex)
        {
            throw new ArgumentException($"Invalid language code: {code}", nameof(code), ex);
        }
    }

    /// <summary>
    /// Returns the string representation of the language code.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() => this.Value;
}
