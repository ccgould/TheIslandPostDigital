using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.ViewModels;

public partial class PendingOrdersPageViewModel : ObservableObject
{
   [ObservableProperty] private IOrderService orderService;
   [ObservableProperty] private bool isBusy;

    public PendingOrdersPageViewModel(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    [RelayCommand]
    private async Task Delete(Order order)
    {
       await OrderService.DeletePendingOrder(order);
    }

    [RelayCommand]
    private async Task OpenOrderFromPending(Order order)
    {
        await OrderService.OpenOrderFromPending(order);
    }

    [RelayCommand]
    private async Task RetrievePendingOrders()
    {
        IsBusy = true;
        await OrderService.GetPendingOrders();
        IsBusy = false;
    }
}
