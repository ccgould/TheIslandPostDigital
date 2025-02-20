using InventoryPro.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryPro;
public partial class MissingDCS : Form
{
    public MissingDCS()
    {
        InitializeComponent();
    }

    private void MissingDCS_Load(object sender, EventArgs e)
    {
        divisionCmb.DataSource = SqliteDataAccess.GetDepartmentsInfo("Division");
    }
}
