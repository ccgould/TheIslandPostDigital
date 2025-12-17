namespace InventoryPro;

partial class ExportForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        groupBox1 = new GroupBox();
        dataGridView1 = new DataGridView();
        groupBox2 = new GroupBox();
        mergeLbl = new Label();
        exportLbl = new Label();
        importCntLbl = new Label();
        exportBtn = new Button();
        groupBox3 = new GroupBox();
        numericUpDown1 = new NumericUpDown();
        label1 = new Label();
        poNameFormatTxtb = new TextBox();
        label6 = new Label();
        exportBrowseBtn = new Button();
        exportLocationTxtb = new TextBox();
        label5 = new Label();
        label4 = new Label();
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        groupBox2.SuspendLayout();
        groupBox3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox1.Controls.Add(dataGridView1);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(620, 455);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Purchase Order";
        // 
        // dataGridView1
        // 
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(3, 23);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.RowHeadersWidth = 51;
        dataGridView1.Size = new Size(614, 429);
        dataGridView1.TabIndex = 0;
        // 
        // groupBox2
        // 
        groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        groupBox2.Controls.Add(mergeLbl);
        groupBox2.Controls.Add(exportLbl);
        groupBox2.Controls.Add(importCntLbl);
        groupBox2.Location = new Point(638, 12);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(472, 150);
        groupBox2.TabIndex = 1;
        groupBox2.TabStop = false;
        groupBox2.Text = "Information";
        // 
        // mergeLbl
        // 
        mergeLbl.AutoSize = true;
        mergeLbl.Location = new Point(8, 101);
        mergeLbl.Name = "mergeLbl";
        mergeLbl.Size = new Size(110, 20);
        mergeLbl.TabIndex = 2;
        mergeLbl.Text = "Merge Count: 0";
        // 
        // exportLbl
        // 
        exportLbl.AutoSize = true;
        exportLbl.Location = new Point(6, 67);
        exportLbl.Name = "exportLbl";
        exportLbl.Size = new Size(110, 20);
        exportLbl.TabIndex = 1;
        exportLbl.Text = "Export Count: 0";
        // 
        // importCntLbl
        // 
        importCntLbl.AutoSize = true;
        importCntLbl.Location = new Point(6, 35);
        importCntLbl.Name = "importCntLbl";
        importCntLbl.Size = new Size(112, 20);
        importCntLbl.TabIndex = 0;
        importCntLbl.Text = "Import Count: 0";
        // 
        // exportBtn
        // 
        exportBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        exportBtn.Location = new Point(372, 135);
        exportBtn.Name = "exportBtn";
        exportBtn.Size = new Size(94, 29);
        exportBtn.TabIndex = 2;
        exportBtn.Text = "Export";
        exportBtn.UseVisualStyleBackColor = true;
        exportBtn.Click += exportBtn_Click;
        // 
        // groupBox3
        // 
        groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        groupBox3.Controls.Add(numericUpDown1);
        groupBox3.Controls.Add(label1);
        groupBox3.Controls.Add(poNameFormatTxtb);
        groupBox3.Controls.Add(label6);
        groupBox3.Controls.Add(exportBrowseBtn);
        groupBox3.Controls.Add(exportLocationTxtb);
        groupBox3.Controls.Add(exportBtn);
        groupBox3.Controls.Add(label5);
        groupBox3.Controls.Add(label4);
        groupBox3.Location = new Point(638, 168);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(472, 170);
        groupBox3.TabIndex = 3;
        groupBox3.TabStop = false;
        groupBox3.Text = "Configuration";
        // 
        // numericUpDown1
        // 
        numericUpDown1.Location = new Point(134, 111);
        numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericUpDown1.Name = "numericUpDown1";
        numericUpDown1.Size = new Size(150, 27);
        numericUpDown1.TabIndex = 7;
        numericUpDown1.Value = new decimal(new int[] { 20, 0, 0, 0 });
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(8, 113);
        label1.Name = "label1";
        label1.Size = new Size(110, 20);
        label1.TabIndex = 6;
        label1.Text = "Group Amount:";
        // 
        // poNameFormatTxtb
        // 
        poNameFormatTxtb.Location = new Point(134, 71);
        poNameFormatTxtb.Name = "poNameFormatTxtb";
        poNameFormatTxtb.ReadOnly = true;
        poNameFormatTxtb.Size = new Size(332, 27);
        poNameFormatTxtb.TabIndex = 5;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(7, 75);
        label6.Name = "label6";
        label6.Size = new Size(123, 20);
        label6.TabIndex = 4;
        label6.Text = "PO Name Format";
        // 
        // exportBrowseBtn
        // 
        exportBrowseBtn.Location = new Point(372, 33);
        exportBrowseBtn.Name = "exportBrowseBtn";
        exportBrowseBtn.Size = new Size(94, 29);
        exportBrowseBtn.TabIndex = 3;
        exportBrowseBtn.Text = "...";
        exportBrowseBtn.UseVisualStyleBackColor = true;
        exportBrowseBtn.Click += exportBrowseBtn_Click;
        // 
        // exportLocationTxtb
        // 
        exportLocationTxtb.Location = new Point(130, 33);
        exportLocationTxtb.Name = "exportLocationTxtb";
        exportLocationTxtb.ReadOnly = true;
        exportLocationTxtb.Size = new Size(236, 27);
        exportLocationTxtb.TabIndex = 2;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(8, 37);
        label5.Name = "label5";
        label5.Size = new Size(116, 20);
        label5.TabIndex = 1;
        label5.Text = "Export Location:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(8, 23);
        label4.Name = "label4";
        label4.Size = new Size(13, 20);
        label4.TabIndex = 0;
        label4.Text = " ";
        // 
        // ExportForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1122, 479);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Name = "ExportForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "ExportForm";
        groupBox1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox3.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox groupBox1;
    private DataGridView dataGridView1;
    private GroupBox groupBox2;
    private Button exportBtn;
    private Label importCntLbl;
    private GroupBox groupBox3;
    private Label mergeLbl;
    private Label exportLbl;
    private Label label4;
    private Button exportBrowseBtn;
    private TextBox exportLocationTxtb;
    private Label label5;
    private TextBox poNameFormatTxtb;
    private Label label6;
    private NumericUpDown numericUpDown1;
    private Label label1;
}