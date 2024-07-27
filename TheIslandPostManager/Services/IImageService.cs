using System.Drawing.Imaging;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;

public interface IImageService
{
    //Image CurrentImage { get; set; }
    //ObservableCollection<Image> CurrentImages { get; set; }

    void AddImage(Image image);
    Task DeleteAllImages();
    Task DeleteImage(Image image);
    void DeSelectAllImages();
    Task OpenImageDialogBrowser();
    void PrintAllImages();
    void SelectAllImages();
    void SetOrder(Order order);
    void UpdateOrder(Order obj);
}