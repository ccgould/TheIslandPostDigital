using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using TheIslandPostManager.Models;
using TheIslandPostManager.Services;

namespace TheIslandPostManager.ViewModels;
public partial class BackupPageViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<DriveInfo> drives;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private decimal backupPercentage;
    [ObservableProperty] private string backupPercentageTxt;
    [ObservableProperty] private DriveInfo selectedDrive;
    [ObservableProperty] private string backupSizeText;
    [ObservableProperty] private string daysLeftText;
    private StringBuilder sb = new();
    [ObservableProperty] private string logText;
    private decimal _total;
    private decimal _currentProgress = 0;
    private readonly IFileService fileService;
    private readonly IMessageService messageService;

    private AppSettings? settings => App.AppConfig.GetSection("AppSettings") as AppSettings;

    public BackupPageViewModel(IFileService fileService,IMessageService messageService)
    {
        //BackupSizeText = new DirectoryInfo(settings.BackupDirectory).Attributes
        Drives = new ObservableCollection<DriveInfo>();

        DateTime beginDate = DateTime.Now;
        var daysLeft = DateTime.DaysInMonth(beginDate.Year, beginDate.Month) - beginDate.Day;
        var dat = daysLeft > 1 ? "days" : "day";
        DaysLeftText = $"{daysLeft} {dat}";

        GetMemoryCards();
        this.fileService = fileService;
        this.messageService = messageService;
    }

    [RelayCommand]
    private void GetMemoryCards()
    {
        Drives.Clear();
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady && (drive.DriveFormat.Equals("exFat",StringComparison.OrdinalIgnoreCase) || drive.DriveType == DriveType.Removable || drive.DriveFormat.Equals("FAT32", StringComparison.OrdinalIgnoreCase)))
            {
                if (drive.TotalSize > 0) {
                    Drives.Add(drive);
                    AppendNewLog($"Added drive {drive.Name} to list.");
                }

            }
        }
    }

    private void AppendNewLog(string text)
    {
        sb.Append($"{text}  -  {DateTime.Now.ToString("MMM dd,yyyy @ hh:mm:ss tt")}");
        sb.Append(Environment.NewLine);
        LogText = sb.ToString();
    }

    [RelayCommand]
    private async Task Backup()
    {
        try
        {
            IsBusy = true;

            _currentProgress = 0;
            BackupPercentageTxt = "0%";
            BackupPercentage = 0;

            if (SelectedDrive is not null)
            {
                var path = Path.Combine(SelectedDrive.RootDirectory.FullName, "DCIM");
                var jpegs = Directory.GetFiles(path, "*.JPG", SearchOption.AllDirectories);
                var raf = Directory.GetFiles(path, "*.RAF", SearchOption.AllDirectories);
                var arw = Directory.GetFiles(path, "*.ARW", SearchOption.AllDirectories);
                var cr2 = Directory.GetFiles(path, "*.CR2", SearchOption.AllDirectories);

                _total = jpegs.Count() + raf.Count() + arw.Count() + cr2.Count();

                /*
                 * Backup images
                 * Create Folder Structure BACKUPFOLDER/Year/Month/Month_Date_
                 */

                var bkPath = Path.Combine(settings.BackupDirectory, DateTime.Now.Year.ToString(), DateTime.Now.ToString("MMM"), DateTime.Now.ToString("MMM_dd"));
                var rawPath = Path.Combine(bkPath, "RAW");

                fileService.CreateDirectory(bkPath);
                fileService.CreateDirectory(rawPath);

                var rawlocation = new List<Tuple<string, string>>();
                var imglocation = new List<Tuple<string, string>>();

                foreach (var imagePth in jpegs)
                {
                    imglocation.Add(new(imagePth,Path.Combine(bkPath,Path.GetFileName(imagePth))));
                }

                foreach (var imagePth in raf)
                {
                    rawlocation.Add(new(imagePth, Path.Combine(rawPath, Path.GetFileName(imagePth))));
                }

                foreach (var imagePth in arw)
                {
                    rawlocation.Add(new(imagePth, Path.Combine(rawPath, Path.GetFileName(imagePth))));
                }

                foreach (var imagePth in cr2)
                {
                    rawlocation.Add(new(imagePth, Path.Combine(rawPath, Path.GetFileName(imagePth))));
                }


                await fileService.Copy(imglocation, false, (s) => 
                {
                    AppendNewLog($"Backed up File: {s}");
                    UpdatePercentage();
                });

                await fileService.Copy(rawlocation,false,(s)=>
                {
                    AppendNewLog($"Backed up File: {s}");
                    UpdatePercentage();
                });

            }
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error", ex.Message, ex.StackTrace, "", false);
            messageService.ShowSnackBarMessage("Error", "Failed to get images from drive.", Wpf.Ui.Controls.ControlAppearance.Danger, Wpf.Ui.Controls.SymbolRegular.ThumbDislike24);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void UpdatePercentage()
    {
        _currentProgress++;
        decimal percentage = (_currentProgress / _total) * 100;
        BackupPercentageTxt = percentage.ToString();
        BackupPercentage = percentage;
    }
}
