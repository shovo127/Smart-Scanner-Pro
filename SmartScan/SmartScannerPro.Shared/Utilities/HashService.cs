namespace SmartScannerPro.Shared.Utilities;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides cryptographic hashing utilities.
/// </summary>
public static class HashService
{
    /// <summary>
    /// Computes the SHA256 hash of the specified string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The computed hash as a hexadecimal string.</returns>
    public static string ComputeSha256(string input)
    {
        Guard.NotNullOrWhiteSpace(input, nameof(input));

        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
