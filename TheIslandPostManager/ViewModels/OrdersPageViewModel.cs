using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;
using Wpf.Ui;


namespace TheIslandPostManager.ViewModels;
public partial class OrdersPageViewModel : ObservableObject
{
    [ObservableProperty ]private IOrderService orderService;
    private readonly IEmailService emailService;
    private readonly IContentDialogService contentDialogService;
    private readonly IMySQLService mySQLService;
    [ObservableProperty] private string searchText;
    [ObservableProperty] private DateTime dateTime1;
    [ObservableProperty] private DateTime dateTime2;

    public OrdersPageViewModel(IOrderService orderService,IEmailService emailService, IContentDialogService contentDialogService,IMySQLService mySQLService)
    {
        this.orderService = orderService;
        this.emailService = emailService;
        this.contentDialogService = contentDialogService;
        this.mySQLService = mySQLService;
        DateTime1 = DateTime.Now;
        DateTime2 = DateTime.Now;
    }
    [RelayCommand]
    private async Task SendEmail(Order order)
    {

        var emailLinkDialog = new EmailLinkRequestDialog(contentDialogService.GetContentPresenter());

        var result = await emailLinkDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            order.DownloadURL = emailLinkDialog.Link;

            if (await emailService.SendEmail(order))
            {

            }
            else
            {

            }
        }        
    }

    [RelayCommand]
    private void Search()
    {
        OrderService.PurchaseHistory = mySQLService.GetPurchaseHistory(DateTime1, DateTime2, SearchText, "");
    }
}
