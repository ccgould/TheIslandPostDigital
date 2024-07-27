using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
public interface IOrderService
{
    //ObservableCollection<Image> Images { get; set; }
    ObservableCollection<Order> CurrentOrders{ get; set; }
    ObservableCollection<Order> PurchaseHistory{ get; set; }

    Order CurrentOrder { get; set; }
    void AddImageToOrder(Image image);
    void AddImageToOrderPrints(Image image);
    void Cancel(Order order);
    Task CancelAll();
    Task CompleteOrderAsync();
    Task CreateOrder(bool copy = false);
    void DeleteOrder(Order order);
    void DeleteOrder(string orderId);
    void RemoveImageFromOrder(Image image);
    void RemoveImageFromOrderPrints(Image image);
    void UpdateOrder(Order order);
}