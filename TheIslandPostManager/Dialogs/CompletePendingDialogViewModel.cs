using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.Dialogs;
public partial class CompletePendingDialogViewModel : ObservableObject
{
    [ObservableProperty] private int selectedIndex;
    [ObservableProperty] private object selectedValue;
    [ObservableProperty] private object selectedItem;
    [ObservableProperty] private ObservableCollection<Employee> employees;

    public CompletePendingDialogViewModel(IMySQLService mySQLService)
    {
        Employees = mySQLService.GetEmployees().Result;
    }
}
