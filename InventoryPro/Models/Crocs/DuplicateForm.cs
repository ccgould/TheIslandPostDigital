﻿using InventoryPro.Models.Crocs;
using System.ComponentModel;

namespace InventoryPro;
public partial class DuplicateForm : Form
{
    private BindingSource _source;
    private Queue<BindingList<CrocsPurchaseOrderExport>> pairs = new();
    private List<CrocsPurchaseOrderExport> purchaseOrderExport;
    private BindingList<CrocsPurchaseOrderExport> _currentData;

    public DuplicateForm(IEnumerable<IGrouping<object, CrocsPurchaseOrderExport>> duplicates, List<CrocsPurchaseOrderExport> purchaseOrderExport)
    {
        _source = new BindingSource();
        this.purchaseOrderExport = purchaseOrderExport;
        InitializeComponent();

        foreach (var dup in duplicates)
        {
            var items = new BindingList<CrocsPurchaseOrderExport>();
            foreach (var item in dup)
            {
                items.Add(item);
            }
            
            pairs.Enqueue(items);
        }

        getData();
    }

    public DuplicateForm(IGrouping<object, CrocsPurchaseOrderExport> duplicates, List<CrocsPurchaseOrderExport> purchaseOrderExport,int amountRemaing)
    {
        _source = new BindingSource();
        this.purchaseOrderExport = purchaseOrderExport;
        InitializeComponent();

        var items = new BindingList<CrocsPurchaseOrderExport>();
        foreach (var item in duplicates)
        {
            items.Add(item);
        }

        pairs.Enqueue(items);

        getData();

        totalAmount.Text = amountRemaing.ToString() ;
    }

    private void DuplicateForm_Load(object sender, EventArgs e)
    {

    }

    private void mergeBtn_Click(object sender, EventArgs e)
    {
        var total = 0;

        foreach (var item in _currentData)
        {
            total += item.Qty;
        }

        for (int index = 0; index < _currentData.Count - 1; index++)
        {
            purchaseOrderExport.Remove(_currentData[index]);
            _currentData.RemoveAt(index);
        }

        _currentData[0].Qty = total;

        getData();

        if(!pairs.Any())
        {
            Close();
        }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow item in dataGridView.SelectedRows)
        {
           var data =  (CrocsPurchaseOrderExport)item.DataBoundItem;
            purchaseOrderExport.Remove(data);
            _currentData.Remove(data);
        }

        if(_currentData.Count <= 1)
        {
            getData();
        }
    }

    private void getData()
    {
        if (pairs.Any()) 
        {
            _currentData = pairs.Dequeue();
            _source.DataSource = _currentData;
            dataGridView.DataSource = _source;
        }
    }

    private const int CP_NOCLOSE_BUTTON = 0x200;
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams myCp = base.CreateParams;
            myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
            return myCp;
        }
    }
}
