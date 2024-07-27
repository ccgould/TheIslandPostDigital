using System.Windows.Controls;
using TheIslandPostManager.Services;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;

/// <summary>
/// Interaction logic for CompleterOrderDialog.xaml
/// </summary>
public partial class CompleterOrderDialog : ContentDialog
{
    private readonly IMessageService messageService;

    public CompleteOrderDialogViewModel ViewModel { get; set; }
    public CompleterOrderDialog(ContentPresenter contentPresenter,IOrderService orderService, IMessageService messageService) : base(contentPresenter)
    {
        InitializeComponent();

        ViewModel = new CompleteOrderDialogViewModel(orderService);
        
        DataContext = ViewModel;
        this.messageService = messageService;
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if(button == ContentDialogButton.Primary)
        {
            TextBlock1.Visibility = System.Windows.Visibility.Collapsed;
            TextBlock.Visibility = System.Windows.Visibility.Collapsed;

            bool valid = true;

            if (ClerkCmbox.SelectedIndex == -1)
            {
                TextBlock.Visibility = System.Windows.Visibility.Visible;
                ClerkCmbox.Focus();
                valid = false;

            }

            if (string.IsNullOrWhiteSpace(PassengerNameTxtb.Text))
            {
                TextBlock1.Visibility = System.Windows.Visibility.Visible;
                PassengerNameTxtb.Focus();
                valid = false;
            }

            if (valid)
            {
                base.OnButtonClick(button);
                ViewModel.CompleteOrder();
            }
        }
        else
        {
            var result = messageService.ShowMessage("Close Window", "Are you sure you would like to clos this dialog.","NO");

            if(result.Result == MessageBoxResult.Primary)
            {
                base.OnButtonClick(button);
            }
        }


    }

    private void Flyout_Closed(Flyout sender, System.Windows.RoutedEventArgs args)
    {
        ViewModel.OrderService.CurrentOrder.UpdateCartTotal();
    }
}
