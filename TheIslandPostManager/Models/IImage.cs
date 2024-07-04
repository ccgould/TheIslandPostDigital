using System.Windows.Media.Imaging;

namespace TheIslandPostManager.Models;

public interface IImage
{
    string ImageUrl { get; set; }
    string Name { get; set; }
    BitmapImage LowImage { get; set; }
    BitmapImage HDImage { get; set; }
    int PrintAmount { get; set; }
    bool IsSelected { get; set; }
    bool IsPending { get; set; }
    bool IsPrintable { get; set; }
    int Index { get; set; }
}