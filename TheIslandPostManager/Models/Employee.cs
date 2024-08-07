﻿using CommunityToolkit.Mvvm.ComponentModel;


namespace TheIslandPostManager.Models;
public partial class Employee : ObservableObject
{
    [ObservableProperty] private int employeeID;
    [ObservableProperty] private string firstName;
    [ObservableProperty] private string lastName;
    [ObservableProperty] private bool isTerminated;
    [ObservableProperty] private int pin;
}