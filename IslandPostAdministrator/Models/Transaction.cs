#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandPostAdministrator.Models;

public partial class Transaction : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SubCategories))]
    private string category;
    [ObservableProperty] private string subCategory;
    [ObservableProperty] private DateTime date;
    [ObservableProperty] private double amount;
    [ObservableProperty] private string description;


    private string ValidateCategory()
    {
        if (string.IsNullOrEmpty(Category))
        {
            return "Category cannot be empty";
        }
        return null;
    }
    private string ValidateSubCategory()
    {
        if (string.IsNullOrEmpty(SubCategory))
        {
            return "SubCategory cannot be empty";
        }
        return null;
    }

    private string ValidateDate()
    {
        if (Date == null)
        {
            return "Date cannot be empty";
        }
        else if (Date > DateTime.Now.Date)
        {
            return "Date cannot be greater than today";
        }
        return null;
    }

    private string ValidateAmount()
    {
        if (Amount == 0)
        {
            return "Amount cannot be zero";
        }
        return null;
    }

    [ObservableProperty] private double balance;

    public IEnumerable<string> Categories
    {
        get { return GetCategories != null ? GetCategories() : null; }
    }

    public Func<IEnumerable<string>> GetCategories { get; set; }

    public IEnumerable<string> SubCategories
    {
        get { return GetSubCategories != null ? GetSubCategories(Category) : null; }
    }

    public Func<string, IEnumerable<string>> GetSubCategories;

    public string Error
    {
        get
        {
            return ValidateCategory() ?? ValidateSubCategory() ?? ValidateDate() ?? ValidateAmount();
        }
    }

    //public string this[string columnName]
    //{
    //    get
    //    {
    //        RaisePropertyChanged(nameof(Error));
    //        switch (columnName)
    //        {
    //            case nameof(Category):
    //                return ValidateCategory();
    //            case nameof(SubCategory):
    //                return ValidateSubCategory();
    //            case nameof(Date):
    //                return ValidateDate();
    //            case nameof(Amount):
    //                return ValidateAmount();
    //        }
    //        return null;
    //    }
    //}

    internal Transaction Clone()
    {
        return new Transaction()
        {
            Category = Category,
            SubCategory = SubCategory,
            Date = Date,
            Amount = Amount,
            Description = Description,
            Balance = Balance
        };
    }

    internal void Apply(Transaction transaction)
    {
        Category = transaction.Category;
        SubCategory = transaction.SubCategory;
        Date = transaction.Date;
        Amount = transaction.Amount;
        Description = transaction.Description;
        Balance = transaction.Balance;
    }
}

public partial class CategoryExpense : ObservableObject
{
    [ObservableProperty] private string category;
    [ObservableProperty] private double amount;
    [ObservableProperty] private double percentage;
}
