using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;

/// <summary>
/// Interaction logic for PaymentAmountDialog.xaml
/// </summary>
public partial class PaymentAmountDialog : ContentDialog
{
    public decimal Amount { get; set; }
    public decimal PayoutTotal { get; set; }
    public decimal RemainingTotal { get; set; }
    public PaymentAmountDialog(ContentPresenter contentPresenter, decimal amount) : base(contentPresenter)
    {
        Amount = amount;
        PayoutTotal = Amount;
        InitializeComponent();
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if (button == ContentDialogButton.Primary)
        {
            if(PayoutTotal <= 0)
            {
                return;
            }

            RemainingTotal = PayoutTotal - Amount;
        }

        base.OnButtonClick(button);
    }
}
