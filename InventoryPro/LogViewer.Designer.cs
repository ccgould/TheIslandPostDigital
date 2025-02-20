namespace InventoryPro;

partial class LogViewer
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
        logDatesCmb = new ComboBox();
        groupBox2 = new GroupBox();
        logTxtB = new TextBox();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(logDatesCmb);
        groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(250, 69);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Log Dates:";
        // 
        // logDatesCmb
        // 
        logDatesCmb.FormattingEnabled = true;
        logDatesCmb.Location = new Point(6, 26);
        logDatesCmb.Name = "logDatesCmb";
        logDatesCmb.Size = new Size(224, 28);
        logDatesCmb.TabIndex = 0;
        logDatesCmb.SelectedIndexChanged += logDatesCmb_SelectedIndexChanged;
        // 
        // groupBox2
        // 
        groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox2.Controls.Add(logTxtB);
        groupBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        groupBox2.Location = new Point(14, 97);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(705, 359);
        groupBox2.TabIndex = 1;
        groupBox2.TabStop = false;
        groupBox2.Text = "Log";
        // 
        // logTxtB
        // 
        logTxtB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        logTxtB.Location = new Point(6, 26);
        logTxtB.Multiline = true;
        logTxtB.Name = "logTxtB";
        logTxtB.ReadOnly = true;
        logTxtB.ScrollBars = ScrollBars.Both;
        logTxtB.Size = new Size(693, 327);
        logTxtB.TabIndex = 0;
        // 
        // LogViewer
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(731, 468);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Name = "LogViewer";
        Text = "Log Viewer";
        Load += LogViewer_Load;
        groupBox1.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox groupBox1;
    private ComboBox logDatesCmb;
    private GroupBox groupBox2;
    private TextBox logTxtB;
}