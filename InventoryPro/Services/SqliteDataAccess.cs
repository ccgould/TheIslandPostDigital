using InventoryPro.Enumerators;
using InventoryPro.Models;
using InventoryPro.Models.Crocs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace InventoryPro.Services;

public static partial class SqliteDataAccess
{
    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }
    private static SQLiteConnection GetConnection()
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "materials.db");
        return new SQLiteConnection(dbPath);

    }

    public static List<MaterialObject> LoadMaterials()
    {
        using var db = GetConnection();
        return db.Table<MaterialObject>().ToList();
    }

    public static MaterialObject FindItemInMaterialsWithUPC(string upc)
    {
        using var db = GetConnection();
        return db.Table<MaterialObject>().FirstOrDefault(m => m.UPC == upc);
    }

    public static MaterialObject FindUPCInMaterialsWithItem(string materialSize)
    {
        using var db = GetConnection();
        return db.Table<MaterialObject>().FirstOrDefault(m => m.MaterialSize == materialSize);
    }

    public static void SaveMaterials(MaterialObject material)
    {
        using var db = GetConnection();
        db.Insert(material);
    }

    public static void AddCorrectedDescription(string original, string edited)
    {
        using var db = GetConnection();
        db.Insert(new Description { OriginalDesc = original, EditedDesc = edited });
    }

    public static bool CheckForCorrectedDesc(string description, out string result)
    {
        using var db = GetConnection();
        var match = db.Table<Description>().FirstOrDefault(d => d.OriginalDesc == description);
        if (match != null)
        {
            result = match.EditedDesc;
            return true;
        }

        result = string.Empty;
        return false;
    }

    public static bool GetDCSCode(RipCurlPurchaseOrder purchaseOrder, out string result)
    {
        using var db = GetConnection();
        //var match = db.Table<Department>().FirstOrDefault(d =>
        //    d.Division.Trim() == purchaseOrder.Divison &&
        //    d.Category.Trim() == purchaseOrder.Category);

        Department match = null;

        foreach (Department department in db.Table<Department>())
        {
            if(department.Division.Trim().Equals(purchaseOrder.Divison.Trim()) &&
                department.Category.Trim().Equals(purchaseOrder.Category.Trim()))
            {
                match = department;
                break;  
            }
        }

        if (match != null)
        {
            result = match.DCS;
            return true;
        }

        result = string.Empty;
        return false;
    }

    public static void InsertPOHistory(List<POHistory> historyList)
    {
        using var db = GetConnection();
        db.RunInTransaction(() =>
        {
            foreach (var entry in historyList)
            {
                db.Insert(entry);
            }
        });
    }

    public static List<POHistory> GetFilteredHistory(DateTime date, string upc, string fileName)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        using var db = GetConnection();

        var query = db.Table<POHistory>().Where(p => p.DateString == dateString);

        if (!string.IsNullOrWhiteSpace(upc))
        {
            query = query.Where(p => p.UPC == upc);
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            query = query.Where(p => p.FileName.Contains(fileName));
        }

        return query.ToList();
    }


    // CREATE
    public static void InsertDepartment(Department department)
    {
        using var db = GetConnection();
        db.Insert(department);
    }

    // READ ALL
    public static List<Department> LoadAllDepartments()
    {
        using var db = GetConnection();
        return db.Table<Department>().ToList();
    }

    // READ BY Division + Category
    public static Department GetByDivisionAndCategory(string division, string category)
    {
        using var db = GetConnection();
        return db.Table<Department>()
                 .FirstOrDefault(d => d.Division == division && d.Category == category);
    }

    // UPDATE
    public static void UpdateDepartment(Department department)
    {
        using var db = GetConnection();
        db.Update(department);
    }

    // DELETE
    public static void DeleteDepartment(int id)
    {
        using var db = GetConnection();
        var dept = db.Table<Department>().FirstOrDefault(d => d.Id == id);
        if (dept != null)
        {
            db.Delete(dept);
        }
    }

}