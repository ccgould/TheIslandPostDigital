using CsvHelper.Configuration;
using InventoryPro.Converters;

namespace InventoryPro.Models.Crocs;

internal class CrocsPurchaseOrderCSVMap : ClassMap<CrocsPurchaseOrder>
{
    public CrocsPurchaseOrderCSVMap()
    {
        //use the converter on double or decimal types
        Map(x => x.Item).Name("Item");
        Map(x => x.Description).Name("Description");
        Map(x => x.UniversalProductCode).Name("UPC");
        Map(x => x.Quantity).Name("Qty").TypeConverterOption.NullValues("").TypeConverter<IntConverter>();
        Map(x => x.Rate).Name("Rate").TypeConverterOption.NullValues("").TypeConverter<DecimalConverter>();
    }
}
