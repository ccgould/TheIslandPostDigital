using System.Windows;
using TheIslandPostManager.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for OrderHistoryEditorPage.xaml
/// </summary>
public partial class OrderHistoryEditorPage : INavigableView<OrderHistoryEditorPageViewmodel>
{
    public OrderHistoryEditorPage(OrderHistoryEditorPageViewmodel vm)
    {
        InitializeComponent();
        ViewModel = vm;
        DataContext = ViewModel;
    }

    public OrderHistoryEditorPageViewmodel ViewModel { get; }
}
