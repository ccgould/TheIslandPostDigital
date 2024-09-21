using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.ViewModels;
public partial class CustomerWindowViewmodel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    [ObservableProperty] private int currentPosition;

    public CustomerWindowViewmodel(IOrderService orderService)
    {
        this.orderService = orderService;
        orderService.CurrentOrder.OrderCollectionView.CurrentChanged += delegate
        {
            if(orderService.CurrentOrder.OrderCollectionView.CurrentItem is not null)
            {
                CurrentPosition = ((ImageObj)orderService.CurrentOrder.OrderCollectionView.CurrentItem).Index;
            }
        };
    }

    public int GetPrintCount()
    {
        return orderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount);
    }

    [RelayCommand]
    private void PreviousPhoto()
    {
        OrderService.CurrentOrder.PreviousPhoto();
    }

    [RelayCommand]
    private void NextPhoto()
    {
        OrderService.CurrentOrder.NextPhoto();
    }

    [RelayCommand]
    private void AttemptDislikePhoto()
    {
        OrderService.CurrentOrder.MaybeImage();
    }
}
