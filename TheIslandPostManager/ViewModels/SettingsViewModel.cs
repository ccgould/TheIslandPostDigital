using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
    private StringBuilder sb;
    [ObservableProperty] private AppSettings? settings;
    [ObservableProperty] private ObservableCollection<string> purgeList;

    public SettingsViewModel()
    {
        sb = new StringBuilder();
        settings = App.AppConfig.GetSection("AppSettings") as AppSettings;
        var purges = settings.AdditionalDirectories?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        purgeList = new(purges);
    }

    [RelayCommand]
    private void DeletePurgeLocation(string value)
    {
        PurgeList.Remove(value);
        SavePurgeList();
    }

    private void SavePurgeList()
    {
        sb.Clear();

        for (int i = 0; i < PurgeList.Count; i++)
        {
            string? item = PurgeList[i];

            sb.Append(item);

            if (i < PurgeList.Count)
            {
                sb.Append(',');
            }

        }

        Settings.AdditionalDirectories = sb.ToString();

        App.AppConfig.Save();
    }

    [RelayCommand]
    private void AddNewPurge()
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        CommonFileDialogResult result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            if(!PurgeList.Contains(dialog.FileName))
            {
                PurgeList.Add(dialog.FileName);
            }

            SavePurgeList();
        }
    }
}
