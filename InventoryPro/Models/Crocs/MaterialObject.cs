using SQLite;

namespace InventoryPro.Models.Crocs;

[Table("tbl_material")]
public class MaterialObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // Optional: add if your table has a primary key

    [Column("MaterialNumber")]
    public string MaterialNumber { get; set; }

    [Column("MaterialDescription")]
    public string MaterialDescription { get; set; }

    [Column("Color")]
    public string Color { get; set; }

    [Column("ColorText")]
    public string ColorText { get; set; }

    [Column("MaterialGroup")]
    public string MaterialGroup { get; set; }

    [Column("MaterialSize")]
    public string MaterialSize { get; set; }

    [Column("BaseUOM")]
    public string BaseUOM { get; set; }

    [Column("UPC")]
    public string UPC { get; set; }

    [Column("EANCategory")]
    public string EANCategory { get; set; }

    [Column("EANCategoryDescription")]
    public string EANCategoryDescription { get; set; }

    [Column("Size")]
    public string Size { get; set; }

    [Column("Quality")]
    public string Quality { get; set; }
}