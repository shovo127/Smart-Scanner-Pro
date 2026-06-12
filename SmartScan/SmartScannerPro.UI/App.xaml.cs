namespace SmartScannerPro.UI;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SmartScannerPro.Application;
using SmartScannerPro.Application.Interfaces;
using SmartScannerPro.Diagnostics;
using SmartScannerPro.ImageProcessing;
using SmartScannerPro.Infrastructure;
using SmartScannerPro.Infrastructure.Logging;
using SmartScannerPro.Localization;
using SmartScannerPro.OCR;
using SmartScannerPro.PDF;
using SmartScannerPro.Plugins;
using SmartScannerPro.Scanner;
using SmartScannerPro.Scanner.Drivers;
using SmartScannerPro.Settings;
using SmartScannerPro.Shared;
using SmartScannerPro.Shared.Utilities;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IHost host;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        var logsDir = PathService.Combine(PathService.GetAppDataPath(), "Logs");
        if (!Directory.Exists(logsDir))
        {
            Directory.CreateDirectory(logsDir);
        }

        Log.Logger = SerilogConfiguration.CreateLogger(logsDir);

        try
        {
            Log.Information("Starting Smart Scanner Pro Bootstrap");

            this.host = Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    // Register Application Layers
                    services.AddShared()
                            .AddApplication()
                            .AddInfrastructure()
                            .AddSettings()
                            .AddLocalizationServices()
                            .AddDiagnostics()
                            .AddScanner()
                            .AddScannerDrivers()
                            .AddImageProcessing()
                            .AddOCR()
                            .AddPDF()
                            .AddPlugins()
                            .AddUI();

                    // Register UI Window
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly during startup.");
            throw;
        }

        // Global Exception Handlers
        this.DispatcherUnhandledException += this.OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += this.OnAppDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += this.OnUnobservedTaskException;
    }

    /// <inheritdoc/>
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            await this.host.StartAsync().ConfigureAwait(true);

            // Initialize infrastructure directories
            var dirManager = this.host.Services.GetRequiredService<IDirectoryManager>();
            dirManager.InitializeDirectories();

            Log.Information("Directories initialized successfully.");

            var mainWindow = this.host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to start application properly.");
            MessageBox.Show("A fatal error occurred during startup. See logs for details.", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Shutdown(1);
        }
    }

    /// <inheritdoc/>
    protected override async void OnExit(ExitEventArgs e)
    {
        Log.Information("Application shutting down gracefully.");

        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
        {
            await this.host.StopAsync(cts.Token).ConfigureAwait(true);
        }

        this.host.Dispose();
        await Log.CloseAndFlushAsync();
        base.OnExit(e);
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Fatal(e.Exception, "Unhandled UI Dispatcher Exception.");
        e.Handled = true;
        MessageBox.Show("An unexpected error occurred. The application may become unstable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        Log.Fatal(ex, "Unhandled AppDomain Exception.");
        MessageBox.Show("A fatal error occurred. The application will close.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        Log.CloseAndFlush();
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Unobserved Task Exception.");
        e.SetObserved();
    }
}
