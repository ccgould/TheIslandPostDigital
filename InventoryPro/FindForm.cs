using InventoryPro.Services;

namespace InventoryPro;
public partial class FindForm : Form
{
    public FindForm()
    {
        InitializeComponent();
        RefreshDataGrid();
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    private void upcTxtB_TextChanged(object sender, EventArgs e)
    {
        RefreshDataGrid();
    }

    private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
    {
        RefreshDataGrid();
    }

    private void RefreshDataGrid()
    {
        dataGridView1.DataSource = SqliteDataAccess.GetFilteredHistory(dateTimePicker1.Value, upcTxtB.Text, fileNameTxtB.Text);
    }

    private void fileNameTxtB_TextChanged(object sender, EventArgs e)
    {
        RefreshDataGrid();
    }
}
