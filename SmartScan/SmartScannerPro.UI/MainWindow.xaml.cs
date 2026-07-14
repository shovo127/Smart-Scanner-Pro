namespace SmartScannerPro.UI;

using System;
using System.Windows;
using SmartScannerPro.UI.ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="viewModel">The WorkspaceViewModel resolved from DI.</param>
    public MainWindow(WorkspaceViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        this.InitializeComponent();
        this.DataContext = viewModel;
    }
}
