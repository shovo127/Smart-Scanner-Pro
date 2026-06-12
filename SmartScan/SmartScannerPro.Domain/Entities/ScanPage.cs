namespace SmartScannerPro.Domain.Entities;

using System;
using SmartScannerPro.Domain.Abstractions;
using SmartScannerPro.Domain.ValueObjects;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Represents a single scanned page.
/// </summary>
public class ScanPage : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScanPage"/> class.
    /// </summary>
    /// <param name="number">The page number.</param>
    /// <param name="imagePath">The image file path.</param>
    /// <param name="size">The file size.</param>
    /// <param name="hash">The file hash.</param>
    public ScanPage(PageNumber number, string imagePath, FileSize size, FileHash hash)
        : base(Guid.NewGuid())
    {
        Guard.NotNullOrWhiteSpace(imagePath, nameof(imagePath));

        this.Number = number;
        this.ImagePath = imagePath;
        this.Size = size;
        this.Hash = hash;
    }

    /// <summary>
    /// Gets the page number within the document.
    /// </summary>
    public PageNumber Number { get; private set; }

    /// <summary>
    /// Gets the file path to the raw scanned image.
    /// </summary>
    public string ImagePath { get; private set; }

    /// <summary>
    /// Gets the file size of the image.
    /// </summary>
    public FileSize Size { get; private set; }

    /// <summary>
    /// Gets the file hash for integrity checks.
    /// </summary>
    public FileHash Hash { get; private set; }

    /// <summary>
    /// Updates the page number.
    /// </summary>
    /// <param name="newNumber">The new page number.</param>
    public void UpdatePageNumber(PageNumber newNumber)
    {
        this.Number = newNumber;
    }

    /// <summary>
    /// Updates the image path and associated metadata after processing.
    /// </summary>
    /// <param name="newPath">The new image path.</param>
    /// <param name="newSize">The new size.</param>
    /// <param name="newHash">The new hash.</param>
    public void UpdateImage(string newPath, FileSize newSize, FileHash newHash)
    {
        Guard.NotNullOrWhiteSpace(newPath, nameof(newPath));
        this.ImagePath = newPath;
        this.Size = newSize;
        this.Hash = newHash;
    }
}
