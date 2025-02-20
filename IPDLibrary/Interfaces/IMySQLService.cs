using System.Collections.ObjectModel;



namespace IPDLibrary.Interfaces;

public interface IMySQLService
{
    Task AddCompletedOrder(Order currentOrder);
    Task AddPendingOrder(Order order);
    Task<ObservableCollection<Employee>> GetEmployees(bool getAdminOnly = false);
    Task<ObservableCollection<Order>> GetPendingOrders();
    Task<ObservableCollection<Order>> GetPurchaseHistory(DateTime startTime, DateTime endTime, string searchText, string clerkName);
    Task<ObservableCollection<PurchaseItem>> GetStoreItems(bool retailOnly = false);
    Task<double> GetTodaysTotal(DateTime date, ViewModels.EarningsPageViewmodel.TransactionType transactionType);
    Task<Tuple<ObservableCollection<ImageObj>, List<IImage>, List<IImage>>?> LoadImages(string id);
    Task RemovePendingOrder(Order order);
    void SetConnectionString();
    Task UpdateHistoryOrder(Order order);
    Task<bool> ValidatePin(int employeeID, string link);
}