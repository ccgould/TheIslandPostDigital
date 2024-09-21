using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;
using Wpf.Ui;
using TheIslandPostManager.Views.Pages;


namespace TheIslandPostManager.ViewModels;
public partial class OrdersPageViewModel : ObservableObject
{
    [ObservableProperty ]private IOrderService orderService;
    private readonly IEmailService emailService;
    private readonly IContentDialogService contentDialogService;
    private readonly IMySQLService mySQLService;
    private readonly INavigationService navigationService;
    [ObservableProperty] private string searchText;
    [ObservableProperty] private DateTime dateTime1;
    [ObservableProperty] private DateTime dateTime2;
    [ObservableProperty] private bool isBusy;


    public OrdersPageViewModel(IOrderService orderService,IEmailService emailService, IContentDialogService contentDialogService,IMySQLService mySQLService,INavigationService navigationService)
    {
        this.orderService = orderService;
        this.emailService = emailService;
        this.contentDialogService = contentDialogService;
        this.mySQLService = mySQLService;
        this.navigationService = navigationService;
        DateTime1 = DateTime.Now;
        DateTime2 = DateTime.Now;
    }
    [RelayCommand]
    private async Task SendEmail(Order order)
    {

        var emailLinkDialog = new EmailLinkRequestDialog(contentDialogService.GetContentPresenter(),emailService,mySQLService,order);

        var result = await emailLinkDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            order.DownloadURL = emailLinkDialog.Link;

            if (await emailService.SendEmail(order))
            {
                await mySQLService.UpdateHistoryOrder(order);
            }
            else
            {

            }
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        IsBusy = true;
        OrderService.PurchaseHistory = await mySQLService.GetPurchaseHistory(DateTime1, DateTime2, SearchText, "");
        IsBusy = false;
    }

    [RelayCommand]
    private void OpenHistory(Order order)
    {
        OrderService.CurrentHistoryOrder = order;
        navigationService.Navigate(typeof(OrderHistoryEditorPage));
    }
}
