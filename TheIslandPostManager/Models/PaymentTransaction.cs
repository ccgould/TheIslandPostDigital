using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIslandPostManager.Models;
public partial class PaymentTransaction : ObservableObject
{
    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public PurchaseType PurchaseType { get; set; }
}
