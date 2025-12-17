using SQLite;
using InventoryPro.Enumerators;

namespace InventoryPro.Models;

[Table("tbl_history")]
public class POHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Column("fileName")]
    public string FileName { get; set; }

    [Column("store")]
    public Store Store { get; set; }

    [Column("date")]
    public string DateString { get; set; }

    [Column("upc")]
    public string UPC { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    public override string ToString()
    {
        return $"{UPC} | {Qty} on {DateString}";
    }
}