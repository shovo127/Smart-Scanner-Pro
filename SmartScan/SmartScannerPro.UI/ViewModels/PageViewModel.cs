namespace SmartScannerPro.UI.ViewModels;

using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

/// <summary>
/// Represents an acquired scanned page in the queue.
/// </summary>
public sealed partial class PageViewModel : ObservableObject
{
    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private int pageNumber;

    [ObservableProperty]
    private int rotation; // 0, 90, 180, 270

    private ImageSource? thumbnail;
    private ImageSource? previewImage;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageViewModel"/> class.
    /// </summary>
    /// <param name="imagePath">The path to the full-size image.</param>
    /// <param name="pageNumber">The index of the page in the document.</param>
    public PageViewModel(string imagePath, int pageNumber)
    {
        this.imagePath = imagePath ?? throw new ArgumentNullException(nameof(imagePath));
        this.pageNumber = pageNumber;
        this.rotation = 0;
    }

    /// <summary>
    /// Gets the visual thumbnail source for UI representation.
    /// </summary>
    public ImageSource Thumbnail
    {
        get
        {
            if (this.thumbnail == null)
            {
                this.thumbnail = this.LoadThumbnail();
            }
            return this.thumbnail;
        }
    }

    /// <summary>
    /// Gets the full preview image source for UI representation.
    /// </summary>
    public ImageSource PreviewImage
    {
        get
        {
            if (this.previewImage == null)
            {
                this.previewImage = this.LoadPreviewImage();
            }
            return this.previewImage;
        }
    }

    /// <summary>
    /// Clears the cached thumbnail and preview, then reloads them (e.g. after rescan).
    /// </summary>
    public void RefreshThumbnail()
    {
        this.thumbnail = null;
        this.previewImage = null;
        this.OnPropertyChanged(nameof(this.Thumbnail));
        this.OnPropertyChanged(nameof(this.PreviewImage));
    }

    private ImageSource LoadThumbnail()
    {
        if (!File.Exists(this.ImagePath))
        {
            return new BitmapImage();
        }

        try
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(this.ImagePath);
            bitmap.DecodePixelWidth = 200; // Small decoded width for memory efficiency
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // Freezable for cross-thread access
            return bitmap;
        }
        catch
        {
            return new BitmapImage();
        }
    }

    private ImageSource LoadPreviewImage()
    {
        if (!File.Exists(this.ImagePath))
        {
            return new BitmapImage();
        }

        try
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(this.ImagePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // Freezable for cross-thread access
            return bitmap;
        }
        catch
        {
            return new BitmapImage();
        }
    }
}
