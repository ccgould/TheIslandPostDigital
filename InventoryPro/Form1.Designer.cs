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
        browseBtn = new Button();
        importTxtB = new TextBox();
        groupBox2 = new GroupBox();
        hasHeaderChkB = new CheckBox();
        validateChkB = new CheckBox();
        groupBox3 = new GroupBox();
        statusTxtB = new TextBox();
        importBtn = new Button();
        exportBtn = new Button();
        logBtn = new Button();
        groupBox4 = new GroupBox();
        crocsRadBtn = new RadioButton();
        ripcurlRadBtn = new RadioButton();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox3.SuspendLayout();
        groupBox4.SuspendLayout();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(browseBtn);
        groupBox1.Controls.Add(importTxtB);
        groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(929, 77);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Import File:";
        // 
        // browseBtn
        // 
        browseBtn.Anchor = AnchorStyles.Right;
        browseBtn.FlatStyle = FlatStyle.Popup;
        browseBtn.Location = new Point(816, 29);
        browseBtn.Name = "browseBtn";
        browseBtn.Size = new Size(94, 29);
        browseBtn.TabIndex = 1;
        browseBtn.Text = "Browse ....";
        browseBtn.UseVisualStyleBackColor = true;
        browseBtn.Click += browseBtn_Click;
        // 
        // importTxtB
        // 
        importTxtB.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        importTxtB.BorderStyle = BorderStyle.FixedSingle;
        importTxtB.Enabled = false;
        importTxtB.Location = new Point(6, 30);
        importTxtB.Name = "importTxtB";
        importTxtB.ReadOnly = true;
        importTxtB.Size = new Size(804, 27);
        importTxtB.TabIndex = 0;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(hasHeaderChkB);
        groupBox2.Controls.Add(validateChkB);
        groupBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox2.Location = new Point(16, 106);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(250, 107);
        groupBox2.TabIndex = 1;
        groupBox2.TabStop = false;
        groupBox2.Text = "Import Settings:";
        // 
        // hasHeaderChkB
        // 
        hasHeaderChkB.AutoSize = true;
        hasHeaderChkB.FlatStyle = FlatStyle.System;
        hasHeaderChkB.Location = new Point(6, 66);
        hasHeaderChkB.Name = "hasHeaderChkB";
        hasHeaderChkB.Size = new Size(148, 25);
        hasHeaderChkB.TabIndex = 1;
        hasHeaderChkB.Text = "File Has Header";
        hasHeaderChkB.UseVisualStyleBackColor = true;
        // 
        // validateChkB
        // 
        validateChkB.AutoSize = true;
        validateChkB.FlatStyle = FlatStyle.System;
        validateChkB.Location = new Point(6, 35);
        validateChkB.Name = "validateChkB";
        validateChkB.Size = new Size(132, 25);
        validateChkB.TabIndex = 0;
        validateChkB.Text = "Validate Only";
        validateChkB.UseVisualStyleBackColor = true;
        validateChkB.CheckedChanged += checkBox1_CheckedChanged;
        // 
        // groupBox3
        // 
        groupBox3.Anchor = AnchorStyles.Bottom;
        groupBox3.Controls.Add(statusTxtB);
        groupBox3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox3.Location = new Point(406, 273);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(535, 82);
        groupBox3.TabIndex = 2;
        groupBox3.TabStop = false;
        groupBox3.Text = "Status:";
        // 
        // statusTxtB
        // 
        statusTxtB.BorderStyle = BorderStyle.FixedSingle;
        statusTxtB.Enabled = false;
        statusTxtB.Location = new Point(31, 37);
        statusTxtB.Name = "statusTxtB";
        statusTxtB.ReadOnly = true;
        statusTxtB.Size = new Size(483, 27);
        statusTxtB.TabIndex = 0;
        // 
        // importBtn
        // 
        importBtn.Anchor = AnchorStyles.Bottom;
        importBtn.FlatStyle = FlatStyle.Flat;
        importBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        importBtn.Image = (Image)resources.GetObject("importBtn.Image");
        importBtn.ImageAlign = ContentAlignment.TopCenter;
        importBtn.Location = new Point(18, 262);
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
        exportBtn.Anchor = AnchorStyles.Bottom;
        exportBtn.FlatStyle = FlatStyle.Flat;
        exportBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        exportBtn.Image = (Image)resources.GetObject("exportBtn.Image");
        exportBtn.ImageAlign = ContentAlignment.TopCenter;
        exportBtn.Location = new Point(141, 262);
        exportBtn.Name = "exportBtn";
        exportBtn.Size = new Size(104, 104);
        exportBtn.TabIndex = 4;
        exportBtn.Text = "Export";
        exportBtn.TextAlign = ContentAlignment.BottomCenter;
        exportBtn.UseVisualStyleBackColor = true;
        exportBtn.Click += exportBtn_Click;
        // 
        // logBtn
        // 
        logBtn.Anchor = AnchorStyles.Bottom;
        logBtn.FlatStyle = FlatStyle.Flat;
        logBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        logBtn.Image = (Image)resources.GetObject("logBtn.Image");
        logBtn.ImageAlign = ContentAlignment.TopCenter;
        logBtn.Location = new Point(263, 262);
        logBtn.Name = "logBtn";
        logBtn.Size = new Size(104, 104);
        logBtn.TabIndex = 5;
        logBtn.Text = "Log";
        logBtn.TextAlign = ContentAlignment.BottomCenter;
        logBtn.UseVisualStyleBackColor = true;
        logBtn.Click += logBtn_Click;
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(ripcurlRadBtn);
        groupBox4.Controls.Add(crocsRadBtn);
        groupBox4.Location = new Point(288, 116);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new Size(250, 97);
        groupBox4.TabIndex = 7;
        groupBox4.TabStop = false;
        groupBox4.Text = "Store";
        // 
        // crocsRadBtn
        // 
        crocsRadBtn.AutoSize = true;
        crocsRadBtn.Checked = true;
        crocsRadBtn.Location = new Point(15, 27);
        crocsRadBtn.Name = "crocsRadBtn";
        crocsRadBtn.Size = new Size(66, 24);
        crocsRadBtn.TabIndex = 0;
        crocsRadBtn.TabStop = true;
        crocsRadBtn.Text = "Crocs";
        crocsRadBtn.UseVisualStyleBackColor = true;
        // 
        // ripcurlRadBtn
        // 
        ripcurlRadBtn.AutoSize = true;
        ripcurlRadBtn.Location = new Point(15, 57);
        ripcurlRadBtn.Name = "ripcurlRadBtn";
        ripcurlRadBtn.Size = new Size(78, 24);
        ripcurlRadBtn.TabIndex = 1;
        ripcurlRadBtn.Text = "RipCurl";
        ripcurlRadBtn.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(980, 378);
        Controls.Add(groupBox4);
        Controls.Add(logBtn);
        Controls.Add(exportBtn);
        Controls.Add(importBtn);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        MaximizeBox = false;
        Name = "Form1";
        Text = "Inventory Pro";
        Load += Form1_Load;
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox3.PerformLayout();
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        ResumeLayout(false);
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
}
