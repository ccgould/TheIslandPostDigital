using IslandPostAdministrator.ViewModels;
using Syncfusion.SfSkinManager;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace IslandPostAdministrator;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(
        MainWindowViewModel viewModel,
        IPageService pageService,
        INavigationService navigationService,
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService)
    {

        string style = "Windows11Dark";
        SkinHelper styleInstance = null;
        var skinHelpterStr = "Syncfusion.Themes." + style + ".WPF." + style + "SkinHelper, Syncfusion.Themes." + style + ".WPF";
        Type skinHelpterType = Type.GetType(skinHelpterStr);
        if (skinHelpterType != null)
            styleInstance = Activator.CreateInstance(skinHelpterType) as SkinHelper;
        if (styleInstance != null)
        {
            SfSkinManager.RegisterTheme("Windows11Dark", styleInstance);
        }

        // set icon in project properties!
        string manifestModuleName = System.Reflection.Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;
        var icon = System.Drawing.Icon.ExtractAssociatedIcon(manifestModuleName);

        ViewModel = viewModel;
        DataContext = this;

        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();
        SetPageService(pageService);

        navigationService.SetNavigationControl(RootNavigation);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetContentPresenter(RootContentDialog);
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