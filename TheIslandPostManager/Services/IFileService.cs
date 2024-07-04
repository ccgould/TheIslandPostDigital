using System.Windows.Media.Imaging;

namespace TheIslandPostManager.Services;
public interface IFileService
{
    void DeleteFile(string path);
    BitmapImage LoadImageFile(string path, bool fullQuality = false);
    void OpenLocation(string path);
}