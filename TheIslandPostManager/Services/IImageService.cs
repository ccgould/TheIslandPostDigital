using System.Drawing.Imaging;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;

public interface IImageService
{
    //Image CurrentImage { get; set; }
    //ObservableCollection<Image> CurrentImages { get; set; }

    void AddImage(ImageObj image);
    Task DeleteAllImages();
    Task<bool> DeleteImage(ImageObj image);
    void DeSelectAllImages();
    Task OpenImageDialogBrowser();
    void PrintAllImages();
    void SelectAllImages();
    void SetOrder(Order order);
    void UpdateOrder(Order obj);
}