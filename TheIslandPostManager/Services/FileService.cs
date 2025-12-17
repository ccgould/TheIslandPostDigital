using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using TheIslandPostManager.Helpers;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;

public class FileService : IFileService
{
    private readonly IMessageService messageService;
    private AppSettings? settings => App.AppConfig.GetSection("AppSettings") as AppSettings;

    public FileService(IMessageService messageService)
    {
        this.messageService = messageService;
        DeleteBackups();
    }

    public void DeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex, "Failed to delete file", "0190550a-78cc-7a04-b6d4-8fad0c9ba663");
        }
    }

    private void ShowErrorMessage(Exception ex, string friendlyMessage, string error)
    {
        messageService.ShowErrorMessage("Error", friendlyMessage);
        messageService.ShowErrorMessage("Error", ex.Message, ex.StackTrace, error, false);
    }

    public void OpenLocation(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex, $"Failed to open path = {path}", "00190550a-78cc-771e-a78b-b324ee2ce67d");
        }
    }

    public BitmapImage LoadImageFile(string path, bool fullQuality = false)
    {
        try
        {
            var _orientationQuery = "System.Photo.Orientation";

            if (File.Exists(path))
            {
                Rotation rotation = Rotation.Rotate0;

                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    BitmapFrame bitmapFrame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                    BitmapMetadata bitmapMetadata = bitmapFrame.Metadata as BitmapMetadata;

                    if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery(_orientationQuery)))
                    {
                        object o = bitmapMetadata.GetQuery(_orientationQuery);

                        if (o != null)
                        {
                            switch ((ushort)o)
                            {
                                case 6:
                                    {
                                        rotation = Rotation.Rotate90;
                                    }
                                    break;
                                case 3:
                                    {
                                        rotation = Rotation.Rotate180;
                                    }
                                    break;
                                case 8:
                                    {
                                        rotation = Rotation.Rotate270;
                                    }
                                    break;
                            }
                        }
                    }

                    BitmapImage _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(path);
                    _image.Rotation = rotation;

                    if (!fullQuality)
                    {
                        _image.DecodePixelWidth = 200;
                    }

                    _image.StreamSource = fileStream;
                    _image.CacheOption = BitmapCacheOption.OnLoad;
                    _image.EndInit();
                    _image.Freeze();

                    return _image;
                }
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex, $"Failed to load ImageObj = {path}", "0190550a-78cc-71f9-8434-61e0d91adc17");
        }

        return null;
    }

    public bool CreateDirectory(string directory)
    {
        try
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("Error Occured!", ex.Message, ex.StackTrace, "");
            return false;
        }

        return true;
    }

    public async Task Copy(List<Tuple<string, string>> file,bool moveToPrinterDir = true,Action<string> callBack = null)
    {
        await Copier.CopyFiles(file, (prog, fileName) =>
        {
            callBack?.Invoke(fileName);
            if(moveToPrinterDir)
            {
                var fileNameWEx = Path.GetFileName(fileName);
                var result = Path.Combine(settings.PrinterDirectory, fileNameWEx);
                File.Move(fileName, result, true);
            }
        });
    }

    public async Task MoveBulk(List<Tuple<string, string>> files)
    {
        try
        {
            foreach (var file in files)
            {
                using (FileStream sourceStream = File.Open(file.Item1, FileMode.Open))
                {
                    using (FileStream destinationStream = File.Create(file.Item2))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                        sourceStream.Close();
                        File.Delete(file.Item1);
                    }
                }
            }
        }
        catch (IOException ioex)
        {
           await messageService.ShowErrorMessage("Error", $"An IOException occured during move, {ioex.Message}", ioex.StackTrace,"");
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error", $"An Exception occured during move, {ex.Message}",ex.StackTrace,"" );
        }
    }

    public async Task Move(Tuple<string, string> files)
    {
        try
        {
            using (FileStream sourceStream = File.Open(files.Item1, FileMode.Open))
            {
                using (FileStream destinationStream = File.Create(files.Item2))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                    sourceStream.Close();
                    File.Delete(files.Item1);
                }
            }
        }
        catch (IOException ioex)
        {
            await messageService.ShowErrorMessage("Error", $"An IOException occured during move, {ioex.Message}", ioex.StackTrace, "");
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error", $"An Exception occured during move, {ex.Message}", ex.StackTrace, "");
        }
    }

    public void DeleteDirectory(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath,true);
            }
        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("Error Occured!", ex.Message, ex.StackTrace, "65a519ae-d7d8-45d2-aece-3ffd0251d873");
        }
    }

    public void PurgeAll()
    {
        try
        {
            if (Directory.Exists(settings.InputDirectory))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(settings.InputDirectory);


                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("Error Occured!", ex.Message, ex.StackTrace, "c02511ad-1db1-4bbb-9057-d70c4fc36917");
        }
    }

    public void Purge(Order order)
    {
        try
        {
            if(Directory.Exists(settings.InputDirectory))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(settings.InputDirectory);


                foreach (FileInfo file in di.GetFiles())
                {
                    if(order.CurrentImages.Any(x=>x.ImageUrl == file.FullName))
                    {
                        file.Delete();
                    }
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("Error Occured!", ex.Message, ex.StackTrace, "c02511ad-1db1-4bbb-9057-d70c4fc36917");
        }
    }

    public void Copy(string imageUrl, string newFile)
    {
        try
        {
            File.Copy(imageUrl, newFile);

        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("Error Occured!", ex.Message, ex.StackTrace, "54e626ff-d252-4b45-88e7-7fc0d532c1dc");

        }
    }

    public void DeleteBackups()
    {
        DateTime beginDate = DateTime.Now;
        var daysLeft = DateTime.DaysInMonth(beginDate.Year, beginDate.Month) - beginDate.Day;

        if (daysLeft == 0)
        {
            CleanInputDirectory();
            CleanYearDirectory();
            //CleanDirectory();
        }
    }

    public void CleanupAdditionalDirectories()
    {
        try
        {
            var purges = settings.AdditionalDirectories?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (purges is not null)
            {
                foreach (var item in purges)
                {
                    DirectoryInfo d = new DirectoryInfo(item);

                    FileInfo[] Files = d.GetFiles("*.*");

                    foreach (FileInfo file in Files)
                    {
                        File.Delete(file.FullName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            messageService.ShowErrorMessage("Error", ex.Message, ex.StackTrace, "81d3e1db-5556-45a9-af60-46b80acaf025", true);
        }
    }

    public void CleanInputDirectory()
    {
        DirectoryInfo d = new DirectoryInfo(settings.InputDirectory);

        FileInfo[] Files = d.GetFiles("*.*");

        string str = "";

        foreach (FileInfo file in Files)
        {
            File.Delete(file.FullName);
        }
    }

    private void CleanYearDirectory()
    {
        DirectoryInfo d = new DirectoryInfo(settings.BackupDirectory);

        FileInfo[] Files = d.GetFiles("*.*");

        string str = "";

        foreach (FileInfo file in Files)
        {
            File.Delete(file.FullName);
        }

        var directories = Directory.GetDirectories(settings.BackupDirectory);

        foreach (var dir in directories)
        {
            var name = Path.GetFileName(dir);
            
            if(Int32.TryParse(name,out var year))
            {
                if(year < DateTime.Now.Year)
                {
                    Directory.Delete(dir,true);
                }
            }
            else
            {
                Directory.Delete(dir,true);
            }
        }
    }

    private void CleanDirectory()
    {
        var directory = Path.Combine(settings.BackupDirectory, DateTime.Now.Year.ToString());
        DirectoryInfo d = new DirectoryInfo(directory);

        FileInfo[] Files = d.GetFiles("*.*");
        

        string str = "";

        foreach (FileInfo file in Files)
        {
            File.Delete(file.FullName);
        }

        foreach (var dir in d.GetDirectories())
        {
            CultureInfo enUS = new CultureInfo("en-US");

            if (DateTime.TryParseExact(dir.Name, "MMM", enUS , DateTimeStyles.None, out var dateValue))
            {
                if (dateValue.Month < DateTime.Now.Month)
                {
                    Directory.Delete(dir.FullName,true);
                }
            }
            else
            {
                Directory.Delete(dir.FullName, true);
            }
        }
    }
}
