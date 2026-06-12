using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartScan.Services;
using SmartScan.ViewModels;
using SmartScan.Views;
using SmartScan.Views.Pages;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace SmartScan
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Services
                    services.AddSingleton<IScannerService, ScannerService>();

                    // ViewModels
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<SettingsViewModel>();

                    // Views
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<HomePage>();
                    services.AddTransient<SettingsPage>();
                    
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<IPageService, PageService>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
