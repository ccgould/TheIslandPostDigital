using CsvHelper;
using CsvHelper.Configuration;
using InventoryPro.Interface;
using InventoryPro.Models.Crocs;
using InventoryPro.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using Serilog;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Forms;

namespace InventoryPro;

public partial class Form1 : Form
{
    private List<IExportObject> _exports = new();

    private List<RipCurlPurchaseOrder> purchaseRecordsRP = new();
    private List<CrocsPurchaseOrder> purchaseRecordsCrocs = new();
    private int _mergesCount;
    private List<IExportObject> _pendingExports = new();

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


        var page = new MaxLengthCorrectionPage();
        page.ShowDialog();
    }

    private void browseBtn_Click(object sender, EventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.Filters.Add(new CommonFileDialogFilter("Comma Seperated Values", "CSV"));
        dialog.Multiselect = true;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            foreach (var fileName in dialog.FileNames)
            {
                if (IsDuplicatePath(dataGridView1, fileName))
                {
                    continue;
                }

                int rowIndex = dataGridView1.Rows.Add(); // Adds a new, empty row and returns its index
                dataGridView1.Rows[rowIndex].Cells[0].Value = fileName;
                importBtn.Enabled = true;   
            }
        }
    }

    bool IsDuplicatePath(DataGridView dgv, string path)
    {
        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (!row.IsNewRow &&
                row.Cells["Path"].Value?.ToString() == path)
            {
                return true;
            }
        }
        return false;
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
        bool passed = false;

        try
        {
            WriteToStatus("Import Started....");

            importBtn.Enabled = false;
            var expectedHeader = new[] { "item", "description", "", "msrp", "", "upc", "qty", "rate", "amount" };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderChkB.Checked,
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = false,
                BadDataFound = null,
                ShouldSkipRecord = (row) =>
                {
                    if (!crocsRadBtn.Checked) return false;

                    var normalized = row.Row.Parser.Record.Select(r => r.Trim().ToLower()).ToArray();

                    if (normalized.SequenceEqual(expectedHeader) && row.Row.Parser.Row > 4)
                    {
                        return true;
                    }


                    return string.IsNullOrWhiteSpace(row.Row[0]) || row.Row.Parser.Row < 4;
                }
            };

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No files to import. Please add files first.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                WriteToStatus("Import Aborted: No files to import.");
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (crocsRadBtn.Checked)
                {
                    passed = CrocsImport(config, row.Cells["Path"].Value.ToString());
                }
                else
                {
                    passed = RipCurlImport(config, row.Cells["Path"].Value.ToString());
                }
            }

            if (passed)
            {
                MergeDuplicated();

                exportBtn.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error(ex, "Error Occured:");
            exportBtn.Enabled = false;
            importBtn.Enabled = true;
        }

        WriteToStatus(passed ? "Import Completed Successfully...." : "Done with errors....");
    }

    private void deleteSelectedButton_Click(object sender, EventArgs e)
    {
        // Optional: Add a confirmation dialog
        if (MessageBox.Show("Are you sure you want to delete the selected row(s)?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
            return; // User cancelled the operation
        }

        var selectedRow = dataGridView1.CurrentRow;
        if (selectedRow != null && !selectedRow.IsNewRow)
        {
            dataGridView1.Rows.RemoveAt(selectedRow.Index);
        }


        // If bound to a DataTable and you marked rows for deletion,
        // you might need to update the database and then call AcceptChanges on the DataTable.
        // Example:
        //myDataTable.AcceptChanges(); // Commits changes to the DataTable
    }

    private bool RipCurlImport(CsvConfiguration config, string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<RipCurlPurchaseOrderCSVMap>();

            while (csv.Read())
            {

                try
                {
                    var record = csv.GetRecord<RipCurlPurchaseOrder>();
                    purchaseRecordsRP.Add(record);
                }
                catch (Exception ex)
                {
                    WriteToStatus($"Skipping row {csv.Context.Parser.Row}: {ex.Message}");
                }
            }
        }

        List<RipCurlPurchaseOrderExport> purchaseOrderExport = new List<RipCurlPurchaseOrderExport>();

        foreach (var purchaseOrder in purchaseRecordsRP)
        {
            var export = new RipCurlPurchaseOrderExport(purchaseOrder);
            export.FilePath = path;
            purchaseOrderExport.Add(export);
        }

        _pendingExports.AddRange(purchaseOrderExport);

        return purchaseOrderExport.Count > 0;
    }

    private bool CrocsImport(CsvConfiguration config, string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<CrocsPurchaseOrderCSVMap>();

            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<CrocsPurchaseOrder>();
                    purchaseRecordsCrocs.Add(record);

                }
                catch (Exception ex)
                {
                    WriteToStatus($"Skipping row {csv.Context.Parser.Row}: {ex.Message}");
                }
            }
        }

        List<CrocsPurchaseOrderExport> purchaseOrderExport = new List<CrocsPurchaseOrderExport>();

        foreach (var purchaseOrder in purchaseRecordsCrocs)
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
            var export = new CrocsPurchaseOrderExport(purchaseOrder);
            export.FilePath = path;
            purchaseOrderExport.Add(export);
        }


        _pendingExports.AddRange(purchaseOrderExport);

        return purchaseOrderExport?.Count > 0;
    }


    private void MergeDuplicated()
    {
        var duplicates = _pendingExports.GroupBy(x => new { x.UPC })
                 .Where(x => x.Skip(1).Any());

        int i = duplicates.Count();

        _mergesCount = i;

        if (duplicates.Any())
        {
            var msg = MessageBox.Show("Duplicates Found", "There were duplicates found in this document would you like to merge all?",MessageBoxButtons.YesNoCancel);
            if(msg == DialogResult.Yes)
            {
                MergeAll(duplicates);
            }
            else if (msg == DialogResult.No)
            {
                foreach (var dupli in duplicates)
                {
                    i -= 1;
                    var dupl = new DuplicateForm(dupli, _pendingExports, i);
                    dupl.ShowDialog();
                }
            }
            else
            {
                return;
            }
        }

        _exports = _pendingExports.ToList();
    }

    private void MergeAll(IEnumerable<IGrouping<object, IExportObject>> duplicates)
    {

        if (duplicates.Any())
        {
            foreach (var dupli in duplicates)
            {
                DataHandler.Merge(dupli, _pendingExports);
            }
        }

        _exports = _pendingExports.ToList();
    }

    private void WriteToStatus(string info)
    {
        Log.Information(info);
        statusTxtB.Text = info;
    }

    private void exportBtn_Click(object sender, EventArgs e)
    {
        try
        {
            WriteToStatus("Export Started....");
            exportBtn.Enabled = false;
            ExportForm exportForm = new ExportForm(_mergesCount, purchaseRecordsCrocs.Count, purchaseRecordsRP.Count, crocsRadBtn.Checked, _exports);
            exportForm.ShowDialog();
            WriteToStatus("Export Completed Successfully....");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error(ex, "Error Occured:");
            WriteToStatus("Done with errors...."); ;
        }
        finally
        {
            Clear();
            importBtn.Enabled = true;
        }
    }

    private void Clear()
    {
        _exports?.Clear();
        purchaseRecordsCrocs?.Clear();
        purchaseRecordsRP.Clear();
        dataGridView1?.Rows?.Clear();
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

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void findMenuItem_Click(object sender, EventArgs e)
    {
        FindForm form = new FindForm();
        form.ShowDialog();
    }
}
