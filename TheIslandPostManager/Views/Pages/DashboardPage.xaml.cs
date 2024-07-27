using System.Windows.Input;
using TheIslandPostManager.ViewModels;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for DashboardPage.xaml
/// </summary>
public partial class DashboardPage : INavigableView<DashboardViewModel>
{
    public DashboardViewModel ViewModel { get; }

    public DashboardPage(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
        dataPage.Focus();
    }

    //private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
    //{
    //    switch (e.Key)
    //    {
    //        case Key.Left:
    //        case Key.Right:
    //        case Key.Up:
    //        case Key.Down:
    //            e.Handled = true;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void txtNum_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }

    private void cmdUp_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void cmdDown_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void ordersListBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                ViewModel.PreviousPhoto();
                e.Handled = true;
                break;
            case Key.Right:
                ViewModel.NextPhoto();
                e.Handled = true;
                break;
            case Key.Up:
                ViewModel.SelectPhoto();
                e.Handled = true;
                break;
            case Key.Down:
                ViewModel.AttemptDislikePhoto();
                e.Handled = true;
                break;
            default:
                break;
        }
    }
}
