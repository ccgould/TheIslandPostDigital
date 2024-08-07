﻿using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
public interface IOrderService
{
    //ObservableCollection<Image> Images { get; set; }
    ObservableCollection<Order> CurrentOrders{ get; set; }
    ObservableCollection<Order> PurchaseHistory{ get; set; }

    Order CurrentOrder { get; set; }
    void AddImageToOrder(ImageObj image);
    void AddImageToOrderPrints(ImageObj image);
    void Cancel(Order order);
    Task CancelAll();
    Task CompleteOrderAsync();
    Task CreateOrder(bool copy = false);
    void DeleteOrder(Order order);
    void DeleteOrder(string orderId);
    Task DeletePendingOrder(Order order);
    Task OpenOrderFromPending(Order order);
    Task PendOrder(string name);
    void RemoveImageFromOrder(ImageObj image);
    void RemoveImageFromOrderPrints(ImageObj image);
    void SetAsMaybe(ImageObj image);
    void UpdateOrder(Order order);
}