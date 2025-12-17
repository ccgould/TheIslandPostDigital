using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;
using Wpf.Ui;
using TheIslandPostManager.Views.Pages;
using System.IO;
using System.Diagnostics;


namespace TheIslandPostManager.ViewModels;
public partial class OrdersPageViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    private readonly IEmailService emailService;
    private readonly IContentDialogService contentDialogService;
    private readonly IMySQLService mySQLService;
    private readonly INavigationService navigationService;
    private readonly IMessageService messageService;
    [ObservableProperty] private string searchText;
    [ObservableProperty] private DateTime dateTime1;
    [ObservableProperty] private DateTime dateTime2;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private Order order;


    public OrdersPageViewModel(IOrderService orderService,IEmailService emailService, IContentDialogService contentDialogService,IMySQLService mySQLService,INavigationService navigationService,IMessageService messageService)
    {
        this.orderService = orderService;
        this.emailService = emailService;
        this.contentDialogService = contentDialogService;
        this.mySQLService = mySQLService;
        this.navigationService = navigationService;
        this.messageService = messageService;
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
    internal void SelectedHistory(Order order)
    {
        Order = order;
    }

    [RelayCommand]
    private void Open(ImageObj imageObj)
    {
        if (File.Exists(imageObj.ImageUrl))
        {
            Process.Start("explorer.exe", "/select, " + imageObj.ImageUrl);
        }
        else
        {
            messageService.ShowSnackBarMessage("File Not Found", $"Image {imageObj.Name} is not found at {imageObj.ImageUrl}.", ControlAppearance.Danger, SymbolRegular.ThumbDislike24);
        }
    }

    [RelayCommand]
    private async Task Delete(Order order)
    {
        if(await messageService.ShowMessage(@"Delete Order", $"Are you sure you would like to delete this order. Order [{order.ID}]?","NO",ControlAppearance.Secondary,true) == MessageBoxResult.Primary)
        {
            if(await mySQLService.DeleteOrderByID(order.ID))
            {
                OrderService.DeleteOrder(order);
            }
        }
    }
}
