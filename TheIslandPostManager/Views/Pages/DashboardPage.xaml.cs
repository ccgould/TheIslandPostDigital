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
    }

    private void txtNum_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }

    private void cmdUp_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void cmdDown_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
