using SQLite;

namespace InventoryPro.Services;

public static partial class SqliteDataAccess
{
    // Internal helper class for corrected descriptions
    [Table("tbl_descriptions")]
    private class Description
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string OriginalDesc { get; set; }
        public string EditedDesc { get; set; }
    }
}