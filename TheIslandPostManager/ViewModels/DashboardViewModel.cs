using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mysqlx.Crud;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Order = TheIslandPostManager.Models.Order;

namespace TheIslandPostManager.ViewModels;
public partial class DashboardViewModel : ObservableObject
{
    public IOrderService OrderService { get; set; }
    [ObservableProperty] private IFileService fileService;
    [ObservableProperty] private IImageService imageService;

    public DashboardViewModel(IOrderService orderService,IFileService fileService,IImageService imageService)
    {
        OrderService = orderService;
        this.fileService = fileService;
        this.imageService = imageService;
    }

    public async Task Open()
    {
        await ImageService.OpenImageDialogBrowser();
    }


    [RelayCommand]
    private void CompleteOrder(Order order)
    {

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
        ImageService.CurrentImage = image;
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
}
