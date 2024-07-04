
using System.Collections.ObjectModel;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;

public interface IImageService
{
    Image CurrentImage { get; set; }
    ObservableCollection<Image> CurrentImages { get; set; }

    void AddImage(Image image);
    Task DeleteAllImages();
    void DeleteImage(Image image);
    void DeSelectAllImages();
    Task OpenImageDialogBrowser();
    void PrintAllImages();
    void SelectAllImages();
    void SetOrder(Order order);
    void UpdateOrder(Order obj);
}