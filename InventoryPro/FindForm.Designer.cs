namespace InventoryPro;

partial class FindForm
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
        fileNameTxtB = new TextBox();
        label3 = new Label();
        dateTimePicker1 = new DateTimePicker();
        label2 = new Label();
        upcTxtB = new TextBox();
        label1 = new Label();
        groupBox2 = new GroupBox();
        dataGridView1 = new DataGridView();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBox1.Controls.Add(fileNameTxtB);
        groupBox1.Controls.Add(label3);
        groupBox1.Controls.Add(dateTimePicker1);
        groupBox1.Controls.Add(label2);
        groupBox1.Controls.Add(upcTxtB);
        groupBox1.Controls.Add(label1);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(799, 138);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Filter";
        // 
        // fileNameTxtB
        // 
        fileNameTxtB.Location = new Point(96, 64);
        fileNameTxtB.Name = "fileNameTxtB";
        fileNameTxtB.Size = new Size(674, 27);
        fileNameTxtB.TabIndex = 5;
        fileNameTxtB.TextChanged += fileNameTxtB_TextChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(11, 67);
        label3.Name = "label3";
        label3.Size = new Size(79, 20);
        label3.TabIndex = 4;
        label3.Text = "File Name:";
        // 
        // dateTimePicker1
        // 
        dateTimePicker1.Location = new Point(520, 30);
        dateTimePicker1.Name = "dateTimePicker1";
        dateTimePicker1.Size = new Size(250, 27);
        dateTimePicker1.TabIndex = 3;
        dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(464, 33);
        label2.Name = "label2";
        label2.Size = new Size(44, 20);
        label2.TabIndex = 2;
        label2.Text = "Date:";
        // 
        // upcTxtB
        // 
        upcTxtB.Location = new Point(53, 30);
        upcTxtB.Name = "upcTxtB";
        upcTxtB.Size = new Size(398, 27);
        upcTxtB.TabIndex = 1;
        upcTxtB.TextChanged += upcTxtB_TextChanged;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(11, 34);
        label1.Name = "label1";
        label1.Size = new Size(36, 20);
        label1.TabIndex = 0;
        label1.Text = "UPC";
        // 
        // groupBox2
        // 
        groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox2.Controls.Add(dataGridView1);
        groupBox2.Location = new Point(12, 178);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(799, 462);
        groupBox2.TabIndex = 1;
        groupBox2.TabStop = false;
        groupBox2.Text = "PO";
        // 
        // dataGridView1
        // 
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.AllowUserToResizeColumns = false;
        dataGridView1.AllowUserToResizeRows = false;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(3, 23);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.RowHeadersWidth = 51;
        dataGridView1.Size = new Size(793, 436);
        dataGridView1.TabIndex = 0;
        // 
        // FindForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(823, 652);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        MinimumSize = new Size(841, 508);
        Name = "FindForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FindForm";
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox groupBox1;
    private DateTimePicker dateTimePicker1;
    private Label label2;
    private TextBox upcTxtB;
    private Label label1;
    private GroupBox groupBox2;
    private DataGridView dataGridView1;
    private TextBox fileNameTxtB;
    private Label label3;
}