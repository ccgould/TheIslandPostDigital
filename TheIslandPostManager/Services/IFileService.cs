using System.Windows.Media.Imaging;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
public interface IFileService
{
    void DeleteFile(string path);
    BitmapImage LoadImageFile(string path, bool fullQuality = false);
    void OpenLocation(string path);
    bool CreateDirectory(string directory);
    Task Copy(List<Tuple<string, string>> file,bool moveToPrinterDir = true, Action<string> callBack = null);
    Task MoveBulk(List<Tuple<string, string>> files);
    Task Move(Tuple<string, string> files);
    void DeleteDirectory(string downloadURL);
    void Purge(Order order);
    void Copy(string imageUrl, string newFile);
    void PurgeAll();
    void DeleteBackups();
    void CleanInputDirectory();
}