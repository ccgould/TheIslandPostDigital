using CsvHelper.Configuration;
using InventoryPro.Converters;

namespace InventoryPro.Models.Crocs;

internal class CrocsPurchaseOrderCSVMap : ClassMap<CrocsPurchaseOrder>
{
    public CrocsPurchaseOrderCSVMap()
    {
        //use the converter on double or decimal types
        Map(x => x.Item).Index(0);
        Map(x => x.Description).Index(1);
        Map(x => x.UniversalProductCode).Index(2);
        Map(x => x.Quantity).Index(3).TypeConverter<IntConverter>();
        Map(x => x.Rate).Index(4).TypeConverter<DecimalConverter>();
    }
}
