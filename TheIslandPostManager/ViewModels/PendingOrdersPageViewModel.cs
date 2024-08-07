using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.ViewModels;

public partial class PendingOrdersPageViewModel : ObservableObject
{
   [ObservableProperty] private IOrderService orderService;

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
}
