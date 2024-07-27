using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using TheIslandPostManager.Models;
using Image = TheIslandPostManager.Models.Image;
using TheIslandPostManager.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace TheIslandPostManager.Services;

public partial class OrderService : ObservableObject, IOrderService
{
    //[ObservableProperty] private ObservableCollection<Image> images;
    [ObservableProperty] private ObservableCollection<Order> currentOrders = new();
    [ObservableProperty] private ObservableCollection<Order> completedOrders = new();
    [ObservableProperty] private ObservableCollection<Order> purchaseHistory = new();
    [ObservableProperty] private Order currentOrder;
    private readonly IMessageService messageService;
    private readonly IFileService fileService;
    private AppSettings? settings => App.AppConfig.GetSection("AppSettings") as AppSettings;
    private int currentOrderIndex;
    [ObservableProperty] private bool isBusy;


    private string _appDataLocation;
    private string _tempFolder;
    private string _inputFolder;

    public OrderService(IMessageService messageService,IFileService fileService)
    {
        currentOrders = new ObservableCollection<Order>();
        this.messageService = messageService;
        this.fileService = fileService;
        var outPutFolder = settings.OutputDirectory;
        _appDataLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        _tempFolder = Path.Combine(_appDataLocation, "Temp");
        _inputFolder = Path.Combine(_appDataLocation, "Input");
    }

    public async Task CreateOrder(bool copy = false)
    {

        var order = new Order(currentOrderIndex);

        if(copy)
        {
            foreach (var image in CurrentOrder.CurrentImages)
            {
                order.CurrentImages.Add(image.Copy(true));
            }
        }


        CurrentOrders.Add(order);
        CurrentOrder = order;

        currentOrderIndex++;
    }

    public void UpdateOrder(Order order)
    {
        CurrentOrder = order;
    }

    public void DeleteOrder(Order order)
    {
        order.Finalize();
        CurrentOrders.Remove(order);
        PurchaseHistory.Add(order);
    }

    public void DeleteOrder(string orderId)
    {
       var order =  CurrentOrders.FirstOrDefault(x => x.CustomerID == orderId);
        DeleteOrder(order);
    }

    public async Task CancelAll()
    {
        CurrentOrders.Clear();
        await CreateOrder();
    }

    public void Cancel(Order order)
    {

        CurrentOrders.Remove(order);

        if(CurrentOrder == order)
        {
            CurrentOrder = null;
        }

    }

    public async Task CompleteOrderAsync()
    {
        IsBusy = true;
                    
        await Task.Run(() =>
        {

            var outputLoc = CurrentOrder.GetOutputLocation();

            if(Directory.Exists(outputLoc))
            {
                outputLoc = CurrentOrder.GetSubOutputLocation();
            }

            foreach (Image image in CurrentOrder.CurrentImages)
            {
                if (image.IsSelected)
                {
                    //TODO Add Images TO mysql

                    fileService.CreateDirectory(_tempFolder);

                    AddWaterMark(outputLoc, image.ImageUrl, async (outputFile, filename) =>
                    {
                        if (image.IsPrintable)
                        {
                            var files = new List<Tuple<string, string>>();


                            for (int i = 0; i < image.PrintAmount; i++)
                            {
                                var wOutExt = Path.GetFileNameWithoutExtension(filename);
                                var result = $"{wOutExt}_{i}.jpg";
                                files.Add(new Tuple<string, string>(outputFile, Path.Combine(_tempFolder, result)));
                            }

                            await fileService.Copy(files);
                        }
                    });
                }
            }
        });

        fileService.OpenLocation(CurrentOrder.GetOutputLocation());

        //Delete Order
        DeleteOrder(CurrentOrder);

        CurrentOrder = CurrentOrders.FirstOrDefault();

    }

    internal void AddWaterMark(string path, string originalImage, Action<string, string> callback)
    {

        

        
        var fileName = Path.GetFileNameWithoutExtension(originalImage);
        var ext = Path.GetExtension(originalImage);
        var newFileName = $"{fileName}_watermarked{ext}";
        var origLocation = Path.GetDirectoryName(originalImage);
        var watermark = settings.WatermarkDirectory;
        var outPutFolder = settings.OutputDirectory;


        using (System.Drawing.Image image = System.Drawing.Image.FromFile(originalImage))
        {
            ImageHelpers.Rotate(image);

            if (settings.AddWwatermark)
            {
                using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(watermark))
                {
                    var newWatermark = ImageHelpers.ResizeImage(watermarkImage,
                        new Size(watermarkImage.Width * 2, watermarkImage.Height * 2));

                    using (Graphics imageGraphics = Graphics.FromImage(image))
                    using (TextureBrush watermarkBrush = new TextureBrush(newWatermark))
                    {
                        int x = 0;
                        int y = 0;

                        switch (settings.WatermarkPosition)
                        {
                            case 0:
                                x = settings.WatermarkBufferW;
                                y = settings.WatermarkBufferH;
                                break;
                            case 1:
                                x = (image.Width - newWatermark.Width - settings.WatermarkBufferW);
                                y = settings.WatermarkBufferH;

                                break;
                            case 2:
                                x = (image.Width - newWatermark.Width) / 2;
                                y = (image.Height - newWatermark.Height) / 2;
                                break;
                            case 3:
                                x = settings.WatermarkBufferW;
                                y = (image.Height - newWatermark.Height - settings.WatermarkBufferH);
                                break;
                            case 4:
                                x = (image.Width - newWatermark.Width - settings.WatermarkBufferW);
                                y = (image.Height - newWatermark.Height - settings.WatermarkBufferH);
                                break;
                        }

                        watermarkBrush.TranslateTransform(x, y);
                        imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(newWatermark.Width + 1, newWatermark.Height)));
                    }
                }
            }

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, settings.ImageQuality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = ImageHelpers.GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            //var personalDir = Path.Combine(outPutFolder, ReplaceInvalidChars(order.Name));


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var newFileLocation = Path.Combine(path, newFileName);
            image.Save(newFileLocation, jpegCodec, encoderParams);
            callback?.Invoke(newFileLocation, newFileName);
        }
    }

    private string ReplaceInvalidChars(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }

    public void AddImageToOrder(Image image)
    {
        CurrentOrder.ApproveImage();
    }

    public void RemoveImageFromOrder(Image image)
    {
        CurrentOrder.DisApproveImage(image);
    }

    public void AddImageToOrderPrints(Image image)
    {
        CurrentOrder.ApprovePrint(image);
    }

    public void RemoveImageFromOrderPrints(Image image)
    {
        CurrentOrder.DisApprovePrint(image);
    }
}