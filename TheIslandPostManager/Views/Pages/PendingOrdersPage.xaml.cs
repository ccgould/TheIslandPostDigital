using System.Windows.Controls;
using TheIslandPostManager.ViewModels;

namespace TheIslandPostManager.Views.Pages;

/// <summary>
/// Interaction logic for PendingOrdersPage.xaml
/// </summary>
public partial class PendingOrdersPage : Page
{
    public PendingOrdersPage(PendingOrdersPageViewModel vm)
    {
        InitializeComponent();
        ViewModel = vm;
        DataContext = vm;
    }

    public PendingOrdersPageViewModel ViewModel { get; private set; }
}
