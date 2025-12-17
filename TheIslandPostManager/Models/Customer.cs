using CommunityToolkit.Mvvm.ComponentModel;

namespace TheIslandPostManager.Models;

public partial class Customer : ObservableObject
{
    [ObservableProperty]private int id;
    [ObservableProperty]private string fullName;
    [ObservableProperty]private string email;
    [ObservableProperty]private DateOnly joined_date;
    [ObservableProperty]private TimeOnly joined_time;
    [ObservableProperty]private int returnedCount;
    [ObservableProperty]private DateOnly birthday;
    [ObservableProperty]private bool is_member;
}