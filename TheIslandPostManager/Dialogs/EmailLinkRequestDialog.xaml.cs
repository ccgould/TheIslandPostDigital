using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Dialogs;
/// <summary>
/// Interaction logic for EmailLinkRequestDialog.xaml
/// </summary>
public partial class EmailLinkRequestDialog : ContentDialog,INotifyPropertyChanged
{
    private string link;

    public string Link
    {
        get
        {
            return link;
        }

        set
        {
            link = value;
            OnPropertyChanged();
        }
    }

    public EmailLinkRequestDialog(ContentPresenter contentPresenter) : base(contentPresenter)
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    protected override void OnButtonClick(ContentDialogButton button)
    {
        base.OnButtonClick(button);
    }
}
