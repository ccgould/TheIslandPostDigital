namespace InventoryPro.Models.Crocs;

public class CrocsPurchaseOrder
{
    private string item;

    public string Item
    {
        get => item;
        set
        {
            item = value;
            if (!string.IsNullOrWhiteSpace(item))
            {
                IsJibbit = !item.Contains('-');
            }
        }
    }
    public string Description { get; set; }
    public string UniversalProductCode { get; set; }
    public int Quantity { get; set; }
    public decimal Rate { get; set; }

    public bool IsJibbit { get; set; }
}