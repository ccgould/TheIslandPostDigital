using NetSparkleUpdater.SignatureVerifiers;
using NetSparkleUpdater;
using System.Windows;
using TheIslandPostManager.Controls;
using TheIslandPostManager.ViewModels;
using TheIslandPostManager.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace TheIslandPostManager;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow
{
    public MainWindowViewModel ViewModel { get; }

    private SparkleUpdater _sparkle;


    public MainWindow(
        MainWindowViewModel viewModel,
        IPageService pageService,
        INavigationService navigationService,
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService
    )
    {
        // set icon in project properties!
        string manifestModuleName = System.Reflection.Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;
        var icon = System.Drawing.Icon.ExtractAssociatedIcon(manifestModuleName);


        //ccgould.github.io/Data/appcast.xml
        _sparkle = new SparkleUpdater("https://netsparkleupdater.github.io/NetSparkle/files/sample-app/appcast.xml", new DSAChecker(NetSparkleUpdater.Enums.SecurityMode.Strict))
        {
            UIFactory = new NetSparkleUpdater.UI.WPF.UIFactory(NetSparkleUpdater.UI.WPF.IconUtilities.ToImageSource(icon))
            {
                ProcessWindowAfterInit = (window, factory) =>
                {
                    // Example of setting font styles on a window after init: 
                    TextBlock.SetFontStyle(window, FontStyles.Italic);
                }
            },
            ShowsUIOnMainThread = false,
            //RelaunchAfterUpdate = true,
            //UseNotificationToast = true
        };
        // TLS 1.2 required by GitHub (https://developer.github.com/changes/2018-02-01-weak-crypto-removal-notice/)
        _sparkle.SecurityProtocolType = System.Net.SecurityProtocolType.Tls12;
        _sparkle.StartLoop(true, true);
        //imageControlModule. += ImageControlModule_myEvent;

        ViewModel = viewModel;
        DataContext = this;

        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();
        SetPageService(pageService);

        navigationService.SetNavigationControl(RootNavigation);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetContentPresenter(RootContentDialog);
    }

    private void ManualUpdateCheck_Click(object sender, RoutedEventArgs e)
    {
        _sparkle.CheckForUpdatesAtUserRequest();
    }

    private void ImageControlModule_myEvent(int someValue)
    {
        throw new NotImplementedException();
    }

    #region INavigationWindow methods

    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    #endregion INavigationWindow methods

    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
    }

    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}