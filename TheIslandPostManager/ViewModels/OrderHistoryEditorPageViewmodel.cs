using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui;

namespace TheIslandPostManager.ViewModels;
public partial class OrderHistoryEditorPageViewmodel : ObservableObject
{
    private readonly INavigationService navigationService;
    [ObservableProperty] private IOrderService orderService;
    private readonly IEmailService emailService;
    private readonly IMessageService messageService;
    private readonly IMySQLService mySQLService;

    public OrderHistoryEditorPageViewmodel(INavigationService navigationService,IOrderService orderService,IEmailService emailService,IMessageService messageService,IMySQLService mySQLService)
    {
        this.navigationService = navigationService;
        this.orderService = orderService;
        this.emailService = emailService;
        this.messageService = messageService;
        this.mySQLService = mySQLService;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await mySQLService.UpdateHistoryOrder(OrderService.CurrentHistoryOrder);
        navigationService.GoBack();
        OrderService.CurrentHistoryOrder = null;
    }

    [RelayCommand]
    private async Task ResendEmail()
    {
        if(string.IsNullOrWhiteSpace(OrderService.CurrentHistoryOrder.DownloadURL))
        {
            messageService.ShowSnackBarMessage("No Download Link","Download link is needed to send an email.", Wpf.Ui.Controls.ControlAppearance.Primary, Wpf.Ui.Controls.SymbolRegular.Warning24);
            return;
        }
        await emailService.SendEmail(OrderService.CurrentHistoryOrder);
        await mySQLService.UpdateHistoryOrder(OrderService.CurrentHistoryOrder);
    }

    [RelayCommand]
    private void Open(PurchaseHistoryItem imaageObj)
    {
        //if (File.Exists(imaageObj.ImageUrl))
        //{
        //    Process.Start("explorer.exe", "/select, " + imaageObj.ImageUrl);
        //}
    }
}
