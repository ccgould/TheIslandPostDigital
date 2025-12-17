using CsvHelper;
using InventoryPro.Interface;
using InventoryPro.Models;
using InventoryPro.Models.Crocs;
using InventoryPro.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.Globalization;

namespace InventoryPro;
public partial class ExportForm : Form
{
    private readonly int mergesCount;
    private readonly bool isCrocs;
    private BindingList<IExportObject> exports;
    private List<RipCurlPurchaseOrderExport> _ripCurlExport;
    private List<CrocsPurchaseOrderExport> _crocsCast;
    private List<RipCurlPurchaseOrderExport> _rpCast;

    public ExportForm()
    {
        InitializeComponent();
        Init();
    }

    public ExportForm(int mergesCount, int rpImportCount, int crocsImportCount, bool isCrocs, List<IExportObject> list)
    {
        InitializeComponent();
        Init();
        this.mergesCount = mergesCount;
        this.isCrocs = isCrocs;
        importCntLbl.Text = $"Import Count: {(isCrocs ? crocsImportCount : rpImportCount)}";
        exportLbl.Text = $"Export Count: {list?.Count}";
        poNameFormatTxtb.Enabled = false;

        if (isCrocs)
        {
            _crocsCast = list.Cast<CrocsPurchaseOrderExport>().ToList();

            // Bind to DataGridView
            dataGridView1.DataSource = _crocsCast;

            // Optional: auto-size columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        else
        {
            _rpCast = list.Cast<RipCurlPurchaseOrderExport>().ToList();
            dataGridView1.DataSource = _rpCast;

            // Optional: auto-size columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }

    private void Init()
    {
        var today = DateTime.Today;
        poNameFormatTxtb.Text = $"BAH_{today:dd_MM_yyyy}_PO_###";
    }

    private void Export()
    {
        if (isCrocs)
        {
            CreateCSVFile(_crocsCast, exportLocationTxtb.Text);
        }
        else
        {
            CreateCSVFile(_rpCast, exportLocationTxtb.Text);
        }
        this.Close();
    }

    private void exportBtn_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(exportLocationTxtb.Text))
        {
            MessageBox.Show("Please select an export location.");
            return;
        }
        Export();
    }

    private void exportBrowseBtn_Click(object sender, EventArgs e)
    {
        var saveDialog = new CommonSaveFileDialog();
        saveDialog.Filters.Add(new CommonFileDialogFilter("Comma Seperated Value", ".csv"));
        saveDialog.DefaultExtension = "csv";
        saveDialog.Title = "Save Exported Purchase Order";
        saveDialog.DefaultFileName = isCrocs ? "Crocs_Purchase_Order.csv" : "RipCurl_Purchase_Order.csv";

        saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        if (saveDialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            exportLocationTxtb.Text = saveDialog.FileName;
        }
    }

    private void CreateCSVFile<T>(List<T> exportList,string path) where T : IExportObject, new()
    {
        var history = new List<POHistory>();
        int groupSize = Convert.ToInt32(numericUpDown1.Value);
        var today = DateTime.Today;
        string prefix = $"BAH-{today:dd-MM-yy}_PO_";

        for (int i = 0; i < exportList.Count; i++)
        {
            int groupNumber = (i / groupSize) + 1;
            exportList[i].PONumber = $"{prefix}{groupNumber:D3}";
            history.Add(exportList[i].GetPOHistory(isCrocs));
        }

        SqliteDataAccess.InsertPOHistory(history);

        using (var writer = new StreamWriter(path))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(exportList);
        }
    }
}
