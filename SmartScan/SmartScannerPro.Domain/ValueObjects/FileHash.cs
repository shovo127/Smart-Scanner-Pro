namespace SmartScannerPro.Domain.ValueObjects;

using System;
using System.Text.RegularExpressions;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a cryptographic hash of a file.
/// </summary>
public readonly record struct FileHash
{
    private static readonly Regex HashRegex = new Regex("^[a-fA-F0-9]{64}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the SHA-256 hash string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileHash"/> struct.
    /// </summary>
    /// <param name="hash">The SHA-256 hash.</param>
    public FileHash(string hash)
    {
        Guard.NotNullOrWhiteSpace(hash, nameof(hash));
        Guard.IsTrue(HashRegex.IsMatch(hash), "Invalid SHA-256 hash format.");
        this.Value = hash.ToLowerInvariant();
    }

    /// <summary>
    /// Returns the string representation of the hash.
    /// </summary>
    /// <returns>The hash.</returns>
    public override string ToString() => this.Value;
}
