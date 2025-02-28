using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace TheIslandPostManager.Dialogs;

public partial class CompleteOrderDialogViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    private readonly IMySQLService mySQLService;
    private readonly IMessageService messageService;
    private readonly INavigationService navigationService;
    private readonly IContentDialogService contentDialogService;
    private readonly IFileService fileService;
    private IOrderService _orderService;
    [ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;
    [ObservableProperty] private ObservableCollection<PaymentTransaction> paymentTransactions;
    [ObservableProperty] private ObservableCollection<Employee> employees;
    [ObservableProperty] private bool isFlyoutOpen;
    [ObservableProperty] private bool showImageCountOverMessage;
    [ObservableProperty] private bool showPrintCountOverMessage;
    [ObservableProperty] private int printCount;
    [ObservableProperty] private Employee employee;
    [ObservableProperty] private int retailCount;
    private bool _managerBypass;


    public CompleteOrderDialogViewModel(IOrderService orderService, IMySQLService mySQLService, IMessageService messageService,INavigationService navigationService, IContentDialogService contentDialogService,IFileService fileService)
    {
        Employees = mySQLService.GetEmployees().Result;
        PrintCount = orderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount);
        this.orderService = orderService;
        this.mySQLService = mySQLService;
        this.messageService = messageService;
        this.navigationService = navigationService;
        this.contentDialogService = contentDialogService;
        this.fileService = fileService;
        _orderService = orderService;
    }

    [RelayCommand]
    private async Task ItemClick(PurchaseItem purchaseItem)
    {
        if (purchaseItem.IsRetailItem && purchaseItem.HasChildren()) 
        {
            var itemsDialog = new RetailItemsListPopup(contentDialogService.GetDialogHost(),purchaseItem.ChildrenItems);
            var dialogResult = await itemsDialog.ShowAsync();

            if(dialogResult == ContentDialogResult.None)
            {
                return;
            }
            else
            {
                purchaseItem.IncrementAmount(itemsDialog.SelectedItem);
                OrderService.CurrentOrder.AddItemToCart(itemsDialog.SelectedItem);
            }
        }
        else
        {
            purchaseItem.IncrementAmount();
            OrderService.CurrentOrder.AddItemToCart(purchaseItem);
        }

        RetailCount = OrderService.CurrentOrder.Cart.Count(x=>x.IsRetailItem);
    }

    [RelayCommand]
    private async Task TransactionClick(string inputString)
    {
        PurchaseType inputAsEnum = (PurchaseType)Enum.Parse(typeof(PurchaseType), inputString, true);

        //Check if payment is complete.



        var itemsDialog = new PaymentTransacationDialog(contentDialogService.GetDialogHost(),3000);
        var dialogResult = await itemsDialog.ShowAsync();

        if (dialogResult == ContentDialogResult.None)
        {
            return;
        }
        else
        {
            //purchaseItem.IncrementAmount(itemsDialog.SelectedItem);
            //OrderService.CurrentOrder.AddItemToCart(itemsDialog.SelectedItem);
        }




        // paymentTransactions.Add(new PaymentTransaction(inputAsEnum))

        //if (purchaseItem.IsRetailItem && purchaseItem.HasChildren())
        //{
        //    var itemsDialog = new RetailItemsListPopup(contentDialogService.GetDialogHost(), purchaseItem.ChildrenItems);
        //    var dialogResult = await itemsDialog.ShowAsync();

        //    if (dialogResult == ContentDialogResult.None)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        purchaseItem.IncrementAmount(itemsDialog.SelectedItem);
        //        OrderService.CurrentOrder.AddItemToCart(itemsDialog.SelectedItem);
        //    }
        //}
        //else
        //{
        //    purchaseItem.IncrementAmount();
        //    OrderService.CurrentOrder.AddItemToCart(purchaseItem);
        //}

        //RetailCount = OrderService.CurrentOrder.Cart.Count(x => x.IsRetailItem);
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

        if(OrderService.CurrentOrder.SelectedPurchaseType == PurchaseType.None)
        {
            await messageService.ShowMessage("Cash Type Not Choosen", "Please select if the transaction is Card, Cash or Both", "OK", ControlAppearance.Primary, false);
            return false;
        }

        if (Employee is null)
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

        if (!OrderService.CurrentOrder.Cart.Any())
        {
            await messageService.ShowMessage("Order cost not entered.", "Please select the package/s that best suit this order.", "OK", ControlAppearance.Primary, false);
            return false;
        }

        if(OrderService.CurrentOrder.Cart.Any(x => x.IsRetailItem == false))
        {
            if (OrderService.CurrentOrder.ApprovedImagesCount <= 0)
            {
                messageService.ShowSnackBarMessage("Attention", "No images selected. Please select a photo before attempting to complete this order", ControlAppearance.Caution, SymbolRegular.Stop20);
                return false;
            }

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

            if (string.IsNullOrWhiteSpace(OrderService.CurrentOrder.Name) && OrderService.CurrentOrder.Cart.Any(x => x.IsRetailItem == false))
            {
                var result = await messageService.ShowMessage("No Name Provided", "Please check name field", "OK");

                if (result == MessageBoxResult.None)
                {
                    return false;
                }
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

        if (purchaseItem.IsChild())
        {
            var parent = purchaseItems.FirstOrDefault(x => x.ID == purchaseItem.Parent);
            parent?.ChangeAmount(-purchaseItem.Amount);
        }
        else
        {
            purchaseItem.ChangeAmount(0);
        }

        OrderService.CurrentOrder.UpdateCartTotal();
    }

    [RelayCommand]
    internal async Task CompleteOrder()
    {

       // throw new DivideByZeroException();

        try
        {      
            var result = await CheckConditons();

            if (result)
            {
                OrderService.CurrentOrder.Name = OrderService.CurrentOrder.Name?.Trim() ?? string.Empty;
                OrderService.CurrentOrder.Email = OrderService.CurrentOrder.Email?.Trim() ?? string.Empty;
                OrderService.CurrentOrder.EmployeeID = Employee?.EmployeeID ?? -1;
                OrderService.CurrentOrder.VideoCount = GetVideoCount();
                await OrderService.CompleteOrderAsync();
                navigationService.GoBack();


                foreach (var item in PurchaseItems)
                {
                    item.ChangeAmount(0);
                }
            }

            if(OrderService.GetOrderCount() == 0)
            {
                fileService.PurgeAll();
            }

        }
        catch (Exception ex)
        {
           await messageService.ShowErrorMessage("Complete Order Error", ex.Message, ex.StackTrace, "952a5653-4a41-4119-81b8-b423faddb787", true);
            navigationService.GoBack();
        }
    }

    private int GetVideoCount()
    {
      var video =    OrderService.CurrentOrder.Cart.Where(x => x.Data == "RC3");

        int amount = 0;
        if(video is not null)
        {
            foreach (var item in video)
            {
                amount += item.Amount;
            }
        }

        return amount;
    }

    [RelayCommand]
    internal void CancelOrder()
    {
        var result = messageService.ShowMessage("Go Back", "Are you sure you would like to go to the dashboard?.", "NO", ControlAppearance.Primary, true);

        if (result.Result == MessageBoxResult.Primary)
        {
            navigationService.GoBack();
            _orderService.CurrentOrder.IsCompleteingOrder = false;
        }



    }

    [RelayCommand]
    internal async Task Override()
    {
        try
        {
            var result = messageService.ShowMessage("Authorization needed", "Sorry, you can't perform this action. Please have your manager assist you. Would you like to continue?", "NO", ControlAppearance.Primary, true);

            if (result.Result == MessageBoxResult.Primary)
            {
                if (contentDialogService is not null)
                {
                    var dialog = new OverrideDialog(contentDialogService.GetDialogHost(), mySQLService);
                    var dialogResult = await dialog.ShowAsync();

                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        if (mySQLService.ValidatePin(dialog.SelectedItem.EmployeeID, dialog.Link).Result)
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
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Complete Order Error", ex.Message, ex.StackTrace, "4acdb1dc-e0cd-4a23-804c-4308f709cbcc", true);
        }
    }

    internal void GetCartItems()
    {
        PurchaseItems = mySQLService.GetProducts(App.IsRetailPage)?.Result;


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
}
