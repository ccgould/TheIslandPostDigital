using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;
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

    private Order order;
    private readonly IEmailService emailService;
    private readonly IMySQLService mySQLService;

    public Order Order
    {
        get
        {
            return order;
        }

        set
        {
            order = value;
            OnPropertyChanged();
        }
    }

    public EmailLinkRequestDialog(ContentPresenter contentPresenter, IEmailService emailService, IMySQLService mySQLService, Order order) : base(contentPresenter)
    {
        InitializeComponent();
        this.emailService = emailService;
        this.mySQLService = mySQLService;
        Order = order;
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    protected override async void OnButtonClick(ContentDialogButton button)
    {
        //Order.DownloadURL = Link;

        //if (await emailService.SendEmail(Order))
        //{
        //    await mySQLService.UpdateHistoryOrder(order);
        //}
        //else
        //{

        //}

        base.OnButtonClick(button);
    }
}
