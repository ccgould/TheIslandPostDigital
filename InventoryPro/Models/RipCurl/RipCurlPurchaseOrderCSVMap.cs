using CsvHelper.Configuration;
using InventoryPro.Converters;

namespace InventoryPro.Models.Crocs;

internal class RipCurlPurchaseOrderCSVMap : ClassMap<RipCurlPurchaseOrder>
{
    public RipCurlPurchaseOrderCSVMap()
    {
        //use the converter on double or decimal types
        Map(x => x.OrderNo).Index(0);
        Map(x => x.ItemNumber).Index(1);
        Map(x => x.ShippedQty).Index(2).TypeConverter<IntConverter>();
        Map(x => x.StyleColor).Index(3);
        Map(x => x.Desc).Index(4);
        Map(x => x.Price).Index(5).TypeConverter<DecimalConverter>();
        Map(x => x.Retail).Index(6).TypeConverter<DecimalConverter>();
        Map(x => x.Divison).Index(7);
        Map(x => x.Category).Index(8);
        Map(x => x.Barcode).Index(9);
    }
}