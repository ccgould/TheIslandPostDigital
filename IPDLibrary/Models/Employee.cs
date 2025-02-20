using CommunityToolkit.Mvvm.ComponentModel;


namespace IPDLibrary.Models;
public partial class Employee : ObservableObject
{
    [ObservableProperty] private int employeeID;
    [ObservableProperty] private string firstName;
    [ObservableProperty] private string lastName;
    [ObservableProperty] private bool isTerminated;
    [ObservableProperty] private string pin;
    [ObservableProperty] private string salt;
    private string fullName;

    public string FullName { get => $"{FirstName} {LastName}"; }
}
