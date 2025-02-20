using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandPostAdministrator.Models;
public partial class Employee : ObservableObject
{
    [ObservableProperty] private string name;
    [ObservableProperty] private double earnings;
}
