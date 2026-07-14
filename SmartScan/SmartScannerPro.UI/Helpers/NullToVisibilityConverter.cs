namespace SmartScannerPro.UI.Helpers;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Converts a null value to visibility.
/// </summary>
public sealed class NullToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether to invert the visibility behavior.
    /// When <c>true</c>, null values return <see cref="Visibility.Visible"/> and non-null values return <see cref="Visibility.Collapsed"/>.
    /// When <c>false</c>, null values return <see cref="Visibility.Collapsed"/> and non-null values return <see cref="Visibility.Visible"/>.
    /// </summary>
    public bool Invert { get; set; }

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isNull = value == null;
        if (this.Invert)
        {
            return isNull ? Visibility.Visible : Visibility.Collapsed;
        }

        return isNull ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
