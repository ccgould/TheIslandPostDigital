using Microsoft.WindowsAPICodePack.Dialogs;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage : Page
{
    private readonly IMySQLService mySQLService;


    public SettingsPage(IMySQLService mySQLService)
    {
        InitializeComponent();
        DataContext = App.AppConfig.GetSection("AppSettings");
        this.mySQLService = mySQLService;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
        f.Password = passwordBx.Password;
        f.MysqlPassword = mysqlPasswordBx.Password;

        App.AppConfig.Save(ConfigurationSaveMode.Modified);

        mySQLService.SetConnectionString();

        ConfigurationManager.RefreshSection("AppSettings");
    }

    public static void AddOrUpdateAppSettings(string key, string value)
    {
        try
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
        catch (ConfigurationErrorsException)
        {
            Console.WriteLine("Error writing app settings");
        }
    }

    private void outputBrowseBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            outputDirTxtB.Text = dialog.FileName;

            var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
            f.OutputDirectory = dialog.FileName;
            App.AppConfig.Save();
        }
    }

    private void printerDirTxtB_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            printerDirTxtB.Text = dialog.FileName;

            var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
            f.PrinterDirectory = dialog.FileName;
            App.AppConfig.Save();
        }
    }

    private void watermarkBrowseBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();

        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            watermarkDirTxtB.Text = dialog.FileName;

            var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
            f.WatermarkDirectory = dialog.FileName;
            App.AppConfig.Save();
        }
    }

    private void pendingBrowseBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;

        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            pendingDirTxtB.Text = dialog.FileName;

            var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
            f.PendingDirectory = dialog.FileName;
            App.AppConfig.Save();
        }
    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {

    }

    private void backupBrowseBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;

        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            backupDirTxtB.Text = dialog.FileName;

            var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
            f.BackupDirectory = dialog.FileName;
            App.AppConfig.Save();
        }
    }
}
