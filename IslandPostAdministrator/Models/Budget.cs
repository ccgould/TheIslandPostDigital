using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media;

namespace IslandPostAdministrator.Models;

public partial class Budget : ObservableObject
{
    [ObservableProperty] private string name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SubCategories))]
    private string category;

    [ObservableProperty] private string subCategory;
    [ObservableProperty] private double limit;
    [ObservableProperty] private BudgetType type;
    [ObservableProperty] private double expense;
    [ObservableProperty] private double estimate;
    [ObservableProperty] private double balance;
    [ObservableProperty] private string statusMessage;
    [ObservableProperty] private Brush expenseColor;
    [ObservableProperty] private Brush estimateColor;



    private string ValidateName()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return "Name cannot be empty";
        }
        return null;
    }

    public IEnumerable<string> Categories
    {
        get { return GetCategories(); }
    }

    public Func<IEnumerable<string>> GetCategories { get; set; }

    public IEnumerable<string> SubCategories
    {
        get { return GetSubCategories(Category); }
    }

    public Func<string, IEnumerable<string>> GetSubCategories;

    private string ValidateCategory()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return "Category cannot be empty";
        }
        return null;
    }

    private string ValidateLimit()
    {
        if (Limit < 1)
        {
            return "Limit cannot be less than 1";
        }
        return null;
    }

  
    public Budget Clone()
    {
        return new Budget
        {
            Name = Name,
            Category = Category,
            SubCategory = SubCategory,
            Limit = Limit,
            Balance = Balance,
            Expense = Expense,
            Estimate = Estimate,
            StatusMessage = StatusMessage,
            Type = Type,
        };
    }

    public void Apply(Budget apply)
    {
        Name = apply.Name;
        Category = apply.Category;
        SubCategory = apply.SubCategory;
        Limit = apply.Limit;
        Balance = apply.Balance;
        Expense = apply.Expense;
        Estimate = apply.Estimate;
        StatusMessage = apply.StatusMessage;
        Type = apply.Type;
    }

    GregorianCalendar calendar = new GregorianCalendar();
    public void Update(ObservableCollection<Transaction> transactions, DateTime date)
    {
        Expense = 0;
        foreach (var trans in transactions.Where(
            t =>
                Category == null ? true : t.Category == Category &&
                SubCategory == null ? true : t.SubCategory == SubCategory
        ))
        {
            if (Type == BudgetType.Yearly && trans.Date.Year == date.Year)
            {
                Expense += trans.Amount;
            }
            else if (Type == BudgetType.Monthly && trans.Date.Year == date.Year && trans.Date.Month == date.Month)
            {
                Expense += trans.Amount;
            }
        }
        Expense = -Expense;
        Balance = Math.Max(0, Limit - Expense);
        if (Type == BudgetType.Monthly)
        {
            var numberOfDays = date.Day;
            var maxNumberOfDays = calendar.GetDaysInMonth(date.Year, date.Month);
            var averageExpense = Expense / numberOfDays;
            Estimate = averageExpense * maxNumberOfDays;
        }
        else
        {
            var numberOfDays = calendar.GetDayOfYear(date);
            var maxNumberOfDays = calendar.GetDaysInYear(date.Year);
            var averageExpense = Expense / numberOfDays;
            Estimate = averageExpense * maxNumberOfDays;
        }

        if (Estimate < Limit)
        {
            StatusMessage = "Expense on control";
            ExpenseColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#189E4A"));
            EstimateColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCFCE7"));
        }
        else if (Estimate > Limit)
        {
            if (Expense < Limit)
            {
                StatusMessage = "Risk of overspending";
                ExpenseColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF821C"));
                EstimateColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDD5"));
            }
            else
            {
                StatusMessage = "Overspent";
                ExpenseColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2B4F"));
                EstimateColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEE2E2"));
            }
        }
    }
    public string Error
    {
        get
        {
            return ValidateName() ?? ValidateCategory() ?? ValidateLimit() ?? null;
        }
    }



    //public string this[string columnName]
    //{
    //    get
    //    {
    //        RaisePropertyChanged(nameof(Error));
    //        switch (columnName)
    //        {
    //            case nameof(Name):
    //                return ValidateName();
    //            case nameof(Category):
    //                return ValidateCategory();
    //            case nameof(Limit):
    //                return ValidateLimit();
    //            default:
    //                return null;
    //        }
    //    }
    //}
}

public class AddBudget : Budget { }