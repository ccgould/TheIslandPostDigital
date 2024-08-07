using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIslandPostManager.Dialogs;
public partial class CompletePendingDialogViewModel : ObservableObject
{
    [ObservableProperty] private int selectedIndex;
    [ObservableProperty] private object selectedValue;
    [ObservableProperty] private object selectedItem;

    public CompletePendingDialogViewModel()
    {
        
    }
}
