using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows;

namespace TheIslandPostManager.Models;

public partial class PurchaseItem : ObservableObject
{
    public PurchaseItem(string description, string data, decimal cost)
    {
        Description = description;
        Data = data;
        Cost = cost;
    }

    public string Description { get; set; }
    public string Data { get; set; }
    public decimal Cost { get; set; }
    [ObservableProperty] private decimal totalCost;
    [ObservableProperty] private bool isFlyoutOpen;

    [ObservableProperty] private int amount;
    [ObservableProperty] private Visibility amountVisible = Visibility.Collapsed;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if(e.PropertyName.Equals(nameof(Amount)))
        {
            UpdateValues();
        }
    }

    public void IncrementAmount()
    {
        Amount += 1;
        ToggleAmountState();
        UpdateValues();
    }

    private void ToggleAmountState() => AmountVisible = Amount > 0 ? Visibility.Visible : Visibility.Collapsed;

    public void ChangeAmount(int amount)
    {
        Amount = amount;
        ToggleAmountState();
        UpdateValues();
    }

    private void UpdateValues()
    {
        TotalCost = Cost * Amount;
    }

    public void OpenFlyout()
    {
        IsFlyoutOpen  = true;
    }
}
