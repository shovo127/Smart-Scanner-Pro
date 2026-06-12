namespace SmartScannerPro.Scanner.Abstractions.Models.Capabilities;

using System;

/// <summary>
/// Represents a single capability exposed by a scanner.
/// </summary>
public abstract record Capability
{
    /// <summary>
    /// Gets the unique identifier for this capability.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the human-readable name of the capability.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the category of this capability.
    /// </summary>
    public CapabilityCategory Category { get; init; }

    /// <summary>
    /// Gets a value indicating whether this capability is read-only.
    /// </summary>
    public bool IsReadOnly { get; init; }

    /// <summary>
    /// Gets the underlying value type of the capability.
    /// </summary>
    public required Type ValueType { get; init; }
}

/// <summary>
/// Represents a typed capability exposed by a scanner.
/// </summary>
/// <typeparam name="T">The underlying data type.</typeparam>
public record Capability<T> : Capability
{
    /// <summary>
    /// Gets the value constraints and current state of the capability.
    /// </summary>
    public required CapabilityValue<T> Value { get; init; }
}
