using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TheIslandPostManager.Models;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;

/// <summary>
/// Interaction logic for RetailItemsListPopup.xaml
/// </summary>
public partial class RetailItemsListPopup : ContentDialog, INotifyPropertyChanged
{
    public PurchaseItem SelectedItem { get; private set; }

    private int selectedIndex;

    public int SelectedIndex
    {
        get
        {
            return selectedIndex;
        }

        set
        {
            selectedIndex = value;
            OnPropertyChanged();
        }
    }


    private ObservableCollection<PurchaseItem> items;

    public ObservableCollection<PurchaseItem> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public RetailItemsListPopup(ContentPresenter contentPresenter, List<PurchaseItem> purchaseItems) : base(contentPresenter)
    {
        InitializeComponent();

        Items = new();

        foreach (var item in purchaseItems)
        {
            Items.Add(item);
        };
    }


    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if(button == ContentDialogButton.Primary)
        {
            SelectedItem = Items[SelectedIndex];
        }

        base.OnButtonClick(button);
    }
}
