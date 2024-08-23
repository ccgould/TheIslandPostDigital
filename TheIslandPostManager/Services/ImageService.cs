using System.IO;
using TheIslandPostManager.Models;
using Wpf.Ui.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using TheIslandPostManager.Helpers;
using ImageObj = TheIslandPostManager.Models.ImageObj;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TheIslandPostManager.Services;

public partial class ImageService : ObservableObject, IImageService
{
    //[ObservableProperty] private Image currentImage;
    //[ObservableProperty] private ObservableCollection<Image> currentImages;
    private int currentIndex;
    private readonly IMessageService messageService;
    private readonly IOrderService orderService;

    public ImageService(IMessageService messageService,IOrderService orderService)
    {
        this.messageService = messageService;
        this.orderService = orderService;
        //CurrentImages = new();
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



            await Copier.CopyFiles(CreatePaths(dialog.FileNames), (Action<int, string>)((prog, fileName) =>
            {
                //IsBusy = true;
                //CurrentAmount++;
                //ProgressBarValue = prog;

                AddImage((ImageObj)new(fileName));
            }));

            //TotalImages = 0;
            //CurrentAmount = 0;
            //ProgressBarValue = 0;
            messageService.ShowSnackBarMessage("Import Finished", "All Images have been imported successfully");
            //IsBusy = false;
            GC.Collect();
        }
    }

    public async Task<bool> DeleteImage(ImageObj image)
    {
        var result = await messageService.ShowMessage("Delete", $"Are you sure you would like to delete {image.Name}","NO", ControlAppearance.Secondary, true);

        if(result == MessageBoxResult.Primary)
        {
            GetCurrentOrder().CurrentImages.Remove(image);
            return true;
        }

        return false;
    }

    public void AddImage(ImageObj image)
    {
        image.Index = GetCurrentOrder().CurrentImages.Count + 1;
        GetCurrentOrder().CurrentImages.Add(image);
    }

    private Order GetCurrentOrder()
    {
        if(orderService.CurrentOrder is null)
        {
            orderService.CreateOrder();
        }

        return orderService.CurrentOrder;
    }

    public void ReplaceImage(string path, ImageObj image)
    {
        var imageObj = GetCurrentOrder().CurrentImages.FirstOrDefault(x => x.ImageUrl.Equals(path));

        //Figure out best way relace path and reload image or replace object

    }

    public void OpenImageLocation(ImageObj image)
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
        foreach (ImageObj image in GetCurrentOrder().CurrentImages)
        {
            image.IsSelected = true;
            orderService.AddImageToOrder(image);
        }
    }

    public void DeSelectAllImages()
    {
        foreach (ImageObj image in GetCurrentOrder().CurrentImages)
        {
            image.IsSelected = false;
            orderService.RemoveImageFromOrder(image);
        }
    }

    public void PrintAllImages()
    {
        foreach (ImageObj image in GetCurrentOrder().CurrentImages)
        {
            image.IsPrintable = true;
            image.IsSelected = true;
            orderService.AddImageToOrderPrints(image);
        }
    }

    public async Task DeleteAllImages()
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

        if (result == MessageBoxResult.Primary)
        {
            for (int i = GetCurrentOrder().CurrentImages.Count - 1; i >= 0; i--)
            {
                var image = GetCurrentOrder().CurrentImages[i];
                image.HDImage = null;
                image.LowImage = null;
                File.Delete(GetCurrentOrder().CurrentImages[i].ImageUrl);
                GetCurrentOrder().CurrentImages.Remove(GetCurrentOrder().CurrentImages[i]);
                UpdateImagesIndex();
            }
            GC.Collect();
        }


      //  OnImageCountUpdate?.Invoke();
    }

    public void UpdateImagesIndex()
    {
        for (int i = 0; i < GetCurrentOrder().CurrentImages.Count; i++)
        {
            GetCurrentOrder().CurrentImages[i].Index = i + 1;
        }
    }

    public void SetOrder(Order order)
    {
        //DeSelectAllImages();
        //foreach (Image image in GetCurrentOrder().CurrentImages)
        //{
        //    image.IsPending = false;
        //    image.IsSelected = false;
        //    image.IsPrintable = false;
        //    image.PrintAmount = 1;
        //}
    }

    public void UpdateOrder(Order obj)
    {

        // The Print amount is changing because its still referencing the same object you have to clone the object
        return;

        foreach (ImageObj image in GetCurrentOrder().CurrentImages)
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

    //internal void AddWaterMark(string personName, string originalImage, Action<string, string> callback)
    //{
    //    var fileName = Path.GetFileNameWithoutExtension(originalImage);
    //    var ext = Path.GetExtension(originalImage);
    //    var newFileName = $"{fileName}_watermarked{ext}";
    //    var origLocation = Path.GetDirectoryName(originalImage);

    //    using (System.Drawing.Image image = System.Drawing.Image.FromFile(originalImage))
    //    {
    //        ImageHelpers.Rotate(image);

    //        if (_applicationSaveService.ApplicationSaveData.AddWaterMarkToImage)
    //        {
    //            using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(_watermark))
    //            {
    //                var newWatermark = ResizeImage(watermarkImage,
    //                    new Size(watermarkImage.Width * 2, watermarkImage.Height * 2));

    //                using (Graphics imageGraphics = Graphics.FromImage(image))
    //                using (TextureBrush watermarkBrush = new TextureBrush(newWatermark))
    //                {
    //                    int x = 0;
    //                    int y = 0;

    //                    switch (_applicationSaveService.ApplicationSaveData.WatermarkPosition)
    //                    {
    //                        case 0:
    //                            x = _applicationSaveService.ApplicationSaveData.ImageWidth;
    //                            y = _applicationSaveService.ApplicationSaveData.ImageHeight;
    //                            break;
    //                        case 1:
    //                            x = (image.Width - newWatermark.Width - _applicationSaveService.ApplicationSaveData.ImageWidth);
    //                            y = _applicationSaveService.ApplicationSaveData.ImageHeight;
    //                            break;
    //                        case 2:
    //                            x = (image.Width - newWatermark.Width) / 2;
    //                            y = (image.Height - newWatermark.Height) / 2;
    //                            break;
    //                        case 3:
    //                            x = _applicationSaveService.ApplicationSaveData.ImageWidth;
    //                            y = (image.Height - newWatermark.Height - _applicationSaveService.ApplicationSaveData.ImageHeight);
    //                            break;
    //                        case 4:
    //                            x = (image.Width - newWatermark.Width - _applicationSaveService.ApplicationSaveData.ImageWidth);
    //                            y = (image.Height - newWatermark.Height - _applicationSaveService.ApplicationSaveData.ImageHeight);
    //                            break;
    //                    }

    //                    watermarkBrush.TranslateTransform(x, y);
    //                    imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(newWatermark.Width + 1, newWatermark.Height)));
    //                }
    //            }
    //        }

    //        // Encoder parameter for image quality 
    //        EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, _applicationSaveService.ApplicationSaveData.ImageQuality);
    //        // JPEG image codec 
    //        ImageCodecInfo jpegCodec = ImageHelpers.GetEncoderInfo("image/jpeg");
    //        EncoderParameters encoderParams = new EncoderParameters(1);
    //        encoderParams.Param[0] = qualityParam;

    //        var personalDir = Path.Combine(_outPutFolder, ReplaceInvalidChars(personName));

    //        if (!Directory.Exists(personalDir))
    //        {
    //            Directory.CreateDirectory(personalDir);
    //        }

    //        var newFileLocation = Path.Combine(personalDir, newFileName);
    //        image.Save(newFileLocation, jpegCodec, encoderParams);
    //        callback?.Invoke(newFileLocation, newFileName);
    //    }
    //}

    private string ReplaceInvalidChars(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }

    private static System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, Size size)
    {
        int sourceWidth = imgToResize.Width;
        int sourceHeight = imgToResize.Height;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)size.Width / (float)sourceWidth);
        nPercentH = ((float)size.Height / (float)sourceHeight);

        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
        //b = ChangeImageOpacity(b, 0.1);

        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return (System.Drawing.Image)b;
    }
}
