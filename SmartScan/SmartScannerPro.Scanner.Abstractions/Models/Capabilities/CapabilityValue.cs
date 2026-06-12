namespace SmartScannerPro.Scanner.Abstractions.Models.Capabilities;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents the supported values or bounds for a capability.
/// </summary>
/// <typeparam name="T">The underlying data type of the capability.</typeparam>
public record CapabilityValue<T>
{
    /// <summary>
    /// Gets the default value for this capability.
    /// </summary>
    public T? DefaultValue { get; init; }

    /// <summary>
    /// Gets the currently selected value.
    /// </summary>
    public T? CurrentValue { get; init; }

    /// <summary>
    /// Gets the list of explicitly supported discrete values.
    /// </summary>
    public IReadOnlyList<T>? SupportedValues { get; init; }

    /// <summary>
    /// Gets the minimum allowed value (if applicable to the type).
    /// </summary>
    public T? MinValue { get; init; }

    /// <summary>
    /// Gets the maximum allowed value (if applicable to the type).
    /// </summary>
    public T? MaxValue { get; init; }

    /// <summary>
    /// Gets the step value for ranges (if applicable).
    /// </summary>
    public T? Step { get; init; }

    /// <summary>
    /// Checks if a specified value is supported.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is supported; otherwise, false.</returns>
    public bool IsSupported(T value)
    {
        if (this.SupportedValues is not null && this.SupportedValues.Count > 0)
        {
            foreach (var supportedValue in this.SupportedValues)
            {
                if (EqualityComparer<T>.Default.Equals(value, supportedValue))
                {
                    return true;
                }
            }
            return false;
        }

        // Add range checking logic here if MinValue/MaxValue are implemented and type is IComparable
        if (value is IComparable comparableValue)
        {
            if (this.MinValue is IComparable min && comparableValue.CompareTo(min) < 0)
            {
                return false;
            }

            if (this.MaxValue is IComparable max && comparableValue.CompareTo(max) > 0)
            {
                return false;
            }
        }

        return true;
    }
}
