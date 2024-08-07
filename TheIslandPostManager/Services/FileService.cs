using System.Diagnostics;
using System.IO;
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

    public async Task Copy(List<Tuple<string, string>> file)
    {
        await Copier.CopyFiles(file, (prog, fileName) =>
        {
            var fileNameWEx = Path.GetFileName(fileName);
            var result = Path.Combine(settings.PrinterDirectory, fileNameWEx);
            File.Move(fileName, result, true);
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
            messageService.ShowErrorMessage("Error Occured!", ex.Message, ex.StackTrace, "");
        }
    }
}
