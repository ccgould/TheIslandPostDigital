using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;


namespace TheIslandPostManager.ViewModels;
public partial class OrdersPageViewModel : ObservableObject
{
    [ObservableProperty ]private IOrderService orderService;

    public OrdersPageViewModel(IOrderService orderService)
    {
        this.orderService = orderService;
    }
}
