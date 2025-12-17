using System.Windows;
using System.Windows.Controls;
using TheIslandPostManager.ViewModels;

namespace TheIslandPostManager.Views.Pages;

/// <summary>
/// Interaction logic for CheckOutPage.xaml
/// </summary>
public partial class CheckOutPage : Page
{
    public CheckOutPage(CheckoutPageViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }

    private void TextBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        MessageBox.Show("It Worked");
    }
}
