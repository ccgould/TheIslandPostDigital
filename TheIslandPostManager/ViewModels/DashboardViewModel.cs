using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using TheIslandPostManager.Windows;
using Wpf.Ui;
using Order = TheIslandPostManager.Models.Order;

namespace TheIslandPostManager.ViewModels;
public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    [ObservableProperty] private IFileService fileService;
    [ObservableProperty] private IImageService imageService;
    private readonly IMessageService messageService;
    private readonly IContentDialogService contentDialogService;

    public DashboardViewModel(IOrderService orderService,IFileService fileService,IImageService imageService,IMessageService messageService, IContentDialogService contentDialogService)
    {
        OrderService = orderService;
        FileService = fileService;
        ImageService = imageService;
        this.messageService = messageService;
        this.contentDialogService = contentDialogService;
        OrderService.CreateOrder();
    }

    public async Task Open()
    {
        await ImageService.OpenImageDialogBrowser();
    }

    [RelayCommand]
    private void CancelOrder(Order order)
    {
        OrderService.Cancel(order);
    }

    public async Task CreateOrder()
    {

        var result = await messageService.ShowMessage("Copy Images", "Would you like to copy the images from the current order to this new ?","Cancel",Wpf.Ui.Controls.ControlAppearance.Primary,true,"Yes",true,"No");

        if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            await OrderService.CreateOrder(true);
        }

        if(result == Wpf.Ui.Controls.MessageBoxResult.Secondary)
        {
            await OrderService.CreateOrder(false);
        }
    }

    internal async Task CancelAllOrders()
    {
        await OrderService.CancelAll();
    }

    [RelayCommand]
    private void SelectedItemChanged(Order obj)
    {
        OrderService.UpdateOrder(obj);
        ImageService.SetOrder(obj);
        ImageService.UpdateOrder(obj);
    }


    [RelayCommand]
    private void ImageClick(Image image)
    {
        //OrderService.CurrentOrder.currentImage
        //Service.SelectedImage = image;
        //Service.SelectedImage.HDImage = image.Image;
       // _ = _navigationService.Navigate(typeof(ImageViewer));
        //Service.SelectedImage.HDImage = await Service.LoadHDImage(image.ImageUrl);
    }

    [RelayCommand]
    private void SelectImageClick(Image image)
    {
        image.IsSelected = !image.IsSelected;
        OrderService.AddImageToOrder(image);
    }

    [RelayCommand]
    private void DeleteImageClick(Image image)
    {
        ImageService.DeleteImage(image);
        OrderService.RemoveImageFromOrder(image);
    }

    [RelayCommand]
    private void SelectAsMaybeClick(Image image)
    {
        image.IsPending = !image.IsPending;
    }

    [RelayCommand]
    private void PrintImageClick(Image image)
    {
        image.IsPrintable = !image.IsPrintable;

        if(image.IsPrintable)
        {
            OrderService.AddImageToOrderPrints(image);
        }
        else
        {
            OrderService.RemoveImageFromOrderPrints(image);

        }
    }

    [RelayCommand]
    private void IncreasePrintCountClick(Image image)
    {
        image.PrintAmount += 1;
    }

    [RelayCommand]
    private void DecreasePrintCountClick(Image image)
    {
        if (image.PrintAmount == 1) return;
        image.PrintAmount -= 1;
    }

    internal void OpenCustomerView()
    {
        CustomerWindow customerWindow = new CustomerWindow(new CustomerWindowViewmodel(OrderService));
        customerWindow.Show();
    }

    [RelayCommand]
    private async Task GetCustomerInformation(Order order)
    {
        OrderService.CurrentOrder = order;
        OrderService.CurrentOrder.IsCompleteingOrder = true;

        var termsOfUseContentDialog = new CompleterOrderDialog(contentDialogService.GetContentPresenter(),OrderService,messageService);

        _ = await termsOfUseContentDialog.ShowAsync();
    }

    internal void PreviousPhoto()
    {
        OrderService.CurrentOrder.PreviousPhoto();
    }

    internal void NextPhoto()
    {
        OrderService.CurrentOrder.NextPhoto();
    }

    internal void SelectPhoto()
    {
        OrderService.CurrentOrder.ApproveImage();
    }

    internal void AttemptDislikePhoto()
    {
        OrderService.CurrentOrder.MaybeImage();
    }
}
