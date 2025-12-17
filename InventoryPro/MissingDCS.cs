using InventoryPro.Services;

namespace InventoryPro;
public partial class MissingDCS : Form
{
    public MissingDCS()
    {
        InitializeComponent();
    }

    private void MissingDCS_Load(object sender, EventArgs e)
    {
        divisionCmb.DataSource = SqliteDataAccess.LoadAllDepartments();
    }
}
