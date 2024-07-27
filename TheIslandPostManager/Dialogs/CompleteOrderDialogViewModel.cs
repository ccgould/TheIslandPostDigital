using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mysqlx.Crud;
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.Dialogs;

public partial class CompleteOrderDialogViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    [ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;
    [ObservableProperty] private bool isFlyoutOpen;

    public CompleteOrderDialogViewModel(IOrderService orderService)
    {
        PurchaseItems =
        [
            new PurchaseItem("4 Prints for $15 Deal", "RC2", (decimal)13.64),
            new PurchaseItem("Printed Photo", "RC2", (decimal)4.54),
            new PurchaseItem("5 Prints + 5 Digital Deal", "RC2", (decimal)18.18),
            new PurchaseItem("8 Prints + 10 Digital Deal", "RC2", (decimal)36.36),
            new PurchaseItem("Photo Slide Show", "RC2", (decimal)13.64),
            new PurchaseItem("Video Postcard", "RC2", (decimal)9.09),
        ];
        this.orderService = orderService;
    }

    [RelayCommand]
    private void ItemClick(PurchaseItem purchaseItem)
    {
        purchaseItem.IncrementAmount();
        OrderService.CurrentOrder.AddItemToCart(purchaseItem);
    }

    [RelayCommand]
    private void ChangeQuantity(PurchaseItem purchaseItem)
    {
        purchaseItem.OpenFlyout();
    }

    [RelayCommand]
    private void Delete(PurchaseItem purchaseItem)
    {
        OrderService.CurrentOrder.Cart.Remove(purchaseItem);
        purchaseItem.ChangeAmount(0);
        OrderService.CurrentOrder.UpdateCartTotal();
    }

    internal async Task CompleteOrder()
    {
        await OrderService.CompleteOrderAsync();
    }
}
