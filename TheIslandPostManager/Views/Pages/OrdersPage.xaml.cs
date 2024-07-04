using TheIslandPostManager.ViewModels;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for OrdersPage.xaml
/// </summary>
public partial class OrdersPage : INavigableView<OrdersPageViewModel>
{
    public OrdersPageViewModel ViewModel { get; }

    public OrdersPage(OrdersPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
