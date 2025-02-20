using CommunityToolkit.Mvvm.ComponentModel;
using IslandPostAdministrator.Models;
using IslandPostAdministrator.Test;
using System.Collections.ObjectModel;
using System.Transactions;
using Transaction = IslandPostAdministrator.Models.Transaction;

namespace IslandPostAdministrator.ViewModels;
public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Transaction> transactions;
    [ObservableProperty] private ObservableCollection<DayBalance> dailyBalance;
    [ObservableProperty] private ObservableCollection<DayBalance> monthlyBalance;

    public DashboardViewModel()
    {
        //LoadRandomData();
    }

    public void GetDailyBalance(DateTime startDate)
    {
        //DailyBalance.Clear();
        //foreach (var day in GetBalance(startDate, 1))
        //{
        //    DailyBalance.Add(day);
        //}
    }

    private void LoadRandomData()
    {
        //Goals = new ObservableCollection<Goal>(SampleDataGenerator.GetGoals());
        Transactions = new ObservableCollection<Transaction>(
            SampleDataGenerator.GenerateTransactions(DateTime.Now.AddDays(-900), DateTime.Now).Where(
                t => t.Date < DateTime.Now.Date
            ));

        

        DailyBalance = new ObservableCollection<DayBalance>(GetBalance(DateTime.Now.Date.AddDays(-30), 1));

        foreach (var item in DailyBalance)
        {
            Random r = new Random();
            item.Balance = r.Next(0, 100);
        }

        MonthlyBalance = new ObservableCollection<DayBalance>(GetBalance(DateTime.Now.Date.AddYears(-1), 2));

    }

    /// <summary>
    /// Get the balance in the specified frequency.
    /// </summary>
    /// <param name="start">start of the frequency</param>
    /// <param name="frequency">1 - daily, 2 - monthly, 3 - yearly</param>
    /// <returns></returns>
    public IEnumerable<DayBalance> GetBalance(DateTime start, int frequency)
    {
        DateTime run = start;
        DateTime? dateTime = null;
        //double balance = 0;
        foreach (var tran in Transactions
            .Where(t => t.Date > start)
            .OrderBy(t => t.Date))
        {
            if (dateTime == null)
            {
                dateTime = tran.Date;
            }
            if (
                ((frequency == 1 && tran.Date.Date.Date != dateTime.Value.Date.Date) ||
                (frequency == 2 && tran.Date.Date.Month != dateTime.Value.Date.Month) ||
                (frequency == 3 && tran.Date.Date.Month != dateTime.Value.Date.Month)) &&
                (tran.Date > start)
                )
            {
                while (tran.Date.Date >= run.Date)
                {
                    if (frequency == 2 || frequency == 3)
                    {
                        run = tran.Date.Date;
                    }
                    DayBalance bal = new DayBalance();
                    bal.Date = run.Date;
                    bal.Balance = tran.Balance;
                    yield return bal;
                    run = run.AddDays(1);
                }
            }
            else
            {
                //balance += tran.Amount;
            }
            dateTime = tran.Date;
            //yield return new DayBalance { Date = tran.Date, Balance = tran.Balance };
        }
    }
}
