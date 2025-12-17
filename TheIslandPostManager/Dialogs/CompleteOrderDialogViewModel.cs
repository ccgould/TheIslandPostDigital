using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Syncfusion.Windows.Controls.Input;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Controls;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using TheIslandPostManager.Views.Pages;
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
    private readonly IFileService fileService;
    private readonly POSPrinter printer;
    private IOrderService _orderService;
    [ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;
    [ObservableProperty] private ObservableCollection<Employee> employees;
    [ObservableProperty] private ObservableCollection<Customer> customers;
    [ObservableProperty] private bool isFlyoutOpen;
    [ObservableProperty] private bool showImageCountOverMessage;
    [ObservableProperty] private bool showPrintCountOverMessage;
    [ObservableProperty] private int printCount;
    [ObservableProperty] private Employee employee;
    [ObservableProperty] private int retailCount;
    private bool _managerBypass;

    public CompleteOrderDialogViewModel(IOrderService orderService, IMySQLService mySQLService, IMessageService messageService, INavigationService navigationService, IContentDialogService contentDialogService, IFileService fileService)
    {
        Employees = mySQLService.GetEmployees().Result;
        PrintCount = orderService.CurrentOrder.ApprovedPrints.Sum(x => x.PrintAmount);
        this.orderService = orderService;
        this.mySQLService = mySQLService;
        this.messageService = messageService;
        this.navigationService = navigationService;
        this.contentDialogService = contentDialogService;
        this.fileService = fileService;
        this.printer = printer;
        _orderService = orderService;
    }

    [RelayCommand]
    private async Task ItemClick(PurchaseItem purchaseItem)
    {
        if (purchaseItem.IsRetailItem && purchaseItem.HasChildren())
        {
            var itemsDialog = new RetailItemsListPopup(contentDialogService.GetDialogHost(), purchaseItem.ChildrenItems);
            var dialogResult = await itemsDialog.ShowAsync();

            if (dialogResult == ContentDialogResult.None)
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

        RetailCount = OrderService.CurrentOrder.Cart.Count(x => x.IsRetailItem);
    }

    [RelayCommand]
    private async Task TransactionClick(string inputString)
    {
        PurchaseType inputAsEnum = (PurchaseType)Enum.Parse(typeof(PurchaseType), inputString, true);

        //Check if payment is complete.

        var itemsDialog = new PaymentTransacationDialog(contentDialogService.GetDialogHost(), OrderService.CurrentOrder.CartTotal / 0.01m);
        var dialogResult = await itemsDialog.ShowAsync();

        if (dialogResult == ContentDialogResult.None)
        {
            return;
        }
        else
        {
            OrderService.CurrentOrder.PaymentTransactions.Add(new PaymentTransaction(inputAsEnum, itemsDialog.GetBalanceAsDecimal()));

            var totalAmount = itemsDialog.TotalAmount;
            var change = itemsDialog.Change;
            var balance = itemsDialog.Balance;
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

    private void DeletePaymentTransaction(PaymentTransaction transaction)
    {
        OrderService.CurrentOrder.PaymentTransactions.Remove(transaction);
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

        //if(OrderService.CurrentOrder.SelectedPurchaseType == PurchaseType.None)
        //{
        //    await messageService.ShowMessage("Cash Type Not Choosen", "Please select if the transaction is Card, Cash or Both", "OK", ControlAppearance.Primary, false);
        //    return false;
        //}

        //if (Employee is null)
        //{
        //    await messageService.ShowMessage("Cashier Not Choosen", "Please select a cashier.", "OK", ControlAppearance.Primary, false);
        //    return false;
        //}

        if (OrderService.CurrentOrder is null) return false;

        if (OrderService.CurrentOrder.ApprovedPrints.Any() && OrderService.CurrentOrder.Cart.Sum(x => x.ImageCount) == 0)
        {
            await messageService.ShowMessage("No Photo Package Selected.", $"Photos are selected but no photo package is selected please select appropriate package and try again");
            return false;
        }

        foreach (var item in OrderService.CurrentOrder.Cart)  ///This is not checking for the selected images which is allowing you to print without selecting a photo package
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

        if (OrderService.CurrentOrder.Cart.Any(x => x.IsRetailItem == false))
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

            if (string.IsNullOrWhiteSpace(OrderService.CurrentOrder.Email) && OrderService.CurrentOrder.Cart.Any(x => x.ImageCount > 0))
            {
                messageService.ShowSnackBarMessage("Attention", "No email provided. Please provide an email to complete this order.", ControlAppearance.Caution, SymbolRegular.Stop20);
                return false;
            }

            //if (string.IsNullOrWhiteSpace(OrderService.CurrentOrder.Name) && OrderService.CurrentOrder.Cart.Any(x => x.IsRetailItem == false))
            //{
            //    var result = await messageService.ShowMessage("No Name Provided", "Please check name field", "OK");

            //    if (result == MessageBoxResult.None)
            //    {
            //        return false;
            //    }
            //}
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


    private int GetVideoCount()
    {
        var video = OrderService.CurrentOrder.Cart.Where(x => x.Data == "RC3");

        int amount = 0;
        if (video is not null)
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

    [RelayCommand]
    private async Task Pay()
    {
        if (await CheckConditons())
        {
            OrderService.CurrentOrder.EmployeeID = Employee?.EmployeeID ?? -1;
            OrderService.CurrentOrder.PurchaseItems = purchaseItems;
            navigationService.Navigate(typeof(CheckOutPage));
        }
    }

    static void StatusChanged(object sender, EventArgs ps)
    {
        var status = (PrinterStatusEventArgs)ps;
        Console.WriteLine($"Status: {status.IsPrinterOnline}");
        Console.WriteLine($"Has Paper? {status.IsPaperOut}");
        Console.WriteLine($"Paper Running Low? {status.IsPaperLow}");
        Console.WriteLine($"Cash Drawer Open? {status.IsCashDrawerOpen}");
        Console.WriteLine($"Cover Open? {status.IsCoverOpen}");
    }
}
