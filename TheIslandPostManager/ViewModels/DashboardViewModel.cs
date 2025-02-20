using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mysqlx.Crud;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Helpers;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using TheIslandPostManager.Views.Pages;
using TheIslandPostManager.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using Order = TheIslandPostManager.Models.Order;

namespace TheIslandPostManager.ViewModels;
public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private IOrderService orderService;
    [ObservableProperty] private IFileService fileService;
    [ObservableProperty] private IImageService imageService;
    private readonly IMessageService messageService;
    private readonly IContentDialogService contentDialogService;
    private readonly IMySQLService mySQLService;
    private readonly INavigationService navigationService;

    public DashboardViewModel(IOrderService orderService,IFileService fileService,IImageService imageService,IMessageService messageService, IContentDialogService contentDialogService,IMySQLService mySQLService, INavigationService navigationService)
    {
        fileService.CleanInputDirectory();
        OrderService = orderService;
        FileService = fileService;
        ImageService = imageService;
        this.messageService = messageService;
        this.contentDialogService = contentDialogService;
        this.mySQLService = mySQLService;
        this.navigationService = navigationService;
        OrderService.CreateOrder();
    }

    public async Task Open()
    {
        try
        {
            await ImageService.OpenImageDialogBrowser();
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Load IMages Error", ex.Message, ex.StackTrace, "3c23fd0b-de96-4dcc-a846-3348f26703bb");            
        }
    }

    [RelayCommand]
    private void CancelOrder(Order order)
    {
        OrderService.Cancel(order);
    }

    public async Task CreateOrder()
    {
        if (OrderService.CurrentOrder is null)
        {
            await OrderService.CreateOrder(false);
            return;
        };

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
    private void ImageClick(ImageObj image)
    {
        //OrderService.CurrentOrder.currentImage
        //Service.SelectedImage = image;
        //Service.SelectedImage.HDImage = image.Image;
       // _ = _navigationService.Navigate(typeof(ImageViewer));
        //Service.SelectedImage.HDImage = await Service.LoadHDImage(image.ImageUrl);
    }

    [RelayCommand]
    public void SelectImageClick(ImageObj image)
    {
        OrderService.AddImageToOrder(image);
    }

    [RelayCommand]
    private async Task DeleteImageClick(ImageObj image)
    {
        if(await ImageService.DeleteImage(image))
        {
            OrderService.RemoveImageFromOrder(image);
        }
    }

    internal async Task DeleteAllImages()
    {
        var uiMessageBox = new MessageBox
        {
            Title = "Delete ImageObj?",
            Content = "Are you sure you would like to remove all images from this collection?",
            IsPrimaryButtonEnabled = true,
            PrimaryButtonText = "Yes",
            CloseButtonText = "No ",

        };

        var result = await uiMessageBox.ShowDialogAsync();

        if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            OrderService.DeleteAllImages();
            ImageService.DeleteAllImages();
        }
    }

    [RelayCommand]
    public void SelectAsMaybeClick(ImageObj image)
    {
        //image.IsPending = !image.IsPending;
        OrderService.SetAsMaybe(image);
    }

    [RelayCommand]
    public void PrintImageClick(ImageObj image)
    {
        OrderService.AddImageToOrderPrints(image);
    }

    [RelayCommand]
    private void IncreasePrintCountClick(ImageObj image)
    {
        image.PrintAmount += 1;
        OrderService.CurrentOrder.UpdateSelectionCounts();
    }

    [RelayCommand]
    private void DecreasePrintCountClick(ImageObj image)
    {
        if (image.PrintAmount == 1) return;
        image.PrintAmount -= 1;
        OrderService.CurrentOrder.UpdateSelectionCounts();
    }

    internal void OpenCustomerView()
    {
        Screen[] s = Screen.AllScreens;

        if (s.Length > 1)
        {
            var display = s.FirstOrDefault(x => x.Primary == false);

            if (display is not null)
            {
                System.Drawing.Rectangle workingArea = display.WorkingArea;
                CustomerWindow customerWindow = new CustomerWindow(new CustomerWindowViewmodel(OrderService));
                customerWindow.Left = workingArea.Left;
                customerWindow.Top = workingArea.Top;
                customerWindow.Width = workingArea.Width;
                customerWindow.Height = workingArea.Height;
                customerWindow.WindowStyle = WindowStyle.None;
                customerWindow.Topmost = true;
                customerWindow.Show();

                messageService.ShowSnackBarMessage("Customer View", $"Customer View now visible on screen {display.DeviceName}");
            }
        }
        else
        {
            messageService.ShowSnackBarMessage("Customer View", $"Secondary Monitor Not Detected", ControlAppearance.Caution, SymbolRegular.Question48);

        }
    }

    [RelayCommand]
    private void CompleteOrder(Order order)
    {
        //if(order.ApprovedImagesCount <= 0)
        //{
        //    messageService.ShowSnackBarMessage("Attention", "No images selected. Please select a photo before attempting to complete this order", ControlAppearance.Caution, SymbolRegular.Stop20);
        //    return;
        //}
        OrderService.CurrentOrder = order;
        OrderService.CurrentOrder.IsCompleteingOrder = true;

        //var termsOfUseContentDialog = new CompleterOrderDialog(contentDialogService.GetContentPresenter(),OrderService,messageService,mySQLService);

        //await termsOfUseContentDialog.GetEmployees();
        //_ = await termsOfUseContentDialog.ShowAsync();

        navigationService.Navigate(typeof(CompleteOrderPage));
    }

    internal void PreviousPhoto()
    {
        OrderService.CurrentOrder.PreviousPhoto();
    }

    internal void NextPhoto()
    {
        OrderService.CurrentOrder.NextPhoto();
    }

    internal async Task PendOrder()
    {
        try
        {
            var termsOfUseContentDialog = new CompletePendingDialog(contentDialogService.GetContentPresenter(), mySQLService);

            var result = await termsOfUseContentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var content = ((Employee)termsOfUseContentDialog.ViewModel.SelectedItem);
                await OrderService.PendOrder((string)content.FullName);
            }
        }
        catch (Exception ex)
        {
          await  messageService.ShowErrorMessage("Error", ex.Message, ex.StackTrace, "", true);
        }
    }

    [RelayCommand]
    private async Task ReplaceImage(ImageObj imageObj)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.Multiselect = true;
        dialog.Filters.Add(new CommonFileDialogFilter("JPEG Files", "*.jpg"));
        dialog.Filters.Add(new CommonFileDialogFilter("PNG Files", "*.png"));

        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            List<Tuple<string,string>> tuples = new List<Tuple<string,string>>();

            var fileName = Path.GetFileName(dialog.FileName);
            var settings = App.AppConfig.GetSection("AppSettings") as AppSettings;

            tuples.Add(new Tuple<string, string>(dialog.FileName, Path.Combine(settings.InputDirectory, fileName)));


            await Copier.CopyFiles(tuples, (Action<int, string>)((prog, fileName) =>
            {
                imageObj.Name = Path.GetFileName(fileName);
                imageObj.ImageUrl = fileName;
                imageObj.LoadImage(fileName);
            }));
            messageService.ShowSnackBarMessage("Import Finished", "All Images have been imported successfully");
        }
    }

    internal void Refresh(string content)
    {
        OrderService.CurrentOrder.RefreshImages(content);
    }
}
