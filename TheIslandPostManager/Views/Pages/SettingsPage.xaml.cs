using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Views.Pages;
/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
        DataContext = App.AppConfig.GetSection("AppSettings");
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        App.AppConfig.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("AppSettings");
    }

    //private void inputBrowseBtn_Click(object sender, RoutedEventArgs e)
    //{
    //    var dialog = new CommonOpenFileDialog();
    //    dialog.IsFolderPicker = true;
    //    CommonFileDialogResult result = dialog.ShowDialog();

    //    if (result == CommonFileDialogResult.Ok)
    //    {
    //        inputDirTxtB.Text = dialog.FileName;

    //        var f = App.AppConfig.GetSection("AppSettings") as AppSettings;
    //        f.InputDirectory = dialog.FileName;
    //        App.AppConfig.Save();



    //       // AddOrUpdateAppSettings("inputDirectory", dialog.FileName);
    //    }

    //}


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
}
