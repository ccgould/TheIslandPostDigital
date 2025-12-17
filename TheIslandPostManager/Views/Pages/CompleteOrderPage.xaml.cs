using System.Windows;
using System.Windows.Controls;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for CompleteOrderPage.xaml
/// </summary>
public partial class CompleteOrderPage : Page
{
    private readonly IMySQLService mySQLService;

    public CompleteOrderDialogViewModel ViewModel { get; set; }

    public CompleteOrderPage(CompleteOrderDialogViewModel vm, IMySQLService mySQLService)
    {
        App.IsRetailPage = false;
        InitializeComponent();
        DataContext = vm;
        vm.GetCartItems();
        ViewModel = vm;
        this.mySQLService = mySQLService;
    }

 

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        TextBlock1.Visibility = Visibility.Collapsed;
        TextBlock.Visibility = Visibility.Collapsed;


        //if (ClerkCmbox.SelectedIndex == -1)
        //{
        //    TextBlock.Visibility = System.Windows.Visibility.Visible;
        //    ClerkCmbox.Focus();
        //}

        //if (string.IsNullOrWhiteSpace(PassengerNameTxtb.Text))
        //{
        //    TextBlock1.Visibility = System.Windows.Visibility.Visible;
        //    PassengerNameTxtb.Focus();
        //}
    }

    private async void SfTextBoxExt_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.Customers = await mySQLService.SearchByEmail(searchBox.Text);
    }
}
