using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using TheIslandPostManager.Models;
using Wpf.Ui.Controls;
using Wpf.Ui;
using Microsoft.WindowsAPICodePack.Dialogs;
using TheIslandPostManager.Helpers;
using Image = TheIslandPostManager.Models.Image;
using System.Configuration;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TheIslandPostManager.Services;

public partial class ImageService : ObservableObject, IImageService
{
    [ObservableProperty] private Image currentImage;
    [ObservableProperty] private ObservableCollection<Image> currentImages;
    private int currentIndex;
    private readonly IMessageService messageService;
    private readonly IOrderService orderService;

    public ImageService(IMessageService messageService,IOrderService orderService)
    {
        //Demo();
        this.messageService = messageService;
        this.orderService = orderService;
        CurrentImages = new();
    }

    private void Demo()
    {
        currentImages =
    [
    new Image()
            {
                Name = "Default",
                ImageUrl = $"E:\\Test\\_JEP5420.JPG",

            }
    ];

        currentImage = new Image
        {
            Name = "Default",
            ImageUrl = "E:\\Test\\_JEP5420.JPG"
        };
    }

    public async Task OpenImageDialogBrowser()
    {
        var dialog = new CommonOpenFileDialog();
        dialog.Multiselect = true;
        dialog.Filters.Add(new CommonFileDialogFilter("JPEG Files", "*.jpg"));
        dialog.Filters.Add(new CommonFileDialogFilter("PNG Files", "*.png"));

        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            //_imageService.ClearImages();
            //CurrentAmount = 0;
            //ProgressBarValue = 0;
            //TotalImages = dialog.FileNames.Count();



            await Copier.CopyFiles(CreatePaths(dialog.FileNames), (prog, fileName) =>
            {
                //IsBusy = true;
                //CurrentAmount++;
                //ProgressBarValue = prog;
                AddImage(new Image(fileName));
            });

            //TotalImages = 0;
            //CurrentAmount = 0;
            //ProgressBarValue = 0;
            await messageService.ShowMessage("Import Finished", "All Images have been imported successfully");
            //IsBusy = false;
        }
    }

    public void DeleteImage(Image image)
    {
        currentImages.Remove(image);
    }

    public void AddImage(Image image)
    {
        image.Index = CurrentImages.Count  + 1;
        CurrentImages.Add(image);
    }

    public void ReplaceImage(string path, Image image)
    {
        var imageObj = currentImages.FirstOrDefault(x => x.ImageUrl.Equals(path));

        //Figure out best way relace path and reload image or replace object

    }

    public void OpenImageLocation(Image image)
    {

    }

    private List<Tuple<string, string>> CreatePaths(IEnumerable<string?> fileNames)
    {
        var result = new List<Tuple<string, string>>();
        foreach (var file in fileNames)
        {
            var fileName = Path.GetFileName(file);
            var settings = App.AppConfig.GetSection("AppSettings") as AppSettings;

            if(!string.IsNullOrWhiteSpace(settings.InputDirectory))
            {
                result.Add(new Tuple<string, string>(file, Path.Combine(settings.InputDirectory, fileName)));
            }
        }

        return result;
    }

    public void SelectAllImages()
    {
        foreach (Image image in CurrentImages)
        {
            image.IsSelected = true;
            orderService.AddImageToOrder(image);
        }
    }

    public void DeSelectAllImages()
    {
        foreach (Image image in CurrentImages)
        {
            image.IsSelected = false;
            orderService.RemoveImageFromOrder(image);
        }
    }

    public void PrintAllImages()
    {
        foreach (Image image in CurrentImages)
        {
            image.IsPrintable = true;
            image.IsSelected = true;

        }
    }

    public async Task DeleteAllImages()
    {
        var uiMessageBox = new MessageBox
        {
            Title = "Delete Image?",
            Content = "Are you sure you would like to remove all images from this collection?",
            IsPrimaryButtonEnabled = true,
            PrimaryButtonText = "Yes",
            CloseButtonText = "No ",

        };

        var result = await uiMessageBox.ShowDialogAsync();

        if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            for (int i = CurrentImages.Count - 1; i >= 0; i--)
            {
                var image = CurrentImages[i];
                image.HDImage = null;
                image.LowImage = null;
                File.Delete(CurrentImages[i].ImageUrl);
                CurrentImages.Remove(CurrentImages[i]);
                UpdateImagesIndex();
            }
            GC.Collect();
        }


      //  OnImageCountUpdate?.Invoke();
    }

    public void UpdateImagesIndex()
    {
        for (int i = 0; i < CurrentImages.Count; i++)
        {
            CurrentImages[i].Index = i + 1;
        }
    }

    public void SetOrder(Order order)
    {
        DeSelectAllImages();
        foreach (Image image in CurrentImages)
        {
            image.IsPending = false;
            image.IsSelected = false;
            image.IsPrintable = false;
            image.PrintAmount = 1;
        }
    }

    public void UpdateOrder(Order obj)
    {

        // The Print amount is changing because its still referencing the same object you have to clone the object

        foreach (Image image in CurrentImages)
        {
            foreach(var imageObj in obj.ApprovedImages)
            {
                if(imageObj.ImageUrl.Equals(image.ImageUrl))
                {
                    image.Restore(imageObj);
                }
            }

            //foreach (var imageObj in obj.ApprovedPrints)
            //{
            //    if (imageObj.ImageUrl.Equals(image.ImageUrl))
            //    {
            //        image.PrintAmount = imageObj.PrintAmount;
            //        image.IsPrintable = true;
            //    }
            //}
        }
    }
}
