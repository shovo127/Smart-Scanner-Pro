namespace SmartScannerPro.Scanner.Abstractions.Models.Capabilities;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a collection of capabilities exposed by a scanner device.
/// </summary>
public class CapabilitySet
{
    private readonly Dictionary<string, Capability> capabilities = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="CapabilitySet"/> class.
    /// </summary>
    /// <param name="capabilities">The initial capabilities.</param>
    public CapabilitySet(IEnumerable<Capability> capabilities)
    {
        foreach (var cap in capabilities)
        {
            this.capabilities[cap.Id] = cap;
        }
    }

    /// <summary>
    /// Gets all capabilities.
    /// </summary>
    public IEnumerable<Capability> All => this.capabilities.Values;

    /// <summary>
    /// Gets a capability by its identifier.
    /// </summary>
    /// <param name="id">The capability identifier.</param>
    /// <returns>The capability, or null if not found.</returns>
    public Capability? GetCapability(string id)
    {
        return this.capabilities.TryGetValue(id, out var capability) ? capability : null;
    }

    /// <summary>
    /// Gets a strongly typed capability by its identifier.
    /// </summary>
    /// <typeparam name="T">The expected underlying type.</typeparam>
    /// <param name="id">The capability identifier.</param>
    /// <returns>The typed capability, or null if not found or type mismatch.</returns>
    public Capability<T>? GetCapability<T>(string id)
    {
        var cap = this.GetCapability(id);
        return cap as Capability<T>;
    }

    /// <summary>
    /// Gets all capabilities within a specific category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>A collection of capabilities.</returns>
    public IEnumerable<Capability> GetByCategory(CapabilityCategory category)
    {
        return this.capabilities.Values.Where(c => c.Category == category);
    }
}
