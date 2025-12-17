using InventoryPro.Models;

namespace InventoryPro.Interface;
public interface IExportObject
{
    string PONumber { get; set; }
    string FilePath { get; set; }
    string UPC { get; set; }
    
    int Qty {  get; set; }

    POHistory GetPOHistory(bool isCrocs);
}
