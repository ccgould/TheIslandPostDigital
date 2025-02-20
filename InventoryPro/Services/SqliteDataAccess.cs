using Dapper;
using InventoryPro.Models.Crocs;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace InventoryPro.Services;
public class SqliteDataAccess
{
    public static List<MaterialObject> LoadMaterials()
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            var output = cnn.Query<MaterialObject>("Select * from tbl_material", new DynamicParameters());
            return output.ToList();
        }
    }

    public static MaterialObject FindItemInMaterialsWithUPC(string upc)
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            var output = cnn.Query<MaterialObject>($"Select * from tbl_material Where upc = {upc}");
            foreach (MaterialObject materialObject in output)
            {
                return materialObject;
            }
        }

        return null;
    }

    public static MaterialObject FindUPCInMaterialsWithItem(string materialSize)
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            var output = cnn.Query<MaterialObject>($"Select * from tbl_material Where MaterialSize = '{materialSize}'");
            foreach (MaterialObject materialObject in output)
            {
                return materialObject;
            }
        }

        return null;
    }

    public static void SaveMaterials(MaterialObject material)
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            cnn.Execute("insert into tbl_material (MaterialNumber,MaterialDescription,Color,ColorText,MaterialGroup,MaterialSize,BaseUOM,UPC,EANCategory,EANCategoryDescription,Size,Quality) values (@MaterialNumber,@MaterialDescription,@Color,@ColorText,@MaterialGroup,@MaterialSize,@BaseUOM,@UPC,@EANCategory,@EANCategoryDescription,@Size,@Quality)", material);
        }
    }

    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }

    internal static void AddCorrectedDescription(string original,string edited)
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            cnn.Execute("insert into tbl_descriptions(originalDesc,editedDesc) values (@val,@val2)", new { val = original,val2=edited });
        }
    }

    internal static bool CheckForCorrectedDesc(string description, out string result)
    {
        try
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingleOrDefault<Decription>($"select * from tbl_descriptions where originalDesc = @val", new { val = description });
                if(output is not null)
                {
                    result = output.EditedDesc;
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error", ex.Message);
        }

        result = string.Empty;
        return false;
    }

    internal static bool GetDCSCode(RipCurlPurchaseOrder purchaseOrder,out string result)
    {
        try
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QueryFirstOrDefault<Department>($"select * from tbl_departments where Division = @division AND Category = @category", new { division = purchaseOrder.Divison.Trim(), category = purchaseOrder.Category.Trim() });
                if (output is not null)
                {
                    result = output.DCS;
                    return true;
                }
                else
                {
                   // result = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        result = string.Empty;
        return false;
    }

    internal static string? GetDepartmentsInfo(string column)
    {
        try
        {
            //using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            //{
            //    var output = cnn.QueryFirstOrDefault<Department>($"select * from tbl_departments where Division = @division AND Category = @category", new { division = purchaseOrder.Divison.Trim(), category = purchaseOrder.Category.Trim() });
            //    if (output is not null)
            //    {
            //        //result = output.DCS;
            //        return output.DCS;
            //    }
            //    else
            //    {

            //    }
            //}
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       // result = string.Empty;
        return string.Empty;
    }

    private class Decription
    {
        public string OriginalDesc { get; set; }
        public string EditedDesc { get; set; }
    }
}