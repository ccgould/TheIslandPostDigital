using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheIslandPostManager.Dialogs;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for CompleteOrderPage.xaml
/// </summary>
public partial class CompleteOrderPage : Page
{
    public CompleteOrderDialogViewModel ViewModel { get; set; }

    public CompleteOrderPage(CompleteOrderDialogViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
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
