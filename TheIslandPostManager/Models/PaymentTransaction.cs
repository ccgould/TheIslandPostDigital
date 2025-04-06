using CommunityToolkit.Mvvm.ComponentModel;

namespace TheIslandPostManager.Models;
public partial class PaymentTransaction : ObservableObject
{
    public PaymentTransaction(PurchaseType type,decimal amount)
    {
        DateTime = DateTime.Now;
        Amount = amount;
        PurchaseType = type;        
    }


    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public PurchaseType PurchaseType { get; set; }
}
