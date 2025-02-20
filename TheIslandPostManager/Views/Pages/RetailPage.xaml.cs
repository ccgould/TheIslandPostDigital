using System.Windows;
using System.Windows.Controls;
using TheIslandPostManager.Dialogs;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for RetailPage.xaml
/// </summary>
public partial class RetailPage : Page
{
    public CompleteOrderDialogViewModel ViewModel { get; set; }

    public RetailPage(CompleteOrderDialogViewModel vm)
    {
        App.IsRetailPage = true;
        InitializeComponent();
        DataContext = vm;
        vm.GetCartItems();
        ViewModel = vm;
    }

    private void Flyout_Closed(Flyout sender, System.Windows.RoutedEventArgs args)
    {
        ViewModel.OrderService.CurrentOrder.UpdateCartTotal();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        TextBlock1.Visibility = System.Windows.Visibility.Collapsed;
        TextBlock.Visibility = System.Windows.Visibility.Collapsed;


        if (ClerkCmbox.SelectedIndex == -1)
        {
            TextBlock.Visibility = System.Windows.Visibility.Visible;
            ClerkCmbox.Focus();
        }

        if (string.IsNullOrWhiteSpace(PassengerNameTxtb.Text))
        {
            TextBlock1.Visibility = System.Windows.Visibility.Visible;
            PassengerNameTxtb.Focus();
        }
    }
}
