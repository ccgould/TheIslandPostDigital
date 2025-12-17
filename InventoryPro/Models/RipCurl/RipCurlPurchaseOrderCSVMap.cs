using CsvHelper.Configuration;
using InventoryPro.Converters;

namespace InventoryPro.Models.Crocs;

internal class RipCurlPurchaseOrderCSVMap : ClassMap<RipCurlPurchaseOrder>
{
    public RipCurlPurchaseOrderCSVMap()
    {
        //use the converter on double or decimal types
        Map(x => x.OrderNo).Name("Order no");
        Map(x => x.ItemNumber).Name("Item number");
        Map(x => x.ShippedQty).Name("Shipped qty").TypeConverter<IntConverter>();
        Map(x => x.StyleColor).Name("Style_Color");
        Map(x => x.Desc).Name("Desc");
        Map(x => x.Price).Name("Price").TypeConverter<DecimalConverter>();
        Map(x => x.Retail).Name("Retail").TypeConverter<DecimalConverter>();
        Map(x => x.Divison).Name("Division");
        Map(x => x.Category).Name("Category");
        Map(x => x.Barcode).Name("Barcode");
    }
}