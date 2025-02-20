namespace InventoryPro;

partial class DescriptionEditor
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
        label1 = new Label();
        groupBox1 = new GroupBox();
        currentDescriptionCountLbl = new Label();
        originalDescTxtB = new TextBox();
        groupBox2 = new GroupBox();
        newDescriptionCountLbl = new Label();
        newDescTxtB = new TextBox();
        label4 = new Label();
        button1 = new Button();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(626, 38);
        label1.Name = "label1";
        label1.Size = new Size(118, 20);
        label1.TabIndex = 0;
        label1.Text = "Character Count:";
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(currentDescriptionCountLbl);
        groupBox1.Controls.Add(originalDescTxtB);
        groupBox1.Controls.Add(label1);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(776, 78);
        groupBox1.TabIndex = 1;
        groupBox1.TabStop = false;
        groupBox1.Text = "Current Description";
        // 
        // currentDescriptionCountLbl
        // 
        currentDescriptionCountLbl.AutoSize = true;
        currentDescriptionCountLbl.Location = new Point(743, 38);
        currentDescriptionCountLbl.Name = "currentDescriptionCountLbl";
        currentDescriptionCountLbl.Size = new Size(17, 20);
        currentDescriptionCountLbl.TabIndex = 1;
        currentDescriptionCountLbl.Text = "0";
        // 
        // originalDescTxtB
        // 
        originalDescTxtB.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        originalDescTxtB.BorderStyle = BorderStyle.FixedSingle;
        originalDescTxtB.Enabled = false;
        originalDescTxtB.Location = new Point(10, 32);
        originalDescTxtB.Name = "originalDescTxtB";
        originalDescTxtB.ReadOnly = true;
        originalDescTxtB.Size = new Size(610, 27);
        originalDescTxtB.TabIndex = 0;
        originalDescTxtB.TabStop = false;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(newDescriptionCountLbl);
        groupBox2.Controls.Add(newDescTxtB);
        groupBox2.Controls.Add(label4);
        groupBox2.Location = new Point(12, 96);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(776, 78);
        groupBox2.TabIndex = 2;
        groupBox2.TabStop = false;
        groupBox2.Text = "New Description";
        // 
        // newDescriptionCountLbl
        // 
        newDescriptionCountLbl.AutoSize = true;
        newDescriptionCountLbl.Location = new Point(743, 38);
        newDescriptionCountLbl.Name = "newDescriptionCountLbl";
        newDescriptionCountLbl.Size = new Size(17, 20);
        newDescriptionCountLbl.TabIndex = 1;
        newDescriptionCountLbl.Text = "0";
        // 
        // newDescTxtB
        // 
        newDescTxtB.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        newDescTxtB.BorderStyle = BorderStyle.FixedSingle;
        newDescTxtB.Location = new Point(10, 32);
        newDescTxtB.MaxLength = 30;
        newDescTxtB.Name = "newDescTxtB";
        newDescTxtB.Size = new Size(610, 27);
        newDescTxtB.TabIndex = 0;
        newDescTxtB.TabStop = false;
        newDescTxtB.TextChanged += newDescTxtB_TextChanged;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(626, 38);
        label4.Name = "label4";
        label4.Size = new Size(118, 20);
        label4.TabIndex = 0;
        label4.Text = "Character Count:";
        // 
        // button1
        // 
        button1.Location = new Point(360, 195);
        button1.Name = "button1";
        button1.Size = new Size(94, 29);
        button1.TabIndex = 3;
        button1.Text = "Replace";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // DescriptionEditor
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 236);
        Controls.Add(button1);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Name = "DescriptionEditor";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Description Editor";
        Load += DescriptionEditor_Load;
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Label label1;
    private GroupBox groupBox1;
    private Label currentDescriptionCountLbl;
    private TextBox originalDescTxtB;
    private GroupBox groupBox2;
    private Label newDescriptionCountLbl;
    private TextBox newDescTxtB;
    private Label label4;
    private Button button1;
}