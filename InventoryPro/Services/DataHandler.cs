using InventoryPro.Interface;

namespace InventoryPro.Services;
internal static class DataHandler
{
    internal static void Merge(IGrouping<object, IExportObject> currentData, List<IExportObject> purchaseOrderExport)
    {
        var total = 0;

        foreach (var item in currentData)
        {
            total += item.Qty;
        }

        var data = currentData.ToList();

        for (int index = 0; index < data.Count - 1; index++)
        {
            purchaseOrderExport.Remove(data[index]);
            data.RemoveAt(index);
        }

        data[0].Qty = total;
    }
}
