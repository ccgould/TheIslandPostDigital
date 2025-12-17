using InventoryPro.Models.Crocs;

namespace InventoryPro;

partial class DuplicateForm
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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DuplicateForm));
        groupBox1 = new GroupBox();
        dataGridView = new DataGridView();
        pONumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        aLUDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        aTTRDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        dESC1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        dESC2DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        sIZEDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        qtyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        vencodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        uDF3DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        uDF4DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        dCSDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOTYPEDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        bILLTODataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        sHIPTODataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOVendCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOPriceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOCOSTDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pRICEDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOPENDINGDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        purchaseOrderExportBindingSource = new BindingSource(components);
        deleteBtn = new Button();
        mergeBtn = new Button();
        label1 = new Label();
        totalAmount = new Label();
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)purchaseOrderExportBindingSource).BeginInit();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox1.Controls.Add(dataGridView);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(1355, 312);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Duplicates List";
        // 
        // dataGridView
        // 
        dataGridView.AutoGenerateColumns = false;
        dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView.Columns.AddRange(new DataGridViewColumn[] { pONumberDataGridViewTextBoxColumn, aLUDataGridViewTextBoxColumn, aTTRDataGridViewTextBoxColumn, dESC1DataGridViewTextBoxColumn, dESC2DataGridViewTextBoxColumn, sIZEDataGridViewTextBoxColumn, qtyDataGridViewTextBoxColumn, vencodeDataGridViewTextBoxColumn, uDF3DataGridViewTextBoxColumn, uDF4DataGridViewTextBoxColumn, dCSDataGridViewTextBoxColumn, pOTYPEDataGridViewTextBoxColumn, bILLTODataGridViewTextBoxColumn, sHIPTODataGridViewTextBoxColumn, pOVendCodeDataGridViewTextBoxColumn, pOPriceDataGridViewTextBoxColumn, pOCOSTDataGridViewTextBoxColumn, pRICEDataGridViewTextBoxColumn, pOPENDINGDataGridViewTextBoxColumn });
        dataGridView.DataSource = purchaseOrderExportBindingSource;
        dataGridView.Dock = DockStyle.Fill;
        dataGridView.Location = new Point(3, 23);
        dataGridView.Name = "dataGridView";
        dataGridView.RowHeadersWidth = 51;
        dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView.Size = new Size(1349, 286);
        dataGridView.TabIndex = 0;
        // 
        // pONumberDataGridViewTextBoxColumn
        // 
        pONumberDataGridViewTextBoxColumn.DataPropertyName = "PONumber";
        pONumberDataGridViewTextBoxColumn.HeaderText = "PONumber";
        pONumberDataGridViewTextBoxColumn.MinimumWidth = 6;
        pONumberDataGridViewTextBoxColumn.Name = "pONumberDataGridViewTextBoxColumn";
        pONumberDataGridViewTextBoxColumn.Width = 125;
        // 
        // aLUDataGridViewTextBoxColumn
        // 
        aLUDataGridViewTextBoxColumn.DataPropertyName = "ALU";
        aLUDataGridViewTextBoxColumn.HeaderText = "ALU";
        aLUDataGridViewTextBoxColumn.MinimumWidth = 6;
        aLUDataGridViewTextBoxColumn.Name = "aLUDataGridViewTextBoxColumn";
        aLUDataGridViewTextBoxColumn.Width = 125;
        // 
        // aTTRDataGridViewTextBoxColumn
        // 
        aTTRDataGridViewTextBoxColumn.DataPropertyName = "ATTR";
        aTTRDataGridViewTextBoxColumn.HeaderText = "ATTR";
        aTTRDataGridViewTextBoxColumn.MinimumWidth = 6;
        aTTRDataGridViewTextBoxColumn.Name = "aTTRDataGridViewTextBoxColumn";
        aTTRDataGridViewTextBoxColumn.Width = 125;
        // 
        // dESC1DataGridViewTextBoxColumn
        // 
        dESC1DataGridViewTextBoxColumn.DataPropertyName = "DESC1";
        dESC1DataGridViewTextBoxColumn.HeaderText = "DESC1";
        dESC1DataGridViewTextBoxColumn.MinimumWidth = 6;
        dESC1DataGridViewTextBoxColumn.Name = "dESC1DataGridViewTextBoxColumn";
        dESC1DataGridViewTextBoxColumn.Width = 125;
        // 
        // dESC2DataGridViewTextBoxColumn
        // 
        dESC2DataGridViewTextBoxColumn.DataPropertyName = "DESC2";
        dESC2DataGridViewTextBoxColumn.HeaderText = "DESC2";
        dESC2DataGridViewTextBoxColumn.MinimumWidth = 6;
        dESC2DataGridViewTextBoxColumn.Name = "dESC2DataGridViewTextBoxColumn";
        dESC2DataGridViewTextBoxColumn.Width = 125;
        // 
        // sIZEDataGridViewTextBoxColumn
        // 
        sIZEDataGridViewTextBoxColumn.DataPropertyName = "SIZE";
        sIZEDataGridViewTextBoxColumn.HeaderText = "SIZE";
        sIZEDataGridViewTextBoxColumn.MinimumWidth = 6;
        sIZEDataGridViewTextBoxColumn.Name = "sIZEDataGridViewTextBoxColumn";
        sIZEDataGridViewTextBoxColumn.Width = 125;
        // 
        // qtyDataGridViewTextBoxColumn
        // 
        qtyDataGridViewTextBoxColumn.DataPropertyName = "Qty";
        qtyDataGridViewTextBoxColumn.HeaderText = "Qty";
        qtyDataGridViewTextBoxColumn.MinimumWidth = 6;
        qtyDataGridViewTextBoxColumn.Name = "qtyDataGridViewTextBoxColumn";
        qtyDataGridViewTextBoxColumn.Width = 125;
        // 
        // vencodeDataGridViewTextBoxColumn
        // 
        vencodeDataGridViewTextBoxColumn.DataPropertyName = "Vencode";
        vencodeDataGridViewTextBoxColumn.HeaderText = "Vencode";
        vencodeDataGridViewTextBoxColumn.MinimumWidth = 6;
        vencodeDataGridViewTextBoxColumn.Name = "vencodeDataGridViewTextBoxColumn";
        vencodeDataGridViewTextBoxColumn.Width = 125;
        // 
        // uDF3DataGridViewTextBoxColumn
        // 
        uDF3DataGridViewTextBoxColumn.DataPropertyName = "UDF3";
        uDF3DataGridViewTextBoxColumn.HeaderText = "UDF3";
        uDF3DataGridViewTextBoxColumn.MinimumWidth = 6;
        uDF3DataGridViewTextBoxColumn.Name = "uDF3DataGridViewTextBoxColumn";
        uDF3DataGridViewTextBoxColumn.Width = 125;
        // 
        // uDF4DataGridViewTextBoxColumn
        // 
        uDF4DataGridViewTextBoxColumn.DataPropertyName = "UDF4";
        uDF4DataGridViewTextBoxColumn.HeaderText = "UDF4";
        uDF4DataGridViewTextBoxColumn.MinimumWidth = 6;
        uDF4DataGridViewTextBoxColumn.Name = "uDF4DataGridViewTextBoxColumn";
        uDF4DataGridViewTextBoxColumn.Width = 125;
        // 
        // dCSDataGridViewTextBoxColumn
        // 
        dCSDataGridViewTextBoxColumn.DataPropertyName = "DCS";
        dCSDataGridViewTextBoxColumn.HeaderText = "DCS";
        dCSDataGridViewTextBoxColumn.MinimumWidth = 6;
        dCSDataGridViewTextBoxColumn.Name = "dCSDataGridViewTextBoxColumn";
        dCSDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOTYPEDataGridViewTextBoxColumn
        // 
        pOTYPEDataGridViewTextBoxColumn.DataPropertyName = "POTYPE";
        pOTYPEDataGridViewTextBoxColumn.HeaderText = "POTYPE";
        pOTYPEDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOTYPEDataGridViewTextBoxColumn.Name = "pOTYPEDataGridViewTextBoxColumn";
        pOTYPEDataGridViewTextBoxColumn.Width = 125;
        // 
        // bILLTODataGridViewTextBoxColumn
        // 
        bILLTODataGridViewTextBoxColumn.DataPropertyName = "BILLTO";
        bILLTODataGridViewTextBoxColumn.HeaderText = "BILLTO";
        bILLTODataGridViewTextBoxColumn.MinimumWidth = 6;
        bILLTODataGridViewTextBoxColumn.Name = "bILLTODataGridViewTextBoxColumn";
        bILLTODataGridViewTextBoxColumn.Width = 125;
        // 
        // sHIPTODataGridViewTextBoxColumn
        // 
        sHIPTODataGridViewTextBoxColumn.DataPropertyName = "SHIPTO";
        sHIPTODataGridViewTextBoxColumn.HeaderText = "SHIPTO";
        sHIPTODataGridViewTextBoxColumn.MinimumWidth = 6;
        sHIPTODataGridViewTextBoxColumn.Name = "sHIPTODataGridViewTextBoxColumn";
        sHIPTODataGridViewTextBoxColumn.Width = 125;
        // 
        // pOVendCodeDataGridViewTextBoxColumn
        // 
        pOVendCodeDataGridViewTextBoxColumn.DataPropertyName = "POVendCode";
        pOVendCodeDataGridViewTextBoxColumn.HeaderText = "POVendCode";
        pOVendCodeDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOVendCodeDataGridViewTextBoxColumn.Name = "pOVendCodeDataGridViewTextBoxColumn";
        pOVendCodeDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOPriceDataGridViewTextBoxColumn
        // 
        pOPriceDataGridViewTextBoxColumn.DataPropertyName = "POPrice";
        pOPriceDataGridViewTextBoxColumn.HeaderText = "POPrice";
        pOPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOPriceDataGridViewTextBoxColumn.Name = "pOPriceDataGridViewTextBoxColumn";
        pOPriceDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOCOSTDataGridViewTextBoxColumn
        // 
        pOCOSTDataGridViewTextBoxColumn.DataPropertyName = "POCOST";
        pOCOSTDataGridViewTextBoxColumn.HeaderText = "POCOST";
        pOCOSTDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOCOSTDataGridViewTextBoxColumn.Name = "pOCOSTDataGridViewTextBoxColumn";
        pOCOSTDataGridViewTextBoxColumn.Width = 125;
        // 
        // pRICEDataGridViewTextBoxColumn
        // 
        pRICEDataGridViewTextBoxColumn.DataPropertyName = "PRICE";
        pRICEDataGridViewTextBoxColumn.HeaderText = "PRICE";
        pRICEDataGridViewTextBoxColumn.MinimumWidth = 6;
        pRICEDataGridViewTextBoxColumn.Name = "pRICEDataGridViewTextBoxColumn";
        pRICEDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOPENDINGDataGridViewTextBoxColumn
        // 
        pOPENDINGDataGridViewTextBoxColumn.DataPropertyName = "POPENDING";
        pOPENDINGDataGridViewTextBoxColumn.HeaderText = "POPENDING";
        pOPENDINGDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOPENDINGDataGridViewTextBoxColumn.Name = "pOPENDINGDataGridViewTextBoxColumn";
        pOPENDINGDataGridViewTextBoxColumn.Width = 125;
        // 
        // purchaseOrderExportBindingSource
        // 
        purchaseOrderExportBindingSource.DataSource = typeof(CrocsPurchaseOrderExport);
        // 
        // deleteBtn
        // 
        deleteBtn.Anchor = AnchorStyles.Bottom;
        deleteBtn.FlatStyle = FlatStyle.Flat;
        deleteBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        deleteBtn.Image = (Image)resources.GetObject("deleteBtn.Image");
        deleteBtn.ImageAlign = ContentAlignment.TopCenter;
        deleteBtn.Location = new Point(636, 334);
        deleteBtn.Name = "deleteBtn";
        deleteBtn.Size = new Size(104, 104);
        deleteBtn.TabIndex = 6;
        deleteBtn.Text = "Delete";
        deleteBtn.TextAlign = ContentAlignment.BottomCenter;
        deleteBtn.UseVisualStyleBackColor = true;
        deleteBtn.Click += deleteBtn_Click;
        // 
        // mergeBtn
        // 
        mergeBtn.Anchor = AnchorStyles.Bottom;
        mergeBtn.FlatStyle = FlatStyle.Flat;
        mergeBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        mergeBtn.Image = (Image)resources.GetObject("mergeBtn.Image");
        mergeBtn.ImageAlign = ContentAlignment.TopCenter;
        mergeBtn.Location = new Point(513, 334);
        mergeBtn.Name = "mergeBtn";
        mergeBtn.Size = new Size(104, 104);
        mergeBtn.TabIndex = 5;
        mergeBtn.Text = "Merge";
        mergeBtn.TextAlign = ContentAlignment.BottomCenter;
        mergeBtn.UseVisualStyleBackColor = true;
        mergeBtn.Click += mergeBtn_Click;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        label1.Location = new Point(15, 418);
        label1.Name = "label1";
        label1.Size = new Size(48, 20);
        label1.TabIndex = 7;
        label1.Text = "Total:";
        // 
        // totalAmount
        // 
        totalAmount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        totalAmount.AutoSize = true;
        totalAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        totalAmount.Location = new Point(69, 419);
        totalAmount.Name = "totalAmount";
        totalAmount.Size = new Size(34, 20);
        totalAmount.TabIndex = 8;
        totalAmount.Text = "0/0";
        // 
        // DuplicateForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1379, 450);
        Controls.Add(totalAmount);
        Controls.Add(label1);
        Controls.Add(deleteBtn);
        Controls.Add(mergeBtn);
        Controls.Add(groupBox1);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Name = "DuplicateForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Duplicates";
        Load += DuplicateForm_Load;
        groupBox1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)purchaseOrderExportBindingSource).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox groupBox1;
    private DataGridView dataGridView;
    private DataGridViewTextBoxColumn pONumberDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn aLUDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn aTTRDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn dESC1DataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn dESC2DataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn sIZEDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn qtyDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn upcDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn vencodeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn uDF3DataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn uDF4DataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn dCSDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOTYPEDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn bILLTODataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn sHIPTODataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOVendCodeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOPriceDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOCOSTDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pRICEDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOPENDINGDataGridViewTextBoxColumn;
    private BindingSource purchaseOrderExportBindingSource;
    private Button deleteBtn;
    private Button mergeBtn;
    private Label label1;
    private Label totalAmount;
}