using System.Collections.ObjectModel;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
public interface IOrderService
{
    ObservableCollection<Image> Images { get; set; }
    ObservableCollection<Order> CurrentOrders{ get; set; }

    void AddImageToOrder(Image image);
    void AddImageToOrderPrints(Image image);
    void CancelAll();
    Task<bool> CompleteOrderAsync(Order order);
    void CreateOrder();
    void DeleteOrder(Order order);
    void DeleteOrder(string orderId);
    void RemoveImageFromOrder(Image image);
    void RemoveImageFromOrderPrints(Image image);
    void UpdateOrder(Order order);
}