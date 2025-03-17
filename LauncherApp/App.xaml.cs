using LauncherApp.Services;
using LauncherApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LauncherApp
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }

        public static Window MainWindow { get; private set; } = null!;
        public static new App Current => (App)Application.Current;

        /// <summary>
        /// Initializes the singleton application object.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Setup Services
            Services = ConfigureServices();
        }

        /// <summary>
        /// Configure application services
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<ISettingsService, JsonSettingsService>();
            services.AddSingleton<ILauncherService, ProcessLauncherService>();
            services.AddSingleton<IDialogService, DialogService>();

            // Register ViewModels
            // services.AddTransient<LauncherViewModel>();
            // services.AddTransient<SettingsViewModel>();

            // Register logging
            services.AddLogging(configure =>
            {
                configure.AddDebug();
#if DEBUG
                configure.SetMinimumLevel(LogLevel.Debug);
#else
                configure.SetMinimumLevel(LogLevel.Information);
#endif
            });

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.
        /// </summary>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            
            // Activate the window
            MainWindow.Activate();
            
            // Navigate to the launcher page
            if (MainWindow.Content is not Frame rootFrame)
            {
                rootFrame = new Frame();
                MainWindow.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(LauncherPage));
        }
    }
}
