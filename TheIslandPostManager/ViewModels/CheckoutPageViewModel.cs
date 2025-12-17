using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Drawing.Printing;
using System.IO;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Services;
using TheIslandPostManager.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.ViewModels;

public partial class CheckoutPageViewModel : ObservableObject
{
    private readonly IContentDialogService contentDialogService;
    private readonly IMySQLService mySQLService;
    private readonly INavigationService navigationService;
    private readonly IFileService fileService;
    private readonly IMessageService messageService;
    [ObservableProperty] IOrderService service;
    [ObservableProperty] decimal total;
    [ObservableProperty] bool saleNotComplete = true;
    private bool _managerBypass;

    public CheckoutPageViewModel(IContentDialogService contentDialogService,IMySQLService mySQLService, IOrderService orderService, INavigationService navigationService,IFileService fileService,IMessageService messageService)
    {
        Service = orderService;
        this.mySQLService = mySQLService;
        this.navigationService = navigationService;
        this.fileService = fileService;
        this.messageService = messageService;
        Total = orderService.CurrentOrder.CartTotal;
        this.contentDialogService = contentDialogService;
    }

    [RelayCommand]
    private async Task PaymentAmount(string type)
    {
        var paymentAmountDialog = new PaymentAmountDialog(contentDialogService.GetContentPresenter(),Total);

        ContentDialogResult result = ContentDialogResult.None;

        switch (type)
        {
            case "cash":

                result = await paymentAmountDialog.ShowAsync();
                Service.CurrentOrder.PaymentTransactions.Add(new(Models.PurchaseType.Cash,paymentAmountDialog.Amount));
                break;

            case "card":
                result = await paymentAmountDialog.ShowAsync();
                Service.CurrentOrder.PaymentTransactions.Add(new(Models.PurchaseType.Card, paymentAmountDialog.Amount));
                break;

            case "bank":
                result = await paymentAmountDialog.ShowAsync();
                Service.CurrentOrder.PaymentTransactions.Add(new(Models.PurchaseType.Both, paymentAmountDialog.Amount));
                break;
            default:
                break;
        }

        if(result == ContentDialogResult.Primary)
        {
            Total = paymentAmountDialog.RemainingTotal;
            if(Total <= 0)
            {
                SaleNotComplete = false;
            }
        }
    }

    [RelayCommand]
    private async Task Done()
    {
        if (await CompleteOrder())
        {
            await PrintReciept();
            await Complete();
            navigationService.Navigate(typeof(DashboardPage));
        }

        Service.CurrentDeleteOrder();
    }

    private async Task Complete()
    {
        try
        {
            //if (result)
            //{

                //OrderService.CurrentOrder.Email = OrderService.CurrentOrder.Email?.Trim() ?? string.Empty;
                //OrderService.CurrentOrder.EmployeeID = Employee?.EmployeeID ?? -1;
                //OrderService.CurrentOrder.VideoCount = GetVideoCount();
                await Service.CompleteOrderAsync();

            //foreach (var item in PurchaseItems)
            //{
            //    item.ChangeAmount(0);
            //}
            //}

            if (Service.GetOrderCount() == 0)
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

    private async Task PrintReciept()
    {
        try
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += Pd_PrintPage;
            pd.DefaultPageSettings.PaperSize = new PaperSize("Receipt80mm", 315, 2000);
            pd.DefaultPageSettings.Margins.Left = 1;
            pd.DefaultPageSettings.Margins.Right = 50;

            pd.Print(); // triggers the event
        }
        catch (Exception ex)
        {
            await messageService.ShowMessage("Error", ex.Message);
        }
    }
    private void Pd_PrintPage(object sender, PrintPageEventArgs e)
    {
        string appRoot = AppDomain.CurrentDomain.BaseDirectory;
        string imagesPath = Path.Combine(appRoot, "Data", "images", "logo_b.png");

        POSPrinter a = new POSPrinter(e);
        a.SetFontSize(9);
        a.PrintImage(imagesPath, true);
        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintTextLn("Island Post Receipt", TextAlign.CENTER);
        a.PrintTextLn("Email: islandpostphotos@gmail.com", TextAlign.CENTER);
        a.PrintTextLn("Address: Building N Nassau Cruise Port,", TextAlign.CENTER);
        a.PrintTextLn("1 Prince George Wharf, Nassau", TextAlign.CENTER);
        a.PrintTextLn("Phone: (242) 603-4400", TextAlign.CENTER);

        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintTextLn("VAT INVOICE OR RECEIPT", TextAlign.CENTER);
        a.PrintTextLn("TIN# 118799018", TextAlign.CENTER);
        a.PrintTextLn("");
        a.PrintTextLn($"Order: {Service.CurrentOrder.ID}");
        a.PrintTextLn($"Date: {DateOnly.FromDateTime(DateTime.Now).ToShortDateString()}");
        a.PrintTextLn("");
        a.PrintTextLn("");

        foreach (var item in Service.CurrentOrder.Cart)
        {
            if (item.Amount > 1)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    a.PrintItemWithDotsTruncate(item.Description, $"${item.Cost:F2}");
                }
            }
            else
            {
                a.PrintItemWithDotsTruncate(item.Description, $"${item.Cost:F2}");

            }
        }
        a.PrintTextLn("----------------------------------------------------------------");
        a.PrintTextLn($"Total Payment:         {Service.CurrentOrder.CartTotal}");
        a.PrintTextLn("");
        a.PrintItemWithDotsTruncate("SOLD TO:", Service.CurrentOrder.Email);
        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintBarcode(Service.CurrentOrder.ID.ToString(), TextAlign.CENTER);
        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintTextLn("");
        a.PrintHLine();
        e.HasMorePages = false;
    }

    internal async Task<bool> CompleteOrder()
    {

        // throw new DivideByZeroException();

        try
        {
            Service.CurrentOrder.VideoCount = GetVideoCount();

            await Service.CompleteOrderAsync();

            navigationService.GoBack();

            foreach (var item in Service.CurrentOrder.PurchaseItems)
            {
                item.ChangeAmount(0);
            }

            if (service.GetOrderCount() == 0)
            {
                fileService.PurgeAll();
            }

            return true;

        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Complete Order Error", ex.Message, ex.StackTrace, "952a5653-4a41-4119-81b8-b423faddb787", true);
            navigationService.GoBack();
        }

        return false;
    }

    private int GetVideoCount()
    {
        var video = Service.CurrentOrder.Cart.Where(x => x.Data == "RC3");

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
}
