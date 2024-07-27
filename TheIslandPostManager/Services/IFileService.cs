using System.Windows.Media.Imaging;

namespace TheIslandPostManager.Services;
public interface IFileService
{
    void DeleteFile(string path);
    BitmapImage LoadImageFile(string path, bool fullQuality = false);
    void OpenLocation(string path);
    bool CreateDirectory(string directory);
    Task Copy(List<Tuple<string, string>> file);
}