using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TheIslandPostManager.Dialogs;

public partial class CompleteOrderDialogViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    private readonly IMessageService messageService;
    private readonly INavigationService navigationService;
    [ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;
    [ObservableProperty] private ObservableCollection<Employee> employees;
    [ObservableProperty] private bool isFlyoutOpen;
    [ObservableProperty] private bool showImageCountOverMessage;
    [ObservableProperty] private bool showPrintCountOverMessage;
    [ObservableProperty] private int printCount;


    public CompleteOrderDialogViewModel(IOrderService orderService, IMySQLService mySQLService, IMessageService messageService,INavigationService navigationService)
    {
        Employees = mySQLService.GetEmployees().Result;
        PrintCount = orderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount);
        this.orderService = orderService;
        this.messageService = messageService;
        this.navigationService = navigationService;
        PurchaseItems = mySQLService.GetStoreItems().Result;

        if (orderService.CurrentOrder.Cart.Any())
        {
            foreach (var item in orderService.CurrentOrder.Cart)
            {
                foreach (var item2 in purchaseItems)
                {
                    if (item.ID == item2.ID)
                    {
                        item2.Copy(item);
                        continue;
                    }
                }
            }
        }
    }

    [RelayCommand]
    private void ItemClick(PurchaseItem purchaseItem)
    {
        purchaseItem.IncrementAmount();
        OrderService.CurrentOrder.AddItemToCart(purchaseItem);
    }

    internal async Task<bool> CheckConditons()
    {
        var imageCount = 0;
        var printCount = 0;

        if (OrderService.CurrentOrder is null) return false;

        foreach (var item in OrderService.CurrentOrder.Cart)
        {
            imageCount += (item.ImageCount * item.Amount);
        }

        foreach (var item in OrderService.CurrentOrder.Cart)
        {
            printCount += item.PrintCount * item.Amount;
        }

        //ShowImageCountOverMessage = imageCount > OrderService.CurrentOrder.ApprovedImagesCount;
        if (!OrderService.CurrentOrder.Cart.Any())
        {
            await messageService.ShowMessage("Order cost not entered.", "Please select the package/s that best suit this order.", "OK", ControlAppearance.Primary, false);
            return false;
        }

        //if (imageCount < OrderService.CurrentOrder.ApprovedImagesCount)
        //{
        //    await messageService.ShowMessage("Incorrect Amounts", $"Please check image count and retry.");
        //    return false;
        //}


        if (printCount < OrderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount))
        {
            await messageService.ShowMessage("Incorrect Amounts", $"Please check print count and try again");
            return false;
        }

        if (string.IsNullOrWhiteSpace(OrderService.CurrentOrder.Name))
        {
            var result = await messageService.ShowMessage("No Name Provided", "Please check name field", "OK");

            if (result == MessageBoxResult.None)
            {
                return false;
            }
        }

        if (string.IsNullOrWhiteSpace(OrderService.CurrentOrder.Email))
        {
            var result = await messageService.ShowMessage("No Email Provided", "There isnt any email provided, this will result in no digitals sent. Do you want to continue?", "NO", ControlAppearance.Primary, true);

            if (result == MessageBoxResult.None)
            {
                return false;
            }
        }

        if(OrderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount) == 0)
        {
            var result = await messageService.ShowMessage("No prints", "There arent any prints selected, this will result in no prints. Do you want to continue?", "NO", ControlAppearance.Primary, true);

            if (result == MessageBoxResult.None)
            {
                return false;
            }
        }

        return true;
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

    [RelayCommand]
    internal async Task CompleteOrder()
    {
        try
        {      
            var result = await CheckConditons();
            if (result)
            {
                OrderService.CurrentOrder.EmployeeID = OrderService.CurrentOrder.Employee.EmployeeID;
                await OrderService.CompleteOrderAsync();
                navigationService.GoBack();
                foreach (var item in PurchaseItems)
                {
                    item.ChangeAmount(0);
                }
            }

        }
        catch (Exception ex)
        {
           await messageService.ShowErrorMessage("Complete Order Error", ex.Message, ex.StackTrace, "952a5653-4a41-4119-81b8-b423faddb787", true);
            navigationService.GoBack();
        }
    }

    [RelayCommand]
    internal void CancelOrder()
    {
        var result = messageService.ShowMessage("Go Back", "Are you sure you would like to go to the dashboard?.", "NO", ControlAppearance.Primary, true);

        if (result.Result == MessageBoxResult.Primary)
        {
            navigationService.GoBack();
        }



    }
}
