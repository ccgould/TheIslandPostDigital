using CsvHelper.Configuration;

namespace InventoryPro.Models.Crocs;

internal class MaterialCSVMap : ClassMap<MaterialObject>
{
    public MaterialCSVMap()
    {
        //use the converter on double or decimal types
        Map(x => x.MaterialNumber).Index(0);
        Map(x => x.MaterialDescription).Index(1);
        Map(x => x.Color).Index(2);
        Map(x => x.ColorText).Index(3);
        Map(x => x.MaterialGroup).Index(4);
        Map(x => x.MaterialSize).Index(5);
        Map(x => x.BaseUOM).Index(6);
        Map(x => x.UPC).Index(7);
        Map(x => x.EANCategory).Index(8);
        Map(x => x.EANCategoryDescription).Index(9);
        Map(x => x.Size).Index(10);
        Map(x => x.Quality).Index(11);
    }
}
