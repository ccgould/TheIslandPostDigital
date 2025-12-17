
using SQLite;

namespace InventoryPro.Services;

[Table("tbl_departments")]
public class Department
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // Optional: include if your table has a primary key

    [Column("Division")]
    public string Division { get; set; }

    [Column("Category")]
    public string Category { get; set; }

    [Column("DCS")]
    public string DCS { get; set; }
}