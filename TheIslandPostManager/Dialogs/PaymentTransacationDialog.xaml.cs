using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;
/// <summary>
/// Interaction logic for PaymentTransacationDialog.xaml
/// </summary>
public partial class PaymentTransacationDialog : ContentDialog, INotifyPropertyChanged
{
    private string numberView;

    public string NumberView
    {
        get { return numberView; }
        set
        {
            numberView = value;
            OnPropertyChanged(nameof(NumberView));
        }
    }

    private string balance;

    public string Balance
    {
        get { return balance; }
        set
        {
            balance = value;
            OnPropertyChanged(nameof(Balance));
        }
    }

    private string change;

    public string Change
    {
        get { return change; }
        set
        {
            change = value;
            OnPropertyChanged(nameof(Change));
        }
    }


    private string totalAmount;

    public string TotalAmount
    {
        get { return totalAmount; }
        set
        {
            totalAmount = value;
            OnPropertyChanged(nameof(TotalAmount));
        }
    }

    private string amount;
    private decimal _totalAmount;

    public event PropertyChangedEventHandler? PropertyChanged;

    public PaymentTransacationDialog(ContentPresenter contentPresenter, decimal totalAmount) : base(contentPresenter)
    {
        _totalAmount = totalAmount;
        InitializeComponent();
        NumberView = ConvertToMoney("0");
        Change = ConvertToMoney("0");
        Balance = ConvertToMoney("0");
        TotalAmount = ConvertToMoney(_totalAmount.ToString());
        DataContext = this;
    }

    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    [RelayCommand]
    private void AddNumber(string value)
    {
        amount += value;
        NumberView = ConvertToMoney(amount);
        Evaluate();

    }

    private void Evaluate()
    {
        var decAmount = decimal.Parse(amount) * 0.01m;

        var change = decAmount - ConvertToDecimal(_totalAmount);

        if (change > 0m)
        {
            Change = ConvertToMoney(change.ToString(), false);
        }
        else
        {
            Change = ConvertToMoney("0");
        }



        var balance = ConvertToDecimal(_totalAmount) - decAmount;

        if (balance < _totalAmount && balance > 0m)
        {

            Balance = ConvertToMoney(balance.ToString(), false);
        }
        else
        {
            Balance = ConvertToMoney("0");
        }
    }

    [RelayCommand]
    private void ChangeAmount(string amount)
    {
        //var convertedAmount = Convert.ToInt32(this.amount) + Convert.ToInt32(amount);
        //this.amount = convertedAmount.ToString();
        this.amount = amount;
        NumberView = ConvertToMoney(this.amount);
        Evaluate();
    }

    [RelayCommand]
    private void AddAmount(string amount)
    {
        var convertedAmount = Convert.ToInt32(this.amount) + Convert.ToInt32(amount);
        this.amount = convertedAmount.ToString();
        //this.amount = amount;
        NumberView = ConvertToMoney(this.amount);
        Evaluate();
    }

    [RelayCommand]
    private void BackSpace()
    {

        if (amount.Length > 0)
        {
            amount = amount.Remove(amount.Length - 1, 1);
        }

        if (amount == string.Empty)
        {
            this.amount = "0";
        }

        NumberView = ConvertToMoney(amount);
        Evaluate();
    }

    private string ConvertToMoney(string input, bool convert = true)
    {
        var cultureInfo = Thread.CurrentThread.CurrentCulture;   // You can also hardcode the culture, e.g. var cultureInfo = new CultureInfo("fr-FR"), but then you lose culture-specific formatting such as decimal point (. or ,) or the position of the currency symbol (before or after)
        var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
        numberFormatInfo.CurrencySymbol = "$"; // Replace with "$" or "£" or whatever you need

        if (decimal.TryParse(input, out decimal result))
        {
            decimal price = 0m;

            if (convert)
            {
                price = ConvertToDecimal(result);
            }
            else
            {
                price = result;
            }

            return price.ToString("C", numberFormatInfo); // Output: "€ 12.30" if the CurrentCulture is "en-US", "12,30 €" if the CurrentCulture is "fr-FR".
        }

        return "$0";
    }

    [RelayCommand]
    private void Clear()
    {
        amount = "0";
        NumberView = ConvertToMoney(amount);
        Evaluate();
    }

    private decimal ConvertToDecimal(decimal result)
    {
        return result * 0.01m;
    }
    private void CancelBTN_Click(object sender, RoutedEventArgs e)
    {
        Hide(ContentDialogResult.None);
    }

    private void DoneBTN_Click(object sender, RoutedEventArgs e)
    {
        Hide(ContentDialogResult.Primary);
    }

    internal decimal GetBalanceAsDecimal()
    {
        return ConvertToDecimal(decimal.Parse(amount));
    }
}
