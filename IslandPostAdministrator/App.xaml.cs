using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using Wpf.Ui;
using IslandPostAdministrator.ViewModels;
using IslandPostAdministrator.Services;
using System.Windows.Threading;
using IslandPostAdministrator.Views.Pages;

namespace IslandPostAdministrator;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
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
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<IContentDialogService, ContentDialogService>();
                //services.AddSingleton<IEmailService, EmailService>();
                //services.AddSingleton<IMySQLService, MySQLService>();

                // Views and ViewModels
                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
           
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();

                services.AddSingleton<EmployeePage>();
                services.AddSingleton<EmployeePageViewmodel>();


                //var settings = AppConfig.GetSection("AppSettings") as AppSettings;

                //var smtp = new SmtpClient
                //{
                //    Host = settings.Host,
                //    Port = settings.PortNumber,
                //    EnableSsl = settings.EnableSSL,
                //    UseDefaultCredentials = false,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Credentials = new NetworkCredential(settings.Email, settings.Password),
                //    Timeout = settings.EmailTimeout * 1000
                //};

                //services.AddFluentEmail(settings.Email, settings.CompanyName)
                //        .AddRazorRenderer()
                //        .AddSmtpSender(smtp);

                //// Configuration
                //services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));


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


    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        ////ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
        ////fileMap.ExeConfigFilename = AppConfig.FilePath;
        ////Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        ////KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

        //if (AppConfig.Sections["AppSettings"] is null)
        //{
        //    AppConfig.Sections.Add("AppSettings", new AppSettings());
        //}


        //// settings.Add("Port", "12");
        //// configuration.Save(ConfigurationSaveMode.Modified);



        ////ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);

        //var setting = AppConfig.GetSection("AppSettings") as AppSettings;

        //setting.InputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Input");
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

