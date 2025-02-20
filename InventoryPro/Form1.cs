using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Globalization;
using Serilog;
using InventoryPro.Services;
using InventoryPro.Models.Crocs;

namespace InventoryPro;

public partial class Form1 : Form
{
    private List<CrocsPurchaseOrderExport> _crocsExport;
    
    private List<RipCurlPurchaseOrderExport> _ripCurlExport;

    public Form1()
    {
        InitializeComponent();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.File("Data/Logs/myapp.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

        //SqliteDataAccess.FindItemInMaterialsWithUPC("674236403609");

        //var materials = SqliteDataAccess.LoadMaterials();
    }

    private void browseBtn_Click(object sender, EventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.Filters.Add(new CommonFileDialogFilter("Comma Seperated Values", "CSV"));
        //dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            importTxtB.Text = dialog.FileName;
        }
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
        try
        {
            WriteToStatus("Import Started....");

            if (string.IsNullOrWhiteSpace(importTxtB.Text))
            {
                //Add To Log
                return;
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderChkB.Checked,
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = false,
                BadDataFound = null,
            };


            if (crocsRadBtn.Checked)
            {
                CrocsImport(config);
            }
            else
            {
                RipCurlImport(config);
            }


            WriteToStatus("Import Completed Successfully....");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error(ex, "Error Occured:");
            WriteToStatus("Done with errors....");
        }

    }

    private void RipCurlImport(CsvConfiguration config)
    {
        List<RipCurlPurchaseOrder> purchaseOrders = null;

        using (var reader = new StreamReader(importTxtB.Text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<RipCurlPurchaseOrderCSVMap>();
            purchaseOrders = csv.GetRecords<RipCurlPurchaseOrder>().ToList();
        }

        List<RipCurlPurchaseOrderExport> purchaseOrderExport = new List<RipCurlPurchaseOrderExport>();

        foreach (var purchaseOrder in purchaseOrders)
        {
            //if (string.IsNullOrWhiteSpace(purchaseOrder.Desc))
            //{
            //    var materialResult = SqliteDataAccess.FindItemInMaterialsWithUPC(purchaseOrder.UniversalProductCode);
            //    if (materialResult is not null)
            //    {
            //        purchaseOrder.Description = materialResult.MaterialDescription;
            //    }
            //}
            purchaseOrderExport.Add(new RipCurlPurchaseOrderExport(purchaseOrder));
        }

        var duplicates = purchaseOrderExport.GroupBy(x => new { x.LocalUPC })
                 .Where(x => x.Skip(1).Any());

        int i = duplicates.Count();

        if (duplicates.Any())
        {
            foreach (var dupli in duplicates)
            {
                i -= 1;
                var dupl = new RipCurlDuplicateForm(dupli, purchaseOrderExport, i);
                dupl.ShowDialog();
            }
        }

        if (!validateChkB.Checked)
        {
            _ripCurlExport = purchaseOrderExport;
        }
        else
        {
            _ripCurlExport = null;
        }
    }

    private void CrocsImport(CsvConfiguration config)
    {
        List<CrocsPurchaseOrder> purchaseOrders = null;

        using (var reader = new StreamReader(importTxtB.Text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<CrocsPurchaseOrderCSVMap>();
            purchaseOrders = csv.GetRecords<CrocsPurchaseOrder>().ToList();
        }

        List<CrocsPurchaseOrderExport> purchaseOrderExport = new List<CrocsPurchaseOrderExport>();

        foreach (var purchaseOrder in purchaseOrders)
        {
            if (string.IsNullOrWhiteSpace(purchaseOrder.Description))
            {
                var materialResult = SqliteDataAccess.FindItemInMaterialsWithUPC(purchaseOrder.UniversalProductCode);
                if (materialResult is not null)
                {
                    purchaseOrder.Description = materialResult.MaterialDescription;
                }
            }

            if (string.IsNullOrWhiteSpace(purchaseOrder.UniversalProductCode))
            {
                var materialResult = SqliteDataAccess.FindUPCInMaterialsWithItem(purchaseOrder.Item);
                if (materialResult is not null)
                {
                    purchaseOrder.UniversalProductCode = materialResult.UPC;
                }
                else
                {
                    WriteToStatus($"Cannot find item with item number {purchaseOrder.Item}");
                }
            }
            purchaseOrderExport.Add(new CrocsPurchaseOrderExport(purchaseOrder));
        }

        var duplicates = purchaseOrderExport.GroupBy(x => new { x.Upc })
                 .Where(x => x.Skip(1).Any());

        int i = duplicates.Count();

        if (duplicates.Any())
        {
            foreach (var dupli in duplicates)
            {
                i -= 1;
                var dupl = new DuplicateForm(dupli, purchaseOrderExport, i);
                dupl.ShowDialog();
            }
        }

        if (!validateChkB.Checked)
        {
            _crocsExport = purchaseOrderExport;
        }
        else
        {
            _crocsExport = null;
        }
    }

    private void WriteToStatus(string info)
    {
        Log.Information(info);
        statusTxtB.Text = info;
    }

    private void CreateCSVFile(List<CrocsPurchaseOrderExport> purchaseOrderExport, string path)
    {
        if(!validateChkB.Checked)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(purchaseOrderExport);
            }
        }
        else
        {
            MessageBox.Show("Cannot export in validate mode.", "Attention", MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
    }

    private void CreateCSVFileRip(List<RipCurlPurchaseOrderExport> purchaseOrderExport, string path)
    {
        if (!validateChkB.Checked)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(purchaseOrderExport);
            }
        }
        else
        {
            MessageBox.Show("Cannot export in validate mode.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void exportBtn_Click(object sender, EventArgs e)
    {
        try
        {
            var saveDialog = new CommonSaveFileDialog();
            saveDialog.Filters.Add(new CommonFileDialogFilter("Comma Seperated Value", ".csv"));

            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);



            if (saveDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                WriteToStatus("Export Started....");

                if(crocsRadBtn.Checked)
                {
                    CreateCSVFile(_crocsExport, saveDialog.FileName);
                }
                else
                {
                    CreateCSVFileRip(_ripCurlExport, saveDialog.FileName);
                }

                WriteToStatus("Export Completed Successfully....");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error(ex, "Error Occured:");
            WriteToStatus("Done with errors...."); ;
        }
    }

    private void logBtn_Click(object sender, EventArgs e)
    {
        var logViewer = new LogViewer();
        logViewer.ShowDialog();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            IgnoreBlankLines = false,
            BadDataFound = null,
        };

        List<MaterialObject> purchaseOrders = null;

        using (var reader = new StreamReader("F:\\Crocs\\Material Master Report 2024 - Copy.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<MaterialCSVMap>();
            purchaseOrders = csv.GetRecords<MaterialObject>().ToList();
        }

        foreach (var purchaseOrder in purchaseOrders)
        {
            SqliteDataAccess.SaveMaterials(purchaseOrder);
        }
    }
}
