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
public partial class DescriptionEditor : Form
{
    public string Result { get; set; }
    public DescriptionEditor()
    {
        InitializeComponent();
    }

    public DescriptionEditor(string originalDescription)
    {
        InitializeComponent();

        originalDescTxtB.Text = originalDescription;
        newDescTxtB.Text = originalDescription;
        currentDescriptionCountLbl.Text = originalDescription.Length.ToString();
    }

    private void DescriptionEditor_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(newDescTxtB.Text))
        {
            MessageBox.Show("Attention", "You must have a description for an item (Character Limit: 30)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (newDescTxtB.Text.Length > 30)
        {
            MessageBox.Show("Attention", "Your description doesnt meet the requirement. (Character Limit: 30)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }


        SqliteDataAccess.AddCorrectedDescription(originalDescTxtB.Text, newDescTxtB.Text);
        Result = newDescTxtB.Text;
        Close();
    }

    private void newDescTxtB_TextChanged(object sender, EventArgs e)
    {
        newDescriptionCountLbl.Text = newDescTxtB.Text.Length.ToString();
    }
}
