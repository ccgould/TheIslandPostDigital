
using CsvHelper.Configuration.Attributes;
using InventoryPro.Interface;
using InventoryPro.Services;

namespace InventoryPro.Models.Crocs;
public class RipCurlPurchaseOrderExport : IExportObject
{
    public RipCurlPurchaseOrderExport()
    {
        
    }
    public RipCurlPurchaseOrderExport(RipCurlPurchaseOrder purchaseOrder)
    {
        LocalUPC = purchaseOrder.Barcode;
        Attr = GetAttr(purchaseOrder.StyleColor) ?? string.Empty;
        Size = GetSize(purchaseOrder.ItemNumber) ?? string.Empty;
        Desc1 = purchaseOrder.Desc;
        Desc2 = GetDesc2(purchaseOrder.StyleColor) ?? string.Empty;
        Price = Math.Round(purchaseOrder.Retail,2);

        if(SqliteDataAccess.GetDCSCode(purchaseOrder, out var result))
        {
            DCSCode = result;
        }

        Cost = Math.Round(purchaseOrder.Price, 2);
        UPC = purchaseOrder.Barcode;
        PONumber = purchaseOrder.OrderNo;
        OrderDate = DateTime.Now.ToString("MM/dd/yyyy");
        ShipDate = DateTime.Now.ToString("MM/dd/yyyy");
        CancelDate = DateTime.Now.ToString("MM/dd/yyyy");
        POPrice = Math.Round(purchaseOrder.Retail, 2);
        POCost = Math.Round(purchaseOrder.Price, 2);
        Qty = purchaseOrder.ShippedQty;
    }

    private string? GetAttr(string styleColor)
    {
        return styleColor?.Substring(styleColor.Length - 4, 4);
    }

    private string? GetSize(string itemNumber)
    {
        return itemNumber?.Substring(10);
    }

    private string? GetDesc2(string styleColor)
    {
        return styleColor?.Substring(0, styleColor.Length - 4);
    }

    private string? FormatDescription(string description)
    {
        if (description.Length > 30)
        {
            if (SqliteDataAccess.CheckForCorrectedDesc(description, out string result))
            {
                return result;
            }
            else
            {
                var descEditor = new DescriptionEditor(description);
                descEditor.ShowDialog();

                return descEditor.Result;
            }
        }

        return description;
    }

    private string DetermineGender(string size)
    {
        string result = string.Empty;

        if (!string.IsNullOrWhiteSpace(size))
        {
            if (size.Contains("M", StringComparison.OrdinalIgnoreCase) && size.Contains("W", StringComparison.OrdinalIgnoreCase))
            {
                return "Unisex";
            }

            if (size.Contains("C", StringComparison.OrdinalIgnoreCase) || size.Contains("J", StringComparison.OrdinalIgnoreCase))
            {
                return "Kids";
            }

            if (size.StartsWith("M", StringComparison.OrdinalIgnoreCase))
            {
                return "Male";
            }

            if (size.StartsWith("W", StringComparison.OrdinalIgnoreCase))
            {
                return "Woman";
            }
        }

        return result;
    }

    [Name("Local UPC")]
    public string LocalUPC { get; set; }
    [Name("DCS Code")]
    public string DCSCode { get; set; }
    public string Attr { get; set; }
    public string Size { get; set; }
    [Name("Desc 1")]
    public string Desc1 { get; set; }
    [Name("Desc 2")]
    public string Desc2 { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    [Name("Tax Code")]
    public int TaxCode { get; set; } = 0;
    public string UPC { get; set; }
    [Name("Vendor Code")]
    public string VendorCode { get; set; } = "RCU";
    [Name("PO Nunmber")]
    public string PONumber { get; set; }
    [Name("PO Vendor Code")]
    public string POVendorCode { get; set; } = "RCU";
    [Name("Order Date")]
    public string OrderDate { get; set; }
    [Name("Ship Date")]
    public string ShipDate { get; set; }
    [Name("Cancel Date")]
    public string CancelDate { get; set; }
    [Name("PO Price")]
    public decimal POPrice { get; set; }
    [Name("PO Cost")]
    public decimal POCost { get; set; }
    [Name("Order Qty")]
    public int Qty { get; set; }
    [Name("Bill To Store Number")]
    public int BillToStoreNumber { get; set; } = 0;
    [Name("Ship To Store Number")]
    public int ShipToStoreNumber { get; set; } = 0;
    public string FilePath { get; set; }

    public override string? ToString()
    {
        return $"{LocalUPC} | {Qty}";
    }

    public POHistory GetPOHistory(bool isCrocs)
    {

        return new POHistory
        {
            FileName = Path.GetFileName(FilePath),
            Store = isCrocs ? Enumerators.Store.Crocs : Enumerators.Store.RipCurl,
            DateString = DateTime.Now.ToString("yyyy-MM-dd"),
            UPC = this.UPC,
            Qty = this.Qty
        };
    }
}
