using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using TheIslandPostManager.Models;
using ImageObj = TheIslandPostManager.Models.ImageObj;
using TheIslandPostManager.Helpers;
using Microsoft.VisualBasic;
using FluentEmail.Core;


namespace TheIslandPostManager.Services;

public partial class OrderService : ObservableObject, IOrderService
{
    //[ObservableProperty] private ObservableCollection<Image> images;
    [ObservableProperty] private ObservableCollection<Order> currentOrders = new();
    [ObservableProperty] private ObservableCollection<Order> completedOrders = new();
    [ObservableProperty] private ObservableCollection<Order> purchaseHistory = new();
    [ObservableProperty] private ObservableCollection<Order> pendingOrders = new();
    [ObservableProperty] private Order currentOrder;
    [ObservableProperty] private Order currentHistoryOrder;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private bool showImageViewer = true;
    [ObservableProperty] private bool showGridViewer = false;

    private AppSettings? settings => App.AppConfig.GetSection("AppSettings") as AppSettings;

    [ObservableProperty] private bool isWatermarkVisible = true;

    private readonly IMessageService messageService;
    private readonly IFileService fileService;
    private readonly IMySQLService mySQLService;
    public Action OnPendingOrderCountChanged;
    private string _appDataLocation;
    private int currentOrderIndex;
    private string _tempFolder;
    private string _inputFolder;

    public OrderService(IMessageService messageService,IFileService fileService,IMySQLService mySQLService)
    {
        currentOrders = new ObservableCollection<Order>();
        this.messageService = messageService;
        this.fileService = fileService;
        this.mySQLService = mySQLService;
        var outPutFolder = settings.OutputDirectory;
        _appDataLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        _tempFolder = Path.Combine(_appDataLocation, "Temp");
        _inputFolder = Path.Combine(_appDataLocation, "Input");
    }

