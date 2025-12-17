using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
public interface IOrderService
{
    //ObservableCollection<Image> Images { get; set; }
    ObservableCollection<Order> CurrentOrders{ get; set; }
    ObservableCollection<Order> PurchaseHistory{ get; set; }
    bool ShowImageViewer { get; set; }
    bool ShowGridViewer { get; set; }
    Order CurrentHistoryOrder { get; set; }

    Order CurrentOrder { get; set; }
    bool IsWatermarkVisible { get; set; }
    void AddImageToOrder(ImageObj image);
    void AddImageToOrderPrints(ImageObj image);
    void Cancel(Order order);
    Task CancelAll();
    Task CompleteOrderAsync();
    Task CreateOrder(bool copy = false);
    void DeleteAllImages();
    void DeleteOrder(Order order);
    void DeleteOrder(int orderId);
    Task DeletePendingOrder(Order order);
    int GetOrderCount();
    Task GetPendingOrders();
    Task OpenOrderFromPending(Order order);
    Task PendOrder(string name);
    Task ExportOrder();
    void RemoveImageFromOrder(ImageObj image);
    void RemoveImageFromOrderPrints(ImageObj image);
    void SetAsMaybe(ImageObj image);
    void UpdateOrder(Order order);
    void CurrentDeleteOrder();
}