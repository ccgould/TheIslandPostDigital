using CommunityToolkit.Mvvm.ComponentModel;
using IPDLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheIslandPostManager.Models;

public partial class Order : ObservableObject
{
    [ObservableProperty] private string downloadURL;

    [Display(Name = "Customer ID", AutoGenerateField = false)]
    public string CustomerID { get; set; } = Guid.NewGuid().ToString();

    [ObservableProperty] private string name;

    [ObservableProperty] private string email;

    public List<Address> CC { get; set; }

    [ObservableProperty] private ObservableCollection<ImageObj> currentImages = new();
    [ObservableProperty] private ICollectionView orderCollectionView;
    [ObservableProperty] private ImageObj currentImage;
    [ObservableProperty] private List<IImage> approvedImages = new();
    [ObservableProperty] private List<IImage> approvedPrints = new();
    [ObservableProperty] private ObservableCollection<PurchaseItem> cart = new();
    [ObservableProperty] private decimal cartTotal;
    [ObservableProperty] private decimal vatTotal;
    [ObservableProperty] private int approvedImagesCount;
    [ObservableProperty] private int approvedPrintsCount;
    [ObservableProperty] private int videoCount;
    [ObservableProperty] private int printingCount;
    [ObservableProperty] private string orderPath;
    [ObservableProperty] private DateTime date;
    [ObservableProperty] private bool isFinalized;
    [ObservableProperty] private bool isCompleteingOrder;
    [ObservableProperty] private int maybeCount;
    [ObservableProperty] private BitmapImage thumbnail;
    [ObservableProperty] private PurchaseType selectedPurchaseType = PurchaseType.None;
    public IEnumerable<PurchaseType> PurchaseTypeValues
    {
        get
        {
            return Enum.GetValues(typeof(PurchaseType))
                .Cast<PurchaseType>();
        }
    }

    private string _orderFilter = "All";
    internal object EmployeeID;


    [ObservableProperty] private Employee employee;
    [ObservableProperty] private ObservableCollection<PurchaseHistoryItem> purchaseHistoryItems;
    //[ObservableProperty] private ObservableCollection<PurchaseItem> purchaseItems;



    public string OrderFilter
    {
        get
        {
            return _orderFilter;
        }
        set
        {
            _orderFilter = value;
            OnPropertyChanged(nameof(OrderFilter));
            OrderCollectionView.Refresh();
        }
    }

    //[ObservableProperty] private int retailCount;

    public Order(int index)
    {
        Name = $"Order {index:D3}";
        CC = new();
        LinkCollection();
    }

    internal void LinkCollection()
    {
        if(OrderCollectionView is not null)
        {
            OrderCollectionView.CurrentChanged -= OrderCollectionView_CurrentChanged;
        }

        OrderCollectionView = CollectionViewSource.GetDefaultView(CurrentImages);
        OrderCollectionView.Filter = FilterImages;
        OrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ImageObj.IsSelected)));
        OrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ImageObj.IsPrintable)));
        OrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ImageObj.IsPending)));
        OrderCollectionView.SortDescriptions.Add(new SortDescription(nameof(ImageObj.Name), ListSortDirection.Ascending));
        
        var item = (ImageObj)OrderCollectionView.CurrentItem;
        
        if (item is not null)
        {
            OrderCollectionView_CurrentChanged(null,null);
        }

        OrderCollectionView.CurrentChanged += OrderCollectionView_CurrentChanged;
    }

    private void OrderCollectionView_CurrentChanged(object? sender, EventArgs e)
    {
        CurrentImage = (ImageObj)OrderCollectionView.CurrentItem;
    }

    private bool FilterImages(object obj)
    {
        //TODO Add Filter

        var image = (ImageObj)obj;

        if (this._orderFilter == null)
            return true;

        switch (OrderFilter)
        {
            case "All":
                return true;
            case "Selected":
                return image.IsSelected;
            case "Maybe":
                return image.IsPending;
            case "Both":
                return image.IsPending || image.IsSelected;
            case "Not Selected":
                return !image.IsPending && !image.IsSelected;
            default:
                return false;
        }
    }
    internal string GetOutputLocation()
    {
        var settings = App.AppConfig.GetSection("AppSettings") as AppSettings;
        return Path.Combine(settings.OutputDirectory,DateTime.Now.ToString("yyyy"),DateTime.Now.ToString("MMM"), DateTime.Now.ToString("MMM_dd_yyyy"), Name);
    }
    
    internal void Finalize()
    {
        try
        {
            Date = DateTime.Now;
            foreach (var item in CurrentImages)
            {
                if (string.IsNullOrWhiteSpace(item.RootImageUrl)) continue;
                File.Delete(item.RootImageUrl);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to delete image from input directrory " + ex.Message, "Error");
        }
    }

    internal string GetSubOutputLocation()
    {
        return Path.Combine(GetOutputLocation(), DateTime.Now.ToString("ddd_MM_yyyy_hh_mm_ss"));
    }

    internal void PreviousPhoto()
    {
        OrderCollectionView.MoveCurrentToPrevious();

        if(OrderCollectionView.CurrentItem == null)
        {
            OrderCollectionView.MoveCurrentToLast();
        }
    }

    internal void NextPhoto()
    {
        OrderCollectionView.MoveCurrentToNext();

        if (OrderCollectionView.CurrentItem == null)
        {
            OrderCollectionView.MoveCurrentToFirst();
        }
    }

    internal void ApproveImage(ImageObj image)
    {
        if(image is null)
        {
            return;
        }

        if(!ApprovedImages.Contains(image))
        {
            image.IsSelected = true;
            ApprovedImages.Add(image);
            UpdateSelectionCounts();
            ApprovePrint(image);
        }
        else
        {
            DisApproveImage(image);
        }
    }

    internal void DisApproveImage(ImageObj image)
    
    {
        if (image is null)
        {
            return;
        }

        if (ApprovedImages.Contains(image))
        {
            image.IsSelected = false;
            ApprovedImages.Remove(image);
            DisApprovePrint(image);
        }

        UpdateSelectionCounts();
    }

    internal void ApprovePrint(ImageObj image)
    {
        if (!image.IsSelected) return;

        if(!ApprovedPrints.Contains(image))
        {
            image.IsPrintable = true;
            ApprovedPrints.Add(image);
            UpdateSelectionCounts();
        }
        else
        {
            DisApprovePrint(image);
        }
    }

    internal void DisApprovePrint(ImageObj image)
    {
        if(ApprovedPrints.Contains(image))
        {
            image.IsPrintable = false;
            ApprovedPrints.Remove(image);
            UpdateSelectionCounts();
        }
    }

    internal void RefreshImages(string selection)
    {
        this._orderFilter = selection;
        this.OrderCollectionView.Refresh();
    }

    internal void MaybeImage()
    {
        CurrentImage.IsPending = !CurrentImage.IsPending;
        DisApproveImage(CurrentImage);
        UpdateSelectionCounts();
    }

    public void UpdateSelectionCounts()
    {
        ApprovedImagesCount = CurrentImages.Count(x => x.IsSelected);
        MaybeCount = CurrentImages.Count(x => x.IsPending);
        ApprovedPrintsCount = CurrentImages.Count(x => x.IsPrintable);
        PrintingCount = ApprovedPrints.Sum(x => x.PrintAmount);
    }

    internal void AddItemToCart(PurchaseItem purchaseItem)
    {
        if(!Cart.Contains(purchaseItem))
        {
            Cart.Add(purchaseItem);
            Date = DateTime.Now;
        }

        UpdateCartTotal();
    }

    internal void UpdateCartTotal()
    {
        decimal total = 0;

        for (int i = 0; i < Cart.Count; i++)
        {
          total += Cart[i].TotalCost;
        }

        var vat = 10m;

        var vatPercent = vat / 100m;

        //CartTotal = Math.Round(total * (1 + vatPercent),2); Charlie doesnt want to show the vat total

        CartTotal = total;


        VatTotal = Math.Round(CartTotal - total,2);
    }

    internal void SetCurrentIndex(int currentOrderIndex)
    {
        Name = $"Order {currentOrderIndex:D3}";
    }

    internal bool HasImages()
    {
        return ApprovedImagesCount > 0;
    }
}

public enum PurchaseType
{
    [Description("No Cash (Default)")]
    None = 1,
    [Description("Cash Payment")]
    Cash = 2,
    [Description("Card Payment")]
    Card = 3,
    [Description("Both card and cash payments")]
    Both = 4
}