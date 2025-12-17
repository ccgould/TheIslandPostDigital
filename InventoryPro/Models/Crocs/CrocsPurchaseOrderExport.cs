
using CsvHelper.Configuration.Attributes;
using InventoryPro.Interface;
using InventoryPro.Services;

namespace InventoryPro.Models.Crocs;
public class CrocsPurchaseOrderExport : IExportObject
{
    public CrocsPurchaseOrderExport()
    {
        
    }
    public CrocsPurchaseOrderExport(CrocsPurchaseOrder purchaseOrder)
    {
        //FigureOutPONumber
        ALU = purchaseOrder.Item;

        if (!purchaseOrder.IsJibbit)
        {
            var split = purchaseOrder.Item.Split('-');
            ATTR = split[1];
            SIZE = split[2];
        }

        DESC1 = FormatDescription(purchaseOrder.Description);
        DESC2 = purchaseOrder.Item;
        Qty = purchaseOrder.Quantity;
        UPC = purchaseOrder.UniversalProductCode;
        //Add Vencode

        if (purchaseOrder.IsJibbit)
        {
            UDF3 = "JIBBITZ";
            UDF4 = "SINGLE";
        }
        else
        {
            UDF3 = DetermineGender(SIZE);
            UDF4 = "CROCS";
        }

        DCS = "CRC";

        POVendCode = "CROCS";

        Vencode = "CROCS";

        POPrice = Math.Round(purchaseOrder.Rate * 2.2m, 2);

        POCOST = purchaseOrder.Rate;

        PRICE = Math.Round(purchaseOrder.Rate * 2.2m, 2);

        //POTYPE BILLTO  SHIPTO POVENDCODE  PO PRICE    POCOST PRICE   POPENDING



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

    [Name("PO NUMBER")]
    public string PONumber { get; set; }
    public string ALU { get; set; }
    public string ATTR { get; set; }
    public string DESC1 { get; set; }
    public string DESC2 { get; set; }
    public string SIZE { get; set; }

    [Name("ORD QTY")]
    public int Qty { get; set; }
    [Name("UPC")]
    public string UPC { get; set; }
    [Name("VENCODE")]
    public string Vencode { get; set; }
    public string UDF3 { get; set; }
    public string UDF4 { get; set; }
    public string DCS { get; set; }
    public int POTYPE { get; set; } = 0;
    public int BILLTO { get; set; } = 1;
    public int SHIPTO { get; set; } = 1;
    [Name("POVENDCODE")]
    public string POVendCode { get; set; }
    [Name("PO PRICE")]
    public decimal POPrice { get; set; }
    public decimal POCOST { get; set; }
    public decimal PRICE { get; set; }
    public int POPENDING { get; set; } = 0;
    public string FilePath { get; set; }

    public POHistory GetPOHistory(bool isCrocs)
    {
        return new POHistory
        {
            FileName = Path.GetFileName(FilePath),
            Store = isCrocs ? Enumerators.Store.Crocs : Enumerators.Store.RipCurl,
            DateString = DateTime.Now.ToString("yyyy-MM-dd"),
            UPC = UPC,
            Qty = Qty
        };
    }

    public override string? ToString()
    {
        return $"{UPC} | {Qty}";
    }
}
