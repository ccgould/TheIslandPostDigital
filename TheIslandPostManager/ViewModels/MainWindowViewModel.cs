using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
using Wpf.Ui;
using TheIslandPostManager.Services;
using System.Reflection;

namespace TheIslandPostManager.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _applicationTitle = String.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = new();

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = new();

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new();

    public MainWindowViewModel(INavigationService navigationService,IOrderService orderService)
    {
        if (!_isInitialized)
            InitializeViewModel();
        this.orderService = orderService;
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = $"Island Post Digitial - Photography Depmartment V{Assembly.GetExecutingAssembly().GetName().Version}";

        NavigationItems = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage),
                IsTabStop = false
            },
            new NavigationViewItem()
            {
                Content = "Orders",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.OrdersPage),
                IsTabStop = false,
                
            },
            new NavigationViewItem()
            {
                Content = "Pending Orders",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Clock24 },
                TargetPageType = typeof(Views.Pages.PendingOrdersPage),
                IsTabStop = false
            },

            new NavigationViewItem()
            {
                Content = "Backup",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Server16 },
                TargetPageType = typeof(Views.Pages.BackupPage),
                IsTabStop = false
            },

            new NavigationViewItem()
            {
                Content = "History",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Clock24 },
                TargetPageType = typeof(Views.Pages.OrderHistoryEditorPage),
                Visibility = System.Windows.Visibility.Collapsed,
                IsTabStop = false

            },

            new NavigationViewItem()
            {
                Content = "Complete Order",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Cart24 },
                TargetPageType = typeof(Views.Pages.CompleteOrderPage),
                Visibility = System.Windows.Visibility.Collapsed,
                IsTabStop = false

            }
        };


        var f = new NavigationViewItem()
        {
            Content = "Retail",
            Icon = new SymbolIcon { Symbol = SymbolRegular.BuildingRetail24 },
            TargetPageType = typeof(Views.Pages.CompleteOrderPage),
            IsTabStop = false
        };

        f.Loaded += (s, e) =>
        {

        };

        NavigationItems.Insert(2, f);

        NavigationFooter = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage),
                IsTabStop = false
            }
        };

        TrayMenuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

        _isInitialized = true;
    }
}
