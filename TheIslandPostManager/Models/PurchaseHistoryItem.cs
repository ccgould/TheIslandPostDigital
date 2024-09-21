using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIslandPostManager.Models;

/// <summary>
/// This class is used to display the details of an item in a order for purchase history list
/// </summary>
public class PurchaseHistoryItem
{
    public int PhotoAccount { get; set; }
    public int PrintCount { get; set; }
    public string ImageLocation { get; set; }
    public int RetailCount { get; set; }
    public string Description { get; set; }
}