    public async Task GetPendingOrders()
    {
        PendingOrders = await mySQLService.GetPendingOrders();
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

    public void CreateOrder(Order order)
    {
        order.SetCurrentIndex(currentOrderIndex);
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
        //fileService.Purge(CurrentOrder);
        order.Finalize();
        CurrentOrders.Remove(order);
        PurchaseHistory.Add(order);
        GC.Collect();
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
        GC.Collect();
    }

    public void Cancel(Order order)
    {
        CurrentOrders.Remove(order);

        if(CurrentOrder == order)
        {
            CurrentOrder = null;
        }
        GC.Collect();
    }

    public async Task CompleteOrderAsync()
     {
        IsBusy = true;
                 
        if(CurrentOrder.HasImages())
        {
            await Task.Run(() =>
            {

                var outputLoc = CurrentOrder.GetOutputLocation();

                if (Directory.Exists(outputLoc))
                {
                    outputLoc = CurrentOrder.GetSubOutputLocation();
                }

                foreach (ImageObj image in CurrentOrder.CurrentImages)
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
                            image.ImageUrl = outputFile;
                        });
                    }
                }

                if (!string.IsNullOrWhiteSpace(CurrentOrder.Email))
                {
                    var emails = CurrentOrder.Email.Split(',');

                    if (emails.Length > 1)
                    {
                        for (int i = 1; i < emails.Length; i++)
                        {
                            CurrentOrder.CC.Add(new FluentEmail.Core.Models.Address(emails[i].Trim()));
                        }

                        CurrentOrder.Email = emails[0].Trim();
                    }
                }
            });

            fileService.OpenLocation(CurrentOrder.GetOutputLocation());

        }

        await mySQLService.AddCompletedOrder(CurrentOrder);

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

    public void AddImageToOrder(ImageObj image)
    {
        CurrentOrder.ApproveImage(image);
    }

    public void RemoveImageFromOrder(ImageObj image)
    {
        CurrentOrder.DisApproveImage(image);
        RemoveImageFromOrderPrints(image);
    }

    public void AddImageToOrderPrints(ImageObj image)
    {
        CurrentOrder.ApprovePrint(image);
    }

    public void RemoveImageFromOrderPrints(ImageObj image)
    {
        CurrentOrder.DisApprovePrint(image);
    }

    public async Task PendOrder(string name)
    {
        try
        {
            if (CurrentOrder.CurrentImages.Count <= 0)
            {
                messageService.ShowSnackBarMessage("Error", "Please import images to pend.", Wpf.Ui.Controls.ControlAppearance.Caution, Wpf.Ui.Controls.SymbolRegular.ThumbDislike24);
                return;
            }

            var dirName = Path.Combine(settings.PendingDirectory, Guid.NewGuid().ToString());

            CurrentOrder.Name = name;

            if (fileService.CreateDirectory(dirName))
            {
                foreach (ImageObj item in CurrentOrder.CurrentImages)
                {
                    var imgPath = Directory.GetParent(item.ImageUrl);

                    var dirInfo = new DirectoryInfo(item.ImageUrl);
                    var dirInfo2 = new DirectoryInfo(settings.PendingDirectory);

                    if (IsStrictSubDirectoryOf(dirInfo, dirInfo2))
                    {
                        continue;
                    }
                    var newFileName = Path.Combine(dirName, item.Name);
                    await fileService.Move(new(item.ImageUrl, newFileName));
                    item.ImageUrl = newFileName;
                }


            }

            CurrentOrder.Thumbnail = CurrentOrder.CurrentImages.FirstOrDefault().LowImage;
            CurrentOrder.DownloadURL = dirName;
            CurrentOrder.Date = DateTime.Now;
            PendingOrders.Add(CurrentOrder);
            await mySQLService.AddPendingOrder(CurrentOrder);
            //CurrentOrder.Thumbnail = LoadImageFile(ThumnailLocationFormat(CurrentOrder));
            //SavePendingOrder(order);
            Cancel(CurrentOrder);
        }
        catch (Exception ex)
        {
            messageService.ShowSnackBarMessage("Error", ex.Message, Wpf.Ui.Controls.ControlAppearance.Primary, Wpf.Ui.Controls.SymbolRegular.ThumbDislike24);
        }
    }

    internal bool IsSubDirectoryOfOrSame(DirectoryInfo directoryInfo, DirectoryInfo potentialParent)
    {
        if (DirectoryInfoComparer.Default.Equals(directoryInfo, potentialParent))
        {
            return true;
        }

        return IsStrictSubDirectoryOf(directoryInfo, potentialParent);
    }

    internal bool IsStrictSubDirectoryOf(DirectoryInfo directoryInfo, DirectoryInfo potentialParent)
    {
        while (directoryInfo.Parent != null)
        {
            if (DirectoryInfoComparer.Default.Equals(directoryInfo.Parent, potentialParent))
            {
                return true;
            }

            directoryInfo = directoryInfo.Parent;
        }

        return false;
    }

    public async Task OpenOrderFromPending(Order order)
    {
        CreateOrder(order);

        var result = await mySQLService.LoadImages(order.CustomerID);
        if(result is not null)
        {
            order.ApprovedPrints = result.Item3;
            order.ApprovedImages = result.Item2;
            order.CurrentImages = result.Item1;
            foreach (var item in order.CurrentImages)
            {
                var newFile = Path.Combine(settings.InputDirectory, Path.GetFileName(item.ImageUrl));
                await fileService.Move(new(item.ImageUrl, newFile));
                item.Restore(newFile);
            }
            order.LinkCollection();

            PendingOrders.Remove(order);
            fileService.DeleteDirectory(order.DownloadURL);
            await mySQLService.RemovePendingOrder(order);

        }
        else
        {
            await messageService.ShowErrorMessage("Error", "Failed to load pending image");
        }
    }

    public async Task DeletePendingOrder(Order order)
    {
        var result = await messageService.ShowMessage("Delete Pending Order", $"Are you sure you would like to delete {order.Name} pending order", "NO", Wpf.Ui.Controls.ControlAppearance.Secondary,true);
        
        if(result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            await mySQLService.RemovePendingOrder(order);
            fileService.DeleteDirectory(order.DownloadURL);
            PendingOrders.Remove(order);
        }
    }

    public void SetAsMaybe(ImageObj image)
    {
        image.IsPending = !image.IsPending;
        RemoveImageFromOrder(image);
    }

    public void DeleteAllImages()
    {
        foreach (var item in CurrentOrder.CurrentImages)
        {
            RemoveImageFromOrder(item);
        }
    }
}
public class DirectoryInfoComparer : IEqualityComparer<DirectoryInfo>
{
    private static readonly char[] TrimEnd = { '\\' };
    public static readonly DirectoryInfoComparer Default = new DirectoryInfoComparer();
    private static readonly StringComparer OrdinalIgnoreCaseComparer = StringComparer.OrdinalIgnoreCase;

    private DirectoryInfoComparer()
    {
    }

    public bool Equals(DirectoryInfo x, DirectoryInfo y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return OrdinalIgnoreCaseComparer.Equals(x.FullName.TrimEnd(TrimEnd), y.FullName.TrimEnd(TrimEnd));
    }

    public int GetHashCode(DirectoryInfo obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        return OrdinalIgnoreCaseComparer.GetHashCode(obj.FullName.TrimEnd(TrimEnd));
    }
}

