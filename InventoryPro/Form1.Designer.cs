namespace InventoryPro;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        groupBox1 = new GroupBox();
        button1 = new Button();
        dataGridView1 = new DataGridView();
        Path = new DataGridViewTextBoxColumn();
        browseBtn = new Button();
        groupBox2 = new GroupBox();
        groupBox4 = new GroupBox();
        ripcurlRadBtn = new RadioButton();
        crocsRadBtn = new RadioButton();
        hasHeaderChkB = new CheckBox();
        groupBox3 = new GroupBox();
        statusTxtB = new TextBox();
        importBtn = new Button();
        exportBtn = new Button();
        logBtn = new Button();
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        logToolStripMenuItem = new ToolStripMenuItem();
        findMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        exportToolStripMenuItem = new ToolStripMenuItem();
        importToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripSeparator();
        settingsToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem2 = new ToolStripSeparator();
        closeToolStripMenuItem = new ToolStripMenuItem();
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        groupBox2.SuspendLayout();
        groupBox4.SuspendLayout();
        groupBox3.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox1.Controls.Add(button1);
        groupBox1.Controls.Add(dataGridView1);
        groupBox1.Controls.Add(browseBtn);
        groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox1.Location = new Point(12, 27);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(784, 391);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Import File:";
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        button1.FlatStyle = FlatStyle.Popup;
        button1.Location = new Point(684, 26);
        button1.Name = "button1";
        button1.Size = new Size(94, 29);
        button1.TabIndex = 3;
        button1.Text = "Remove";
        button1.UseVisualStyleBackColor = true;
        button1.Click += deleteSelectedButton_Click;
        // 
        // dataGridView1
        // 
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToResizeColumns = false;
        dataGridView1.AllowUserToResizeRows = false;
        dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Path });
        dataGridView1.Location = new Point(6, 61);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.RowHeadersWidth = 51;
        dataGridView1.Size = new Size(772, 324);
        dataGridView1.TabIndex = 2;
        // 
        // Path
        // 
        Path.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        Path.HeaderText = "Path";
        Path.MinimumWidth = 6;
        Path.Name = "Path";
        Path.ReadOnly = true;
        Path.Resizable = DataGridViewTriState.False;
        // 
        // browseBtn
        // 
        browseBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        browseBtn.FlatStyle = FlatStyle.Popup;
        browseBtn.Location = new Point(584, 26);
        browseBtn.Name = "browseBtn";
        browseBtn.Size = new Size(94, 29);
        browseBtn.TabIndex = 1;
        browseBtn.Text = "Add";
        browseBtn.UseVisualStyleBackColor = true;
        browseBtn.Click += browseBtn_Click;
        // 
        // groupBox2
        // 
        groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        groupBox2.Controls.Add(groupBox4);
        groupBox2.Controls.Add(hasHeaderChkB);
        groupBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox2.Location = new Point(802, 23);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(250, 163);
        groupBox2.TabIndex = 1;
        groupBox2.TabStop = false;
        groupBox2.Text = "Import Settings:";
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(ripcurlRadBtn);
        groupBox4.Controls.Add(crocsRadBtn);
        groupBox4.Location = new Point(6, 57);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new Size(238, 97);
        groupBox4.TabIndex = 7;
        groupBox4.TabStop = false;
        groupBox4.Text = "Store";
        // 
        // ripcurlRadBtn
        // 
        ripcurlRadBtn.AutoSize = true;
        ripcurlRadBtn.Location = new Point(15, 57);
        ripcurlRadBtn.Name = "ripcurlRadBtn";
        ripcurlRadBtn.Size = new Size(81, 24);
        ripcurlRadBtn.TabIndex = 1;
        ripcurlRadBtn.Text = "RipCurl";
        ripcurlRadBtn.UseVisualStyleBackColor = true;
        // 
        // crocsRadBtn
        // 
        crocsRadBtn.AutoSize = true;
        crocsRadBtn.Checked = true;
        crocsRadBtn.Location = new Point(15, 27);
        crocsRadBtn.Name = "crocsRadBtn";
        crocsRadBtn.Size = new Size(68, 24);
        crocsRadBtn.TabIndex = 0;
        crocsRadBtn.TabStop = true;
        crocsRadBtn.Text = "Crocs";
        crocsRadBtn.UseVisualStyleBackColor = true;
        // 
        // hasHeaderChkB
        // 
        hasHeaderChkB.AutoSize = true;
        hasHeaderChkB.Checked = true;
        hasHeaderChkB.CheckState = CheckState.Checked;
        hasHeaderChkB.FlatStyle = FlatStyle.System;
        hasHeaderChkB.Location = new Point(6, 26);
        hasHeaderChkB.Name = "hasHeaderChkB";
        hasHeaderChkB.Size = new Size(148, 25);
        hasHeaderChkB.TabIndex = 1;
        hasHeaderChkB.Text = "File Has Header";
        hasHeaderChkB.UseVisualStyleBackColor = true;
        // 
        // groupBox3
        // 
        groupBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox3.Controls.Add(statusTxtB);
        groupBox3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox3.Location = new Point(7, 446);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(1045, 82);
        groupBox3.TabIndex = 2;
        groupBox3.TabStop = false;
        groupBox3.Text = "Status:";
        // 
        // statusTxtB
        // 
        statusTxtB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        statusTxtB.BorderStyle = BorderStyle.FixedSingle;
        statusTxtB.Enabled = false;
        statusTxtB.Location = new Point(6, 37);
        statusTxtB.Name = "statusTxtB";
        statusTxtB.ReadOnly = true;
        statusTxtB.Size = new Size(1033, 27);
        statusTxtB.TabIndex = 0;
        // 
        // importBtn
        // 
        importBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        importBtn.FlatStyle = FlatStyle.Flat;
        importBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        importBtn.Image = (Image)resources.GetObject("importBtn.Image");
        importBtn.ImageAlign = ContentAlignment.TopCenter;
        importBtn.Location = new Point(823, 192);
        importBtn.Name = "importBtn";
        importBtn.Size = new Size(104, 104);
        importBtn.TabIndex = 3;
        importBtn.Text = "Import";
        importBtn.TextAlign = ContentAlignment.BottomCenter;
        importBtn.UseVisualStyleBackColor = true;
        importBtn.Click += importBtn_Click;
        // 
        // exportBtn
        // 
        exportBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        exportBtn.Enabled = false;
        exportBtn.FlatStyle = FlatStyle.Flat;
        exportBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        exportBtn.Image = (Image)resources.GetObject("exportBtn.Image");
        exportBtn.ImageAlign = ContentAlignment.MiddleLeft;
        exportBtn.Location = new Point(823, 302);
        exportBtn.Name = "exportBtn";
        exportBtn.Size = new Size(214, 104);
        exportBtn.TabIndex = 4;
        exportBtn.Text = "Export";
        exportBtn.UseVisualStyleBackColor = true;
        exportBtn.Click += exportBtn_Click;
        // 
        // logBtn
        // 
        logBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        logBtn.FlatStyle = FlatStyle.Flat;
        logBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        logBtn.Image = (Image)resources.GetObject("logBtn.Image");
        logBtn.ImageAlign = ContentAlignment.TopCenter;
        logBtn.Location = new Point(933, 192);
        logBtn.Name = "logBtn";
        logBtn.Size = new Size(104, 104);
        logBtn.TabIndex = 5;
        logBtn.Text = "Log";
        logBtn.TextAlign = ContentAlignment.BottomCenter;
        logBtn.UseVisualStyleBackColor = true;
        logBtn.Click += logBtn_Click;
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new Size(20, 20);
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(1063, 28);
        menuStrip1.TabIndex = 6;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { logToolStripMenuItem, findMenuItem, toolStripSeparator1, exportToolStripMenuItem, importToolStripMenuItem, toolStripMenuItem1, settingsToolStripMenuItem, toolStripMenuItem2, closeToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(46, 24);
        fileToolStripMenuItem.Text = "File";
        // 
        // logToolStripMenuItem
        // 
        logToolStripMenuItem.Name = "logToolStripMenuItem";
        logToolStripMenuItem.Size = new Size(145, 26);
        logToolStripMenuItem.Text = "Log";
        logToolStripMenuItem.Click += logBtn_Click;
        // 
        // findMenuItem
        // 
        findMenuItem.Name = "findMenuItem";
        findMenuItem.Size = new Size(145, 26);
        findMenuItem.Text = "Find";
        findMenuItem.Click += findMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(142, 6);
        // 
        // exportToolStripMenuItem
        // 
        exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        exportToolStripMenuItem.Size = new Size(145, 26);
        exportToolStripMenuItem.Text = "Export";
        exportToolStripMenuItem.Click += exportBtn_Click;
        // 
        // importToolStripMenuItem
        // 
        importToolStripMenuItem.Name = "importToolStripMenuItem";
        importToolStripMenuItem.Size = new Size(145, 26);
        importToolStripMenuItem.Text = "Import";
        importToolStripMenuItem.Click += importBtn_Click;
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(142, 6);
        // 
        // settingsToolStripMenuItem
        // 
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
        settingsToolStripMenuItem.Size = new Size(145, 26);
        settingsToolStripMenuItem.Text = "Settings";
        settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        toolStripMenuItem2.Size = new Size(142, 6);
        // 
        // closeToolStripMenuItem
        // 
        closeToolStripMenuItem.Name = "closeToolStripMenuItem";
        closeToolStripMenuItem.Size = new Size(145, 26);
        closeToolStripMenuItem.Text = "Close";
        closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1063, 540);
        Controls.Add(logBtn);
        Controls.Add(exportBtn);
        Controls.Add(importBtn);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Controls.Add(menuStrip1);
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Inventory Pro";
        Load += Form1_Load;
        groupBox1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox3.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox groupBox1;
    private Button browseBtn;
    private TextBox importTxtB;
    private GroupBox groupBox2;
    private CheckBox validateChkB;
    private GroupBox groupBox3;
    private TextBox statusTxtB;
    private Button importBtn;
    private Button exportBtn;
    private Button logBtn;
    private CheckBox hasHeaderChkB;
    private GroupBox groupBox4;
    private RadioButton ripcurlRadBtn;
    private RadioButton crocsRadBtn;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn Path;
    private Button button1;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem logToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem exportToolStripMenuItem;
    private ToolStripMenuItem importToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem settingsToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem closeToolStripMenuItem;
    private ToolStripMenuItem findMenuItem;
}
