using CommunityToolkit.Mvvm.ComponentModel;

namespace TheIslandPostManager.Models;
public partial class PaymentTransaction : ObservableObject
{
    public PaymentTransaction(PurchaseType type,decimal amount)
    {
        Date = DateOnly.FromDateTime(DateTime.Now);
        Time = TimeOnly.FromDateTime(DateTime.Now);
        Amount = amount;
        PurchaseType = type;        
    }

    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public decimal Amount { get; set; }
    public PurchaseType PurchaseType { get; set; }
}
