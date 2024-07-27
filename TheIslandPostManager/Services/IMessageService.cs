using Wpf.Ui.Controls;

namespace TheIslandPostManager.Services;
public interface IMessageService
{
    Task ShowDebugMessage(string title, string message);
    Task ShowErrorMessage(string title, string message, bool verbose = true);
    Task ShowErrorMessage(string title, string message, string stackTrace,string errorID, bool verbose = true);
    Task<MessageBoxResult> ShowMessage(string title, string message, string closeButtonText = "CLOSE",
        ControlAppearance closeButtonAppearance = ControlAppearance.Primary,
        bool isPrimaryVisible = true, string primaryButtonText = "YES",
        bool isSecondaryVisible = false, string secondaryButtonText = "NO");
    void ShowSnackBarMessage(string title, string message, ControlAppearance controlAppearance = ControlAppearance.Success, SymbolRegular symbol = SymbolRegular.ThumbLike28, int seconds = 5);
}