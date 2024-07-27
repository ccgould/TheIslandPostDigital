using CommunityToolkit.Mvvm.ComponentModel;
using Mysqlx.Crud;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Data;

namespace TheIslandPostManager.Models;

public partial class Order : ObservableObject
{
    [ObservableProperty] private string downloadURL;

    [Display(Name = "Customer ID", AutoGenerateField = false)]
    public string CustomerID { get; set; } = Guid.NewGuid().ToString();

    [ObservableProperty] private string name;

    [ObservableProperty] private string email;

    [Display(Name = "Date/Time", AutoGenerateField = false)]
    public DateTime DateTime { get; set; }
    //public List<Address> CC { get; set; }

    [ObservableProperty] private ObservableCollection<Image> currentImages = new();
    [ObservableProperty] private ICollectionView orderCollectionView;

    [ObservableProperty] private Image currentImage;
    [ObservableProperty] private List<IImage> approvedImages = new();
    [ObservableProperty] private List<IImage> approvedPrints = new();
    [ObservableProperty] private ObservableCollection<PurchaseItem> cart = new();
    [ObservableProperty] private decimal cartTotal;
    [ObservableProperty] private decimal vatTotal;
    [ObservableProperty] private int approvedImagesCount;
    [ObservableProperty] private int approvedPrintsCount;
    [ObservableProperty] private DateTime date;

    [ObservableProperty] private bool isFinalized;


    private string _orderFilter = "All";
    [ObservableProperty] private bool isCompleteingOrder;

    [ObservableProperty] private int maybeCount;

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

    public Order(int index)
    {
        Name = $"Order {index:D3}";
        OrderCollectionView = CollectionViewSource.GetDefaultView(currentImages);
        OrderCollectionView.Filter = FilterImages;
        OrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Image.IsSelected)));
        OrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Image.IsPrintable)));
        OrderCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Image.IsPending)));
        OrderCollectionView.SortDescriptions.Add(new SortDescription(nameof(Image.Name), ListSortDirection.Ascending));

        OrderCollectionView.CurrentChanged += delegate
        {
            CurrentImage = (Image)OrderCollectionView.CurrentItem;
        };
    }

    private bool FilterImages(object obj)
    {
        //TODO Add Filter

        var image = (Image)obj;

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
            default:
                break;
        }
        return false;
    }

    internal string GetOutputLocation()
    {
        var settings = App.AppConfig.GetSection("AppSettings") as AppSettings;
        return Path.Combine(settings.OutputDirectory, Name);
    }
    
    internal void Finalize()
    {
        DateTime = DateTime.Now;
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

    internal void ApproveImage()
    {
        if(CurrentImage is null)
        {
            return;
        }
        if(!ApprovedImages.Contains(CurrentImage))
        {
            CurrentImage.IsSelected = !CurrentImage.IsSelected;
            ApprovedImages.Add(CurrentImage);
            UpdateSelectionCounts();
        }
    }

    internal void DisApproveImage(Image image)
    {
        if(ApprovedImages.Contains(image))
        {
            ApprovedImages.Remove(image);
            UpdateSelectionCounts();
        }
    }

    internal void ApprovePrint(Image image)
    {
        if(!ApprovedPrints.Contains(image))
        {
            ApprovedPrints.Add(image);
            UpdateSelectionCounts();
        }
    }

    internal void DisApprovePrint(Image image)
    {
        if(ApprovedPrints.Contains(image))
        {
            ApprovedPrints.Remove(image);
            UpdateSelectionCounts();
        }
    }

    internal void MaybeImage()
    {
        CurrentImage.IsPending = !CurrentImage.IsPending;
        DisApproveImage(CurrentImage);
        UpdateSelectionCounts();
    }


    private void UpdateSelectionCounts()
    {
        ApprovedImagesCount = CurrentImages.Count(x => x.IsSelected);
        MaybeCount = CurrentImages.Count(x => x.IsPending);
        ApprovedPrintsCount = CurrentImages.Count(x => x.IsPrintable);
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

        CartTotal = Math.Round(total * (1 + vatPercent),2);

        VatTotal = Math.Round(CartTotal - total,2);
    }
}
