using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;
namespace TheIslandPostManager.Dialogs;
/// <summary>
/// Interaction logic for OverrideDilalog.xaml
/// </summary>
public partial class OverrideDialog : ContentDialog,INotifyPropertyChanged
{
    private string link;

    public string Link
    {
        get
        {
            return link;
        }

        set
        {
            link = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Employee> admins;

    public ObservableCollection<Employee> Admins
    {
        get
        {
            return admins;
        }

        set
        {
            admins = value;
            OnPropertyChanged();
        }
    }

    private Employee selectedItem;

    public Employee SelectedItem
    {
        get { return selectedItem; }
        set { selectedItem = value; }
    }

    public OverrideDialog(ContentPresenter contentPresenter,IMySQLService mySQLService) : base(contentPresenter)
    {
       InitializeComponent();

        Admins = mySQLService.GetEmployees(true).Result;
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        Link = passwordBox.Password;
        base.OnButtonClick(button);
    }
}
