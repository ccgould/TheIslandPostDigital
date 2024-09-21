using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;

public partial class CompleteOrderDialogViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    private readonly IMySQLService mySQLService;
    private readonly IMessageService messageService;
    private readonly INavigationService navigationService;
    private readonly IContentDialogService contentDialogService;
    [ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;
    [ObservableProperty] private ObservableCollection<Employee> employees;
    [ObservableProperty] private bool isFlyoutOpen;
    [ObservableProperty] private bool showImageCountOverMessage;
    [ObservableProperty] private bool showPrintCountOverMessage;
    [ObservableProperty] private int printCount;
    [ObservableProperty] private Employee employee;
    private bool _managerBypass;

    public CompleteOrderDialogViewModel(IOrderService orderService, IMySQLService mySQLService, IMessageService messageService,INavigationService navigationService, IContentDialogService contentDialogService)
    {
        Employees = mySQLService.GetEmployees().Result;
        PrintCount = orderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount);
        this.orderService = orderService;
        this.mySQLService = mySQLService;
        this.messageService = messageService;
        this.navigationService = navigationService;
        this.contentDialogService = contentDialogService;
        PurchaseItems = mySQLService.GetStoreItems()?.Result;
        var pages = navigationService.GetNavigationControl();

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
        if (_managerBypass)
        {
            _managerBypass = false;
            return true;
        }

        var imageCount = 0;
        var printCount = 0;

        if(Employee is null)
        {
            await messageService.ShowMessage("Cashier Not Choosen", "Please select a cashier.", "OK", ControlAppearance.Primary, false);
            return false;
        }

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

        var printSum = OrderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount);

        if (printCount < printSum)
        {
            await messageService.ShowMessage("Incorrect Amounts", $"Please check print count and try again");
            return false;
        }

        if (printCount > printSum && printSum != 0)
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

            OrderService.CurrentOrder.IsFinalized = true;
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
                OrderService.CurrentOrder.Name = OrderService.CurrentOrder.Name?.Trim() ?? string.Empty;
                OrderService.CurrentOrder.Email = OrderService.CurrentOrder.Email?.Trim() ?? string.Empty;
                OrderService.CurrentOrder.EmployeeID = Employee?.EmployeeID ?? -1;
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

    [RelayCommand]
    internal async Task Override()
    {
        var result = messageService.ShowMessage("Authorization needed", "Sorry, you can't perform this action. Please have your manager assist you. Would you like to continue?", "NO", ControlAppearance.Primary, true);

        if (result.Result == MessageBoxResult.Primary)
        {
            if(contentDialogService is not null)
            {
                var dialog = new OverrideDialog(contentDialogService.GetDialogHost(),mySQLService);
                var dialogResult = await dialog.ShowAsync();
                
                if (dialogResult == ContentDialogResult.Primary)
                {
                    if(mySQLService.ValidatePin(dialog.SelectedItem.EmployeeID, dialog.Link).Result)
                    {
                        _managerBypass = true;
                        await CompleteOrder();
                    }
                    else
                    {
                        await messageService.ShowErrorMessage("Invalid Pin", "Incorrect Authorization PIN. Please try again.");
                    }
                }
            }
        }
    }
}
