using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace InventoryPro;
public partial class LogViewer : Form
{
    private Dictionary<string, string> _logFiles = new();
    public LogViewer()
    {
        InitializeComponent();
    }

    private void logDatesCmb_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            logTxtB.Text = string.Empty;
            var path = _logFiles[logDatesCmb.Text];
            string contents = File.ReadAllText(path);
            logTxtB.Text = contents;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void LogViewer_Load(object sender, EventArgs e)
    {
        CultureInfo provider = CultureInfo.InvariantCulture;

        var path = "Data\\Logs";
        if (Directory.Exists(path))
        {
            var ext = new List<string> { "txt" };
            var myFiles = Directory
                .EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));



            foreach (var file in myFiles) 
            { 
                var dateCombined = Path.GetFileNameWithoutExtension(file).Remove(0, 5);
                var format = "yyyyMMdd";
                var date = DateTime.ParseExact(dateCombined, format,provider);
                _logFiles.Add(date.ToString("D"),file);
                logDatesCmb.Items.Add(date.ToString("D"));
            }
        }
    }
}
