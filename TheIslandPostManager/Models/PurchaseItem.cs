using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows;

namespace TheIslandPostManager.Models;

public partial class PurchaseItem : ObservableObject
{
    public PurchaseItem(int id,string description, string data, decimal cost, int imageCount,int printCount = -1,bool isRetailItem = false, int parent = -1)
    {
        ID = id;
        Description = description;
        Data = data;
        Cost = cost;
        ImageCount = imageCount;
        PrintCount = printCount == -1 ? imageCount : printCount;
        IsRetailItem = isRetailItem;
        Parent = parent;
    }

    public string Description { get; set; }
    public string Data { get; set; }
    public decimal Cost { get; set; }
    public int ID { get; set; }
    public bool IsRetailItem { get; set; }    
    public int Parent { get; set; }


    [ObservableProperty] private decimal totalCost;
    [ObservableProperty] private bool isFlyoutOpen;
    [ObservableProperty] private int imageCount;
    [ObservableProperty] private int printCount;
    [ObservableProperty] private int amount;
    [ObservableProperty] private Visibility amountVisible = Visibility.Collapsed;
    [ObservableProperty] private List<PurchaseItem> childrenItems = new()   ;
    private Action _callBack;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if(e.PropertyName.Equals(nameof(Amount)))
        {
            UpdateValues();
        }
    }

    public void IncrementAmount(PurchaseItem child = null)
    {
        if (child != null)
        {
            child.IncrementAmount();
        }

        Amount += 1;
        ToggleAmountState();
        UpdateValues();
    }

    private void ToggleAmountState() => AmountVisible = Amount > 0 ? Visibility.Visible : Visibility.Collapsed;

    public void ChangeAmount(int amount)
    {

        if(amount == 0)
        {
            Amount = amount;
        }
        else
        {
            Amount += amount;
        }

        ToggleAmountState();
        UpdateValues();
    }

    private void UpdateValues()
    {
        TotalCost = Cost * Amount;
        _callBack?.Invoke();
    }

    public void OpenFlyout(Action callBack = null)
    {
        IsFlyoutOpen  = true;
        _callBack = callBack;
    }

    internal void Copy(PurchaseItem item)
    {
        ChangeAmount(item.Amount);
    }

    internal bool HasChildren()
    {
        return ChildrenItems?.Any() ?? false;
    }

    internal bool IsChild()
    {
        return Parent > -1;
    }
}
