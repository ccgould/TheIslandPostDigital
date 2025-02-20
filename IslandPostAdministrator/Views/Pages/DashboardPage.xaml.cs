using IslandPostAdministrator.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace IslandPostAdministrator.Views.Pages;
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

    private void NumericalAxis_LabelCreated(object sender, Syncfusion.UI.Xaml.Charts.LabelCreatedEventArgs e)
    {
        var axisLabel = e.AxisLabel;
        axisLabel.LabelContent = axisLabel.Position == 0 ? axisLabel.LabelContent :
                    (double.Parse((string)axisLabel.LabelContent) >= 1000) ?
                    "$" + (Math.Round((double.Parse((string)axisLabel.LabelContent) / 1000), 0)) + "K" :
                    "$" + axisLabel.LabelContent;
    }

    private void sfDatePicker_ValueChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        var date = (DateTime)e.NewValue;
        ViewModel.GetDailyBalance(date);
    }
}
