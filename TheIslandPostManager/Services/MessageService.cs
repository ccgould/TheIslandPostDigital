using Wpf.Ui.Controls;
using Wpf.Ui;
using Serilog;

namespace TheIslandPostManager.Services;

public class MessageService : IMessageService
{
    private ISnackbarService snackbarService;

    public MessageService(ISnackbarService snackbarService)
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.File("Data/Logs/myapp.txt", rollingInterval: RollingInterval.Month)
           .CreateLogger();
        this.snackbarService = snackbarService;
    }

    public async Task<MessageBoxResult> ShowMessage(string title, string message, string closeButtonText = "OK",
        ControlAppearance closeButtonAppearance = ControlAppearance.Primary,
        bool isPrimaryVisible = true, string primaryButtonText = "YES",
        bool isSecondaryVisible = false, string secondaryButtonText = "NO")
    {
        var uiMessageBox = new MessageBox
        {
            Title = title,
            Content = message,
            CloseButtonText = closeButtonText,
            CloseButtonAppearance = closeButtonAppearance,
            IsPrimaryButtonEnabled = isPrimaryVisible,
            PrimaryButtonText = primaryButtonText,
            IsSecondaryButtonEnabled = isSecondaryVisible,
            SecondaryButtonText = secondaryButtonText
        };

        Log.Information(message);

        return await uiMessageBox.ShowDialogAsync();
    }

    public async Task ShowErrorMessage(string title, string message,bool verbose = true)
    {
        if (verbose)
        {
            var uiMessageBox = new MessageBox
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",

            };

            await uiMessageBox.ShowDialogAsync();
        }

        Log.Error(message);

    }

    public async Task ShowDebugMessage(string title, string message)
    {
        var uiMessageBox = new MessageBox
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",

        };
        Log.Debug(message);

        await uiMessageBox.ShowDialogAsync();
    }

    public async Task ShowErrorMessage(string title, string message, string stackTrace, string errorID, bool verbose = true)
    {
        if (verbose)
        {
            var uiMessageBox = new MessageBox
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
            };

            await uiMessageBox.ShowDialogAsync();
        }

        Log.Error(message + " ErrorID: " + errorID);
    }
    /// <summary>
    /// Displays a message at the bottom of the application for a set amount of seconds
    /// </summary>
    /// <param name="title">Title of the message</param>
    /// <param name="message">Content to be shown in the snack bar</param>
    /// <param name="seconds">How long in seconds does the message show.</param>
    /// <param name="controlAppearance">The appearance of the screen (Changes the color of the snackbar)</param>
    /// <param name="symbol">The icon to show on the snackbar</param>
    public void ShowSnackBarMessage(string title, string message, ControlAppearance controlAppearance = ControlAppearance.Success, SymbolRegular symbol = SymbolRegular.ThumbLike28, int seconds = 5)
    {
        snackbarService.Show(title, message, controlAppearance, new SymbolIcon(symbol), TimeSpan.FromSeconds(seconds));

        if (controlAppearance == ControlAppearance.Danger)
        {
            Log.Error(message);
        }
        else
        {
            Log.Information(message);
        }
    }
}