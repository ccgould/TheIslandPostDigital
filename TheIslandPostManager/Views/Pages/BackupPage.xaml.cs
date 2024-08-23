using System.Text;
using TheIslandPostManager.ViewModels;
using Wpf.Ui.Controls;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for BackupPage.xaml
/// </summary>
public partial class BackupPage : INavigableView<BackupPageViewModel>
{
    public BackupPage(BackupPageViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;

        //StringBuilder sb = new StringBuilder();

        //for (int i = 0; i < 10; i++)
        //{
        //    sb.AppendLine($"Added file img_{i.ToString("D3")} to location c:/test/folder/img_0001.jpg");
        //    sb.AppendLine(Environment.NewLine);
        //}


        //Log.Text = sb.ToString();
    }

    public BackupPageViewModel ViewModel { get; }
}
