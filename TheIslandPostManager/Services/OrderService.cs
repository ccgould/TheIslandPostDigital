using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;

public partial class OrderService : ObservableObject, IOrderService
{
    [ObservableProperty] private ObservableCollection<Image> images;
    [ObservableProperty] private ObservableCollection<Order> currentOrders = new();
    [ObservableProperty] private Order currentOrder;

    public OrderService()
    {
        currentOrders = new ObservableCollection<Order>();

        CreateOrder();
    }

    private void Demo()
    {
        CurrentOrders =
        [
            new Order
            {
                Name = "Creswell Gould",
                DateTime = DateTime.Now,
            }
        ];
    }

    public void CreateOrder()
    {
        var order = new Order();
        CurrentOrders.Add(order);
        CurrentOrder = order;
    }

    public void UpdateOrder(Order order)
    {
        CurrentOrder = order;
    }

    public void DeleteOrder(Order order)
    {
        CurrentOrders.Remove(order);
    }

    public void DeleteOrder(string orderId)
    {
       var order =  CurrentOrders.FirstOrDefault(x => x.CustomerID == orderId);
        DeleteOrder(order);
    }

    public void CancelAll()
    {
        CurrentOrders.Clear();
        CurrentOrder = new();
    }

    public async Task<bool> CompleteOrderAsync(Order order)
    {
        return false;
    }

    public void AddImageToOrder(Image image)
    {
        CurrentOrder.ApprovedImages.Add(image.Copy());
    }

    public void RemoveImageFromOrder(Image image)
    {
        CurrentOrder.ApprovedImages.Remove(image.Copy());
    }

    public void AddImageToOrderPrints(Image image)
    {
        CurrentOrder.ApprovedPrints.Add(image);
    }

    public void RemoveImageFromOrderPrints(Image image)
    {
        CurrentOrder.ApprovedPrints.Remove(image);
    }


}
