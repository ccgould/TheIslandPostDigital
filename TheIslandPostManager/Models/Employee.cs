using CommunityToolkit.Mvvm.ComponentModel;


namespace TheIslandPostManager.Models;
public partial class Employee : ObservableObject
{
    [ObservableProperty] private int employeeID;
    [ObservableProperty] private string firstName;
    [ObservableProperty] private string lastName;
    [ObservableProperty] private bool isTerminated;
    [ObservableProperty] private string pin;
    [ObservableProperty] private string salt;
    [ObservableProperty] private string email = "ccgould@yahoo.com";
    private string fullName;

    public string FullName { get => $"{FirstName} {LastName}"; }
}
