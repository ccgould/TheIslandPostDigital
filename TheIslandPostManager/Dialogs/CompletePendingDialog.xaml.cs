using System.Windows.Controls;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;
/// <summary>
/// Interaction logic for CompletePendingDialog.xaml
/// </summary>
public partial class CompletePendingDialog : ContentDialog
{
    public CompletePendingDialogViewModel ViewModel { get; private set; }
    public CompletePendingDialog(ContentPresenter contentPresenter,IMySQLService mySQLService) : base(contentPresenter)
    {
        InitializeComponent();
        var vm = new CompletePendingDialogViewModel(mySQLService);
        DataContext = vm;
        ViewModel = vm;
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if (button == ContentDialogButton.Primary)
        {
            //TextBlock1.Visibility = System.Windows.Visibility.Collapsed;
            //TextBlock.Visibility = System.Windows.Visibility.Collapsed;
        }
              
        base.OnButtonClick(button);
    }
}
