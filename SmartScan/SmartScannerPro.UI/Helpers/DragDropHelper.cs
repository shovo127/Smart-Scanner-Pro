namespace SmartScannerPro.UI.Helpers;

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SmartScannerPro.UI.ViewModels;

/// <summary>
/// Attached behaviors for enabling drag and drop thumbnail reordering in WPF ListView or ListBox.
/// </summary>
public static class DragDropHelper
{
    /// <summary>
    /// Dependency property for enabling drag and drop reordering.
    /// </summary>
    public static readonly DependencyProperty IsDragSourceProperty =
        DependencyProperty.RegisterAttached(
            "IsDragSource",
            typeof(bool),
            typeof(DragDropHelper),
            new PropertyMetadata(false, OnIsDragSourceChanged));

    private static Point startPoint;
    private static object? draggedItem;

    /// <summary>
    /// Gets the value of the IsDragSource attached property.
    /// </summary>
    /// <param name="obj">The target element.</param>
    /// <returns>True if enabled; otherwise, false.</returns>
    public static bool GetIsDragSource(DependencyObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return (bool)obj.GetValue(IsDragSourceProperty);
    }

    /// <summary>
    /// Sets the value of the IsDragSource attached property.
    /// </summary>
    /// <param name="obj">The target element.</param>
    /// <param name="value">True to enable; otherwise, false.</param>
    public static void SetIsDragSource(DependencyObject obj, bool value)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.SetValue(IsDragSourceProperty, value);
    }

    private static void OnIsDragSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListBox listBox)
        {
            if ((bool)e.NewValue)
            {
                listBox.PreviewMouseLeftButtonDown += ListBox_PreviewMouseLeftButtonDown;
                listBox.PreviewMouseMove += ListBox_PreviewMouseMove;
                listBox.Drop += ListBox_Drop;
                listBox.AllowDrop = true;
            }
            else
            {
                listBox.PreviewMouseLeftButtonDown -= ListBox_PreviewMouseLeftButtonDown;
                listBox.PreviewMouseMove -= ListBox_PreviewMouseMove;
                listBox.Drop -= ListBox_Drop;
                listBox.AllowDrop = false;
            }
        }
    }

    private static void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        startPoint = e.GetPosition(null);
        if (sender is ListBox)
        {
            draggedItem = FindVisualParent<ListBoxItem>((DependencyObject)e.OriginalSource)?.Content;
        }
    }

    private static void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && draggedItem != null)
        {
            var mousePos = e.GetPosition(null);
            var diff = startPoint - mousePos;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                if (sender is ListBox listBox)
                {
                    DragDrop.DoDragDrop(listBox, draggedItem, DragDropEffects.Move);
                    draggedItem = null;
                }
            }
        }
    }

    private static void ListBox_Drop(object sender, DragEventArgs e)
    {
        if (sender is ListBox listBox && e.Data.GetDataPresent(typeof(PageViewModel)))
        {
            var sourceItem = e.Data.GetData(typeof(PageViewModel)) as PageViewModel;
            if (sourceItem == null)
            {
                return;
            }

            var targetItem = FindVisualParent<ListBoxItem>((DependencyObject)e.OriginalSource)?.Content as PageViewModel;
            if (listBox.ItemsSource is ObservableCollection<PageViewModel> list)
            {
                int oldIndex = list.IndexOf(sourceItem);
                int newIndex = targetItem != null ? list.IndexOf(targetItem) : list.Count - 1;

                if (oldIndex != newIndex && oldIndex >= 0 && newIndex >= 0)
                {
                    list.Move(oldIndex, newIndex);

                    // Re-index page numbers
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].PageNumber = i + 1;
                    }
                }
            }
        }
    }

    private static T? FindVisualParent<T>(DependencyObject child)
        where T : DependencyObject
    {
        var parentObject = VisualTreeHelper.GetParent(child);
        if (parentObject == null)
        {
            return null;
        }

        if (parentObject is T parent)
        {
            return parent;
        }

        return FindVisualParent<T>(parentObject);
    }
}
