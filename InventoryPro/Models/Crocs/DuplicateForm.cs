using InventoryPro.Interface;
using InventoryPro.Models.Crocs;
using InventoryPro.Services;
using System.ComponentModel;

namespace InventoryPro;
public partial class DuplicateForm : Form
{
    private BindingSource _source;
    private IGrouping<object, IExportObject> duplicates;
    private Queue<BindingList<IExportObject>> pairs = new();
    private readonly Form1 parent;
    private List<IExportObject> purchaseOrderExport;
    private BindingList<IExportObject> _currentData;

    public DuplicateForm(IGrouping<object, IExportObject> duplicates, List<IExportObject> purchaseOrderExport, int amountRemaing)
    {
        _source = new BindingSource();
        this.duplicates = duplicates;
        this.parent = parent;
        this.purchaseOrderExport = purchaseOrderExport;
        InitializeComponent();

        var items = new BindingList<IExportObject>();
        foreach (var item in duplicates)
        {
            items.Add(item);
        }

        pairs.Enqueue(items);

        getData();

        totalAmount.Text = amountRemaing.ToString();
    }

    private void DuplicateForm_Load(object sender, EventArgs e)
    {

    }

    private void mergeBtn_Click(object sender, EventArgs e)
    {
        DataHandler.Merge(duplicates, purchaseOrderExport);

        getData();

        if (!pairs.Any())
        {
            Close();
        }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow item in dataGridView.SelectedRows)
        {
            var data = (CrocsPurchaseOrderExport)item.DataBoundItem;
            purchaseOrderExport.Remove(data);
            _currentData.Remove(data);
        }

        if (_currentData.Count <= 1)
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
