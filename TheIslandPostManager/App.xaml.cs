using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using TheIslandPostManager.Windows;
using Wpf.Ui;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace TheIslandPostManager;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory));
        })
        .ConfigureServices(
            (context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<IOrderService, OrderService>();
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IImageService, ImageService>();
                services.AddSingleton<IMessageService, MessageService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<ViewModels.MainWindowViewModel>();

                services.AddSingleton<IContentDialogService, ContentDialogService>();

                // Views and ViewModels
                services.AddSingleton<Views.Pages.DashboardPage>();
                services.AddSingleton<ViewModels.DashboardViewModel>();
                services.AddSingleton<Views.Pages.OrdersPage>();
                services.AddSingleton<ViewModels.OrdersPageViewModel>();
                services.AddSingleton<Views.Pages.PendingOrdersPage>();
                services.AddSingleton<ViewModels.PendingOrdersPageViewModel>();

                //services.AddSingleton<ViewModels.DataViewModel>();

                services.AddSingleton<Views.Pages.SettingsPage>();
                services.AddSingleton<ViewModels.SettingsViewModel>();

                //services.AddSingleton<CustomerWindow>();
                //services.AddSingleton<ViewModels.CustomerWindowViewmodel>();

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));


            }
        )
        .Build();

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T? GetService<T>()
        where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    public static Configuration AppConfig { get; set; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
     {

        //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
        //fileMap.ExeConfigFilename = AppConfig.FilePath;
        //Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        //KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

        if (AppConfig.Sections["AppSettings"] is null)
        {
            AppConfig.Sections.Add("AppSettings", new AppSettings());
        }


        // settings.Add("Port", "12");
        // configuration.Save(ConfigurationSaveMode.Modified);



        //ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);

       // var setting = AppConfig.GetSection("AppSettings");
        await _host.StartAsync();
    } 

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}
