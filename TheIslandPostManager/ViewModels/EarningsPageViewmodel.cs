using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.ViewModels;
public partial class EarningsPageViewmodel : ObservableObject
{
    private readonly IMySQLService service;
    [ObservableProperty] private double earnings;
    [ObservableProperty] private double cashEarnings;
    [ObservableProperty] private double cardEarnings;
    [ObservableProperty] private double bothEarnings;
    public EarningsPageViewmodel(IMySQLService service)
    {
        Task.Run(Refresh);
        this.service = service;
    }

    [RelayCommand]
    private async Task Refresh()
    {         
        Earnings = await service.GetTodaysTotal(DateTime.Today,TransactionType.Total);
        CashEarnings = await service.GetTodaysTotal(DateTime.Today,TransactionType.Cash);
        CardEarnings = await service.GetTodaysTotal(DateTime.Today,TransactionType.Card);
        BothEarnings = await service.GetTodaysTotal(DateTime.Today,TransactionType.Both);
    }

    public enum TransactionType
    {
        Total = 0,
        Cash = 1,
        Card = 2,
        Both = 3
    }
}
