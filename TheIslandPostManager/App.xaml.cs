using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using NetSparkleUpdater;
using Serilog;
using TheIslandPostManager.ViewModels;

namespace TheIslandPostManager;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static bool IsRetailPage { get; set; }
    public static Configuration AppConfig { get; set; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


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
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<IMySQLService, MySQLService>();

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

                services.AddSingleton<Views.Pages.BackupPage>();
                services.AddSingleton<ViewModels.BackupPageViewModel>();

                services.AddTransient<Views.Pages.OrderHistoryEditorPage>();
                services.AddTransient<ViewModels.OrderHistoryEditorPageViewmodel>();

                services.AddTransient<Views.Pages.RetailPage>();

                services.AddTransient<Views.Pages.CompleteOrderPage>();
                services.AddTransient<Dialogs.CompleteOrderDialogViewModel>();


                services.AddTransient<Views.Pages.EarningsPage>();
                services.AddTransient<EarningsPageViewmodel>();

                //services.AddSingleton<CustomerWindow>();
                //services.AddSingleton<ViewModels.CustomerWindowViewmodel>();


                var settings = AppConfig.GetSection("AppSettings") as AppSettings;

                var smtp = new SmtpClient
                {
                    Host = settings.Host,
                    Port = settings.PortNumber,
                    EnableSsl = settings.EnableSSL,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(settings.Email, settings.Password),
                    Timeout = settings.EmailTimeout * 1000
                };

                services.AddFluentEmail(settings.Email, settings.CompanyName)
                        .AddRazorRenderer()
                        .AddSmtpSender(smtp);

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));


            }
        )
        .Build();
    private SparkleUpdater _sparkle;

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

        var setting = AppConfig.GetSection("AppSettings") as AppSettings;

        setting.InputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Input");
        await _host.StartAsync();

        SetupExceptionHandling();
    }

    private void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

        DispatcherUnhandledException += (s, e) =>
        {
            LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
            e.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
            e.SetObserved();
        };
    }

    private void LogUnhandledException(Exception exception, string source)
    {
        string message = $"Unhandled exception ({source})";
        try
        {
            System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
        }
        catch (Exception ex)
        {
            //await messageService.ShowErrorMessage("Complete Order Error", ex.Message, ex.StackTrace, "952a5653-4a41-4119-81b8-b423faddb787", true);
            Log.Error(ex, "Exception in LogUnhandledException");
        }
        finally
        {
            MessageBox.Show("Error occured Please Check Log (Error Code: 0793ca31-5b45-45e5-b33f-28bbf660f3b9)");
            Log.Error(exception, message);
        }
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
