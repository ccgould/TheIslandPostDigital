using System.Data;

namespace InventoryPro;
public partial class MaxLengthCorrectionPage : Form
{
    private DataTable stringTable;
    private const int MaxLength = 20; // Set your desired max length

    public MaxLengthCorrectionPage()
    {
        InitializeComponent();
        InitializeGrid();
        InitializeButtons();
    }

    private void InitializeGrid()
    {
        stringTable = new DataTable();
        stringTable.Columns.Add("Original", typeof(string));
        stringTable.Columns.Add("Corrected", typeof(string));

        stringTable.Rows.Add("This string is way too long", "");
        stringTable.Rows.Add("Another overly lengthy string", "");

        dataGridView1.DataSource = stringTable;
        dataGridView1.Columns["Original"].ReadOnly = true;

        dataGridView1.CellValidating += DataGridView1_CellValidating;
        dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
        dataGridView1.RowPrePaint += DataGridView1_RowPrePaint;
    }

    private void DataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (dataGridView1.Columns[e.ColumnIndex].Name == "Corrected")
        {
            string newValue = e.FormattedValue.ToString();
            if (newValue.Length > MaxLength)
            {
                MessageBox.Show($"Corrected string must be ≤ {MaxLength} characters.");
                e.Cancel = true;
            }
        }
    }

    private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == stringTable.Columns["Corrected"].Ordinal)
        {
            string original = stringTable.Rows[e.RowIndex]["Original"].ToString();
            string corrected = stringTable.Rows[e.RowIndex]["Corrected"].ToString();
            Console.WriteLine($"Updated row {e.RowIndex}: {original} → {corrected}");
            dataGridView1.InvalidateRow(e.RowIndex); // Refresh row highlight
        }
    }

    private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
    {
        var row = dataGridView1.Rows[e.RowIndex];
        string corrected = row.Cells["Corrected"].Value?.ToString() ?? "";

        if (string.IsNullOrWhiteSpace(corrected) || corrected.Length > MaxLength)
        {
            row.DefaultCellStyle.BackColor = Color.MistyRose;
        }
        else
        {
            row.DefaultCellStyle.BackColor = Color.White;
        }
    }

    private void InitializeButtons()
    {
        var saveButton = new Button
        {
            Text = "Save Corrections",
            Dock = DockStyle.Bottom
        };
        saveButton.Click += SaveButton_Click;
        Controls.Add(saveButton);

        var suggestButton = new Button
        {
            Text = "Auto-Suggest Corrections",
            Dock = DockStyle.Bottom
        };
        suggestButton.Click += SuggestButton_Click;
        Controls.Add(suggestButton);
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        foreach (DataRow row in stringTable.Rows)
        {
            string original = row["Original"].ToString();
            string corrected = row["Corrected"].ToString();
            if (!string.IsNullOrWhiteSpace(corrected))
            {
                Console.WriteLine($"Saving: {original} → {corrected}");
                // TODO: Replace with actual save logic
            }
        }

        MessageBox.Show("Corrections saved!");
    }

    private void SuggestButton_Click(object sender, EventArgs e)
    {
        foreach (DataRow row in stringTable.Rows)
        {
            string original = row["Original"].ToString();
            if (string.IsNullOrWhiteSpace(row["Corrected"].ToString()))
            {
                row["Corrected"] = original.Length > MaxLength
                    ? original.Substring(0, MaxLength - 3) + "..."
                    : original;
            }
        }

        dataGridView1.Refresh();
        MessageBox.Show("Suggestions applied!");
    }


}
