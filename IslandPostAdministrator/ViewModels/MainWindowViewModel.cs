using System.Collections.ObjectModel;
using System.Reflection;
using Wpf.Ui.Controls;
using Wpf.Ui;
using CommunityToolkit.Mvvm.ComponentModel;
using IslandPostAdministrator.Views.Pages;
using IslandPostAdministrator.Models;

namespace IslandPostAdministrator.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _applicationTitle = String.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = new();

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = new();

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new();

    public MainWindowViewModel(INavigationService navigationService)
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = $"Island Post Digitial - Photography Department V{Assembly.GetExecutingAssembly().GetName().Version}";

        NavigationItems = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Name = "Home",
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(DashboardPage),
                IsTabStop = false
            },
            new NavigationViewItemSeparator(),

              new NavigationViewItem()
            {
                Name = "Employees",
                Content = "Employees",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                IsTabStop = false
            },           


        };




        var employees = new List<Employee>
        {
            new Employee
            {
                Name = "Creswell"
            },
              new Employee
            {
                Name = "Brandon"
            },
             new Employee
            {
                Name = "Adwele"
            },
             new Employee
            {
                Name = "Jordan"
            },
        };

        NavigationViewItem item = NavigationItems[2] as NavigationViewItem;
        var employeesList = new List<NavigationViewItem>();

        foreach (var employee in employees)
        {

            employeesList.Add(new NavigationViewItem()
            {
                Name = employee.Name,
                Content = employee.Name,
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(EmployeePage),
                IsTabStop = false
            });
        }

        item.MenuItemsSource = employeesList.ToArray();

       var f =  new NavigationViewItem()
        {
            Name = "Home",
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(DashboardPage),
            IsTabStop = false
        };

        //f.

NavigationFooter = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage),
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