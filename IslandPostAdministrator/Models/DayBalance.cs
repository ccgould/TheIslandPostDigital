using CommunityToolkit.Mvvm.ComponentModel;

namespace IslandPostAdministrator.Models;
public partial class DayBalance : ObservableObject
{
    [ObservableProperty] private DateTime date;
    [ObservableProperty] private double balance;
}