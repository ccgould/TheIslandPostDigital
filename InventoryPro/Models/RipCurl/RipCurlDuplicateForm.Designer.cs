using InventoryPro.Models.Crocs;

namespace InventoryPro;

partial class RipCurlDuplicateForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RipCurlDuplicateForm));
        groupBox1 = new GroupBox();
        dataGridView = new DataGridView();
        localUPCDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        dCSCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        attrDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        sizeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        desc1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        desc2DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        priceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        costDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        taxCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        uPCDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        vendorCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pONumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOVendorCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        orderDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        shipDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        cancelDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOPriceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        pOCostDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        orderQtyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        billToStoreNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        shipToStoreNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        ripCurlPurchaseOrderExportBindingSource2 = new BindingSource(components);
        purchaseOrderExportBindingSource = new BindingSource(components);
        deleteBtn = new Button();
        mergeBtn = new Button();
        label1 = new Label();
        totalAmount = new Label();
        ripCurlPurchaseOrderExportBindingSource = new BindingSource(components);
        ripCurlPurchaseOrderExportBindingSource1 = new BindingSource(components);
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)ripCurlPurchaseOrderExportBindingSource2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)purchaseOrderExportBindingSource).BeginInit();
        ((System.ComponentModel.ISupportInitialize)ripCurlPurchaseOrderExportBindingSource).BeginInit();
        ((System.ComponentModel.ISupportInitialize)ripCurlPurchaseOrderExportBindingSource1).BeginInit();
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
        dataGridView.Columns.AddRange(new DataGridViewColumn[] { localUPCDataGridViewTextBoxColumn, dCSCodeDataGridViewTextBoxColumn, attrDataGridViewTextBoxColumn, sizeDataGridViewTextBoxColumn, desc1DataGridViewTextBoxColumn, desc2DataGridViewTextBoxColumn, priceDataGridViewTextBoxColumn, costDataGridViewTextBoxColumn, taxCodeDataGridViewTextBoxColumn, uPCDataGridViewTextBoxColumn, vendorCodeDataGridViewTextBoxColumn, pONumberDataGridViewTextBoxColumn, pOVendorCodeDataGridViewTextBoxColumn, orderDateDataGridViewTextBoxColumn, shipDateDataGridViewTextBoxColumn, cancelDateDataGridViewTextBoxColumn, pOPriceDataGridViewTextBoxColumn, pOCostDataGridViewTextBoxColumn, orderQtyDataGridViewTextBoxColumn, billToStoreNumberDataGridViewTextBoxColumn, shipToStoreNumberDataGridViewTextBoxColumn });
        dataGridView.DataSource = ripCurlPurchaseOrderExportBindingSource2;
        dataGridView.Dock = DockStyle.Fill;
        dataGridView.Location = new Point(3, 23);
        dataGridView.Name = "dataGridView";
        dataGridView.RowHeadersWidth = 51;
        dataGridView.Size = new Size(1349, 286);
        dataGridView.TabIndex = 0;
        // 
        // localUPCDataGridViewTextBoxColumn
        // 
        localUPCDataGridViewTextBoxColumn.DataPropertyName = "LocalUPC";
        localUPCDataGridViewTextBoxColumn.HeaderText = "Local UPC";
        localUPCDataGridViewTextBoxColumn.MinimumWidth = 6;
        localUPCDataGridViewTextBoxColumn.Name = "localUPCDataGridViewTextBoxColumn";
        localUPCDataGridViewTextBoxColumn.Width = 125;
        // 
        // dCSCodeDataGridViewTextBoxColumn
        // 
        dCSCodeDataGridViewTextBoxColumn.DataPropertyName = "DCSCode";
        dCSCodeDataGridViewTextBoxColumn.HeaderText = "DCS Code";
        dCSCodeDataGridViewTextBoxColumn.MinimumWidth = 6;
        dCSCodeDataGridViewTextBoxColumn.Name = "dCSCodeDataGridViewTextBoxColumn";
        dCSCodeDataGridViewTextBoxColumn.Width = 125;
        // 
        // attrDataGridViewTextBoxColumn
        // 
        attrDataGridViewTextBoxColumn.DataPropertyName = "Attr";
        attrDataGridViewTextBoxColumn.HeaderText = "Attr";
        attrDataGridViewTextBoxColumn.MinimumWidth = 6;
        attrDataGridViewTextBoxColumn.Name = "attrDataGridViewTextBoxColumn";
        attrDataGridViewTextBoxColumn.Width = 125;
        // 
        // sizeDataGridViewTextBoxColumn
        // 
        sizeDataGridViewTextBoxColumn.DataPropertyName = "Size";
        sizeDataGridViewTextBoxColumn.HeaderText = "Size";
        sizeDataGridViewTextBoxColumn.MinimumWidth = 6;
        sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
        sizeDataGridViewTextBoxColumn.Width = 125;
        // 
        // desc1DataGridViewTextBoxColumn
        // 
        desc1DataGridViewTextBoxColumn.DataPropertyName = "Desc1";
        desc1DataGridViewTextBoxColumn.HeaderText = "Desc1";
        desc1DataGridViewTextBoxColumn.MinimumWidth = 6;
        desc1DataGridViewTextBoxColumn.Name = "desc1DataGridViewTextBoxColumn";
        desc1DataGridViewTextBoxColumn.Width = 125;
        // 
        // desc2DataGridViewTextBoxColumn
        // 
        desc2DataGridViewTextBoxColumn.DataPropertyName = "Desc2";
        desc2DataGridViewTextBoxColumn.HeaderText = "Desc2";
        desc2DataGridViewTextBoxColumn.MinimumWidth = 6;
        desc2DataGridViewTextBoxColumn.Name = "desc2DataGridViewTextBoxColumn";
        desc2DataGridViewTextBoxColumn.Width = 125;
        // 
        // priceDataGridViewTextBoxColumn
        // 
        priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
        priceDataGridViewTextBoxColumn.HeaderText = "Price";
        priceDataGridViewTextBoxColumn.MinimumWidth = 6;
        priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
        priceDataGridViewTextBoxColumn.Width = 125;
        // 
        // costDataGridViewTextBoxColumn
        // 
        costDataGridViewTextBoxColumn.DataPropertyName = "Cost";
        costDataGridViewTextBoxColumn.HeaderText = "Cost";
        costDataGridViewTextBoxColumn.MinimumWidth = 6;
        costDataGridViewTextBoxColumn.Name = "costDataGridViewTextBoxColumn";
        costDataGridViewTextBoxColumn.Width = 125;
        // 
        // taxCodeDataGridViewTextBoxColumn
        // 
        taxCodeDataGridViewTextBoxColumn.DataPropertyName = "TaxCode";
        taxCodeDataGridViewTextBoxColumn.HeaderText = "Tax Code";
        taxCodeDataGridViewTextBoxColumn.MinimumWidth = 6;
        taxCodeDataGridViewTextBoxColumn.Name = "taxCodeDataGridViewTextBoxColumn";
        taxCodeDataGridViewTextBoxColumn.Width = 125;
        // 
        // uPCDataGridViewTextBoxColumn
        // 
        uPCDataGridViewTextBoxColumn.DataPropertyName = "UPC";
        uPCDataGridViewTextBoxColumn.HeaderText = "UPC";
        uPCDataGridViewTextBoxColumn.MinimumWidth = 6;
        uPCDataGridViewTextBoxColumn.Name = "uPCDataGridViewTextBoxColumn";
        uPCDataGridViewTextBoxColumn.Width = 125;
        // 
        // vendorCodeDataGridViewTextBoxColumn
        // 
        vendorCodeDataGridViewTextBoxColumn.DataPropertyName = "VendorCode";
        vendorCodeDataGridViewTextBoxColumn.HeaderText = "Vendor Code";
        vendorCodeDataGridViewTextBoxColumn.MinimumWidth = 6;
        vendorCodeDataGridViewTextBoxColumn.Name = "vendorCodeDataGridViewTextBoxColumn";
        vendorCodeDataGridViewTextBoxColumn.Width = 125;
        // 
        // pONumberDataGridViewTextBoxColumn
        // 
        pONumberDataGridViewTextBoxColumn.DataPropertyName = "PONumber";
        pONumberDataGridViewTextBoxColumn.HeaderText = "PO Number";
        pONumberDataGridViewTextBoxColumn.MinimumWidth = 6;
        pONumberDataGridViewTextBoxColumn.Name = "pONumberDataGridViewTextBoxColumn";
        pONumberDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOVendorCodeDataGridViewTextBoxColumn
        // 
        pOVendorCodeDataGridViewTextBoxColumn.DataPropertyName = "POVendorCode";
        pOVendorCodeDataGridViewTextBoxColumn.HeaderText = "PO Vendor Code";
        pOVendorCodeDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOVendorCodeDataGridViewTextBoxColumn.Name = "pOVendorCodeDataGridViewTextBoxColumn";
        pOVendorCodeDataGridViewTextBoxColumn.Width = 125;
        // 
        // orderDateDataGridViewTextBoxColumn
        // 
        orderDateDataGridViewTextBoxColumn.DataPropertyName = "OrderDate";
        orderDateDataGridViewTextBoxColumn.HeaderText = "Order Date";
        orderDateDataGridViewTextBoxColumn.MinimumWidth = 6;
        orderDateDataGridViewTextBoxColumn.Name = "orderDateDataGridViewTextBoxColumn";
        orderDateDataGridViewTextBoxColumn.Width = 125;
        // 
        // shipDateDataGridViewTextBoxColumn
        // 
        shipDateDataGridViewTextBoxColumn.DataPropertyName = "ShipDate";
        shipDateDataGridViewTextBoxColumn.HeaderText = "Ship Date";
        shipDateDataGridViewTextBoxColumn.MinimumWidth = 6;
        shipDateDataGridViewTextBoxColumn.Name = "shipDateDataGridViewTextBoxColumn";
        shipDateDataGridViewTextBoxColumn.Width = 125;
        // 
        // cancelDateDataGridViewTextBoxColumn
        // 
        cancelDateDataGridViewTextBoxColumn.DataPropertyName = "CancelDate";
        cancelDateDataGridViewTextBoxColumn.HeaderText = "Cancel Date";
        cancelDateDataGridViewTextBoxColumn.MinimumWidth = 6;
        cancelDateDataGridViewTextBoxColumn.Name = "cancelDateDataGridViewTextBoxColumn";
        cancelDateDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOPriceDataGridViewTextBoxColumn
        // 
        pOPriceDataGridViewTextBoxColumn.DataPropertyName = "POPrice";
        pOPriceDataGridViewTextBoxColumn.HeaderText = "PO Price";
        pOPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOPriceDataGridViewTextBoxColumn.Name = "pOPriceDataGridViewTextBoxColumn";
        pOPriceDataGridViewTextBoxColumn.Width = 125;
        // 
        // pOCostDataGridViewTextBoxColumn
        // 
        pOCostDataGridViewTextBoxColumn.DataPropertyName = "POCost";
        pOCostDataGridViewTextBoxColumn.HeaderText = "PO Cost";
        pOCostDataGridViewTextBoxColumn.MinimumWidth = 6;
        pOCostDataGridViewTextBoxColumn.Name = "pOCostDataGridViewTextBoxColumn";
        pOCostDataGridViewTextBoxColumn.Width = 125;
        // 
        // orderQtyDataGridViewTextBoxColumn
        // 
        orderQtyDataGridViewTextBoxColumn.DataPropertyName = "OrderQty";
        orderQtyDataGridViewTextBoxColumn.HeaderText = "Order Qty";
        orderQtyDataGridViewTextBoxColumn.MinimumWidth = 6;
        orderQtyDataGridViewTextBoxColumn.Name = "orderQtyDataGridViewTextBoxColumn";
        orderQtyDataGridViewTextBoxColumn.Width = 125;
        // 
        // billToStoreNumberDataGridViewTextBoxColumn
        // 
        billToStoreNumberDataGridViewTextBoxColumn.DataPropertyName = "BillToStoreNumber";
        billToStoreNumberDataGridViewTextBoxColumn.HeaderText = "Bill To Store Number";
        billToStoreNumberDataGridViewTextBoxColumn.MinimumWidth = 6;
        billToStoreNumberDataGridViewTextBoxColumn.Name = "billToStoreNumberDataGridViewTextBoxColumn";
        billToStoreNumberDataGridViewTextBoxColumn.Width = 125;
        // 
        // shipToStoreNumberDataGridViewTextBoxColumn
        // 
        shipToStoreNumberDataGridViewTextBoxColumn.DataPropertyName = "ShipToStoreNumber";
        shipToStoreNumberDataGridViewTextBoxColumn.HeaderText = "Ship To Store Number";
        shipToStoreNumberDataGridViewTextBoxColumn.MinimumWidth = 6;
        shipToStoreNumberDataGridViewTextBoxColumn.Name = "shipToStoreNumberDataGridViewTextBoxColumn";
        shipToStoreNumberDataGridViewTextBoxColumn.Width = 125;
        // 
        // ripCurlPurchaseOrderExportBindingSource2
        // 
        ripCurlPurchaseOrderExportBindingSource2.DataSource = typeof(RipCurlPurchaseOrderExport);
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
        deleteBtn.Location = new Point(675, 334);
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
        mergeBtn.Location = new Point(552, 334);
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
        // ripCurlPurchaseOrderExportBindingSource
        // 
        ripCurlPurchaseOrderExportBindingSource.DataSource = typeof(RipCurlPurchaseOrderExport);
        // 
        // ripCurlPurchaseOrderExportBindingSource1
        // 
        ripCurlPurchaseOrderExportBindingSource1.DataSource = typeof(RipCurlPurchaseOrderExport);
        // 
        // RipCurlDuplicateForm
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
        MaximizeBox = false;
        Name = "RipCurlDuplicateForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Duplicates";
        Load += DuplicateForm_Load;
        groupBox1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)ripCurlPurchaseOrderExportBindingSource2).EndInit();
        ((System.ComponentModel.ISupportInitialize)purchaseOrderExportBindingSource).EndInit();
        ((System.ComponentModel.ISupportInitialize)ripCurlPurchaseOrderExportBindingSource).EndInit();
        ((System.ComponentModel.ISupportInitialize)ripCurlPurchaseOrderExportBindingSource1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox groupBox1;
    private BindingSource purchaseOrderExportBindingSource;
    private Button deleteBtn;
    private Button mergeBtn;
    private Label label1;
    private Label totalAmount;
    private DataGridView dataGridView;
    private BindingSource ripCurlPurchaseOrderExportBindingSource2;
    private BindingSource ripCurlPurchaseOrderExportBindingSource;
    private BindingSource ripCurlPurchaseOrderExportBindingSource1;
    private DataGridViewTextBoxColumn localUPCDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn dCSCodeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn attrDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn desc1DataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn desc2DataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn costDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn taxCodeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn uPCDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn vendorCodeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pONumberDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOVendorCodeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn orderDateDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn shipDateDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn cancelDateDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOPriceDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn pOCostDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn orderQtyDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn billToStoreNumberDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn shipToStoreNumberDataGridViewTextBoxColumn;
}