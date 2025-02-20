namespace InventoryPro;

partial class MissingDCS
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
        divisionCmb = new ComboBox();
        categoryCmb = new ComboBox();
        textBox1 = new TextBox();
        groupBox1 = new GroupBox();
        groupBox2 = new GroupBox();
        dcsCmb = new GroupBox();
        saveBtn = new Button();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        dcsCmb.SuspendLayout();
        SuspendLayout();
        // 
        // divisionCmb
        // 
        divisionCmb.FormattingEnabled = true;
        divisionCmb.Location = new Point(6, 31);
        divisionCmb.Name = "divisionCmb";
        divisionCmb.Size = new Size(238, 28);
        divisionCmb.TabIndex = 0;
        // 
        // categoryCmb
        // 
        categoryCmb.FormattingEnabled = true;
        categoryCmb.Location = new Point(6, 31);
        categoryCmb.Name = "categoryCmb";
        categoryCmb.Size = new Size(238, 28);
        categoryCmb.TabIndex = 1;
        // 
        // textBox1
        // 
        textBox1.Location = new Point(6, 26);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(238, 27);
        textBox1.TabIndex = 2;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(divisionCmb);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(250, 73);
        groupBox1.TabIndex = 3;
        groupBox1.TabStop = false;
        groupBox1.Text = "Division";
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(categoryCmb);
        groupBox2.Location = new Point(282, 12);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(250, 73);
        groupBox2.TabIndex = 4;
        groupBox2.TabStop = false;
        groupBox2.Text = "Category";
        // 
        // dcsCmb
        // 
        dcsCmb.Controls.Add(textBox1);
        dcsCmb.Location = new Point(538, 12);
        dcsCmb.Name = "dcsCmb";
        dcsCmb.Size = new Size(250, 73);
        dcsCmb.TabIndex = 5;
        dcsCmb.TabStop = false;
        dcsCmb.Text = "DCS";
        // 
        // saveBtn
        // 
        saveBtn.Location = new Point(302, 105);
        saveBtn.Name = "saveBtn";
        saveBtn.Size = new Size(211, 29);
        saveBtn.TabIndex = 6;
        saveBtn.Text = "SAVE";
        saveBtn.UseVisualStyleBackColor = true;
        // 
        // MissingDCS
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 155);
        Controls.Add(saveBtn);
        Controls.Add(dcsCmb);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Name = "MissingDCS";
        Text = "Missing DCS";
        Load += MissingDCS_Load;
        groupBox1.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        dcsCmb.ResumeLayout(false);
        dcsCmb.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private ComboBox divisionCmb;
    private ComboBox categoryCmb;
    private TextBox textBox1;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private GroupBox dcsCmb;
    private Button saveBtn;
}