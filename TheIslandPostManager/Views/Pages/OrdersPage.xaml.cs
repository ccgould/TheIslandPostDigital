using TheIslandPostManager.Models;
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

        CalendarDatePicker.Click += CalendarDatePicker_Click;
    }

    private void CalendarDatePicker_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var listView = (ListView)sender;
        ViewModel.SelectedHistory((Order)listView.SelectedValue);
    }
}
