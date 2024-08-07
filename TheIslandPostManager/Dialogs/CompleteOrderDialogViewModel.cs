using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.Dialogs;

public partial class CompleteOrderDialogViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    [ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;
    [ObservableProperty] private ObservableCollection<Employee> employees;
    [ObservableProperty] private Employee selected;
    [ObservableProperty] private bool isFlyoutOpen;
    [ObservableProperty] private bool showImageCountOverMessage;
    [ObservableProperty] private bool showPrintCountOverMessage;

    public CompleteOrderDialogViewModel(IOrderService orderService,IMySQLService mySQLService)
    {
        PurchaseItems =
        [
            new PurchaseItem("4 Prints for $15 Deal", "RC2", (decimal)13.64,4),
            new PurchaseItem("Printed Photo", "RC2", (decimal)4.54,1),
            new PurchaseItem("5 Prints + 5 Digital Deal", "RC2", (decimal)18.18,5),
            new PurchaseItem("8 Prints + 10 Digital Deal", "RC2", (decimal)36.36,10,8),
            new PurchaseItem("Photo Slide Show", "RC2", (decimal)13.64,6),
            new PurchaseItem("Video Postcard", "RC2", (decimal)9.09,1),
        ];

        this.orderService = orderService;

        Employees = mySQLService.GetEmployees();
    }

    [RelayCommand]
    private void ItemClick(PurchaseItem purchaseItem)
    {
        purchaseItem.IncrementAmount();
        OrderService.CurrentOrder.AddItemToCart(purchaseItem);
        CheckConditons();
    }

    internal bool CheckConditons()
    {
        var imageCount = 0;
        var printCount = 0;

        foreach (var item in OrderService.CurrentOrder.Cart)
        {
            imageCount += (item.ImageCount * item.Amount);
        }

        foreach (var item in OrderService.CurrentOrder.Cart)
        {
            printCount += (item.PrintCount * item.Amount);
        }

        ShowImageCountOverMessage = imageCount > OrderService.CurrentOrder.ApprovedImagesCount;
        ShowPrintCountOverMessage = printCount > OrderService.CurrentOrder.ApprovedPrintsCount;

        return !ShowImageCountOverMessage && !ShowPrintCountOverMessage;
    }


    [RelayCommand]
    private void ChangeQuantity(PurchaseItem purchaseItem)
    {
        purchaseItem.OpenFlyout();
        CheckConditons();
    }

    [RelayCommand]
    private void Delete(PurchaseItem purchaseItem)
    {
        OrderService.CurrentOrder.Cart.Remove(purchaseItem);
        purchaseItem.ChangeAmount(0);
        OrderService.CurrentOrder.UpdateCartTotal();
        CheckConditons();
    }

    internal async Task CompleteOrder()
    {
        OrderService.CurrentOrder.EmployeeID = Selected.EmployeeID;
        await OrderService.CompleteOrderAsync();
    }
}
