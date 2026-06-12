using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartScannerPro.Application;
using SmartScannerPro.Infrastructure;
using SmartScannerPro.Settings;

namespace SmartScannerPro.UI;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : System.Windows.Application
{
    private static IHost? host;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register Layers
                services.AddApplication()
                        .AddInfrastructure()
                        .AddSettings();

                // Register UI Views and ViewModels
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    /// <inheritdoc/>
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
        {
            // Log fatal crash here
            MessageBox.Show("A fatal error occurred. The application will close.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        await host!.StartAsync().ConfigureAwait(true);

        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    /// <inheritdoc/>
    protected override async void OnExit(ExitEventArgs e)
    {
        await host!.StopAsync().ConfigureAwait(true);
        host.Dispose();
        base.OnExit(e);
    }
}
