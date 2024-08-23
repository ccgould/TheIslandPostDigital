using System.Windows.Controls;
using TheIslandPostManager.Services;
using TheIslandPostManager.ViewModels;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;

/// <summary>
/// Interaction logic for PendingOrdersPage.xaml
/// </summary>
public partial class PendingOrdersPage : INavigableView<PendingOrdersPageViewModel>
{
    public PendingOrdersPage(PendingOrdersPageViewModel vm, IOrderService orderService)
    {
        InitializeComponent();
        ViewModel = vm;
        DataContext = vm;
        orderService.GetPendingOrders();
    }

    public PendingOrdersPageViewModel ViewModel { get; private set; }


}
