#if PLATFORM
using CommunityToolkit.Maui.Core;
using Prism.Common;
using Prism.Navigation.Xaml;
using Prism.Services.Dialogs;
using Font = Microsoft.Maui.Font;
using SB = CommunityToolkit.Maui.Alerts.Snackbar;

namespace Shiny.Impl;


public class NativeDialogs : IDialogs
{
    readonly IApplication application;
    readonly IPlatform platform;


    public NativeDialogs(IApplication application, IPlatform platform)
    {
        this.application = application;
        this.platform = platform;
    }


    public Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options)
        => this.Run(dialogs => dialogs.DisplayActionSheetAsync(
            title,
            acceptText,
            dismissText,
            options
        ));

    public Task Alert(string message, string? title = null, string? dismissText = null)
        => this.Run(async dialogs =>
        {
            await dialogs.DisplayAlertAsync(title, message, dismissText ?? "OK");
            return true;
        });

    public Task<bool> Confirm(string message, string? title = null, string? okText = null, string? cancelText = null)
        => this.Run(dialogs => dialogs.DisplayAlertAsync(title, message, okText, cancelText));

    public Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null)
        => this.Run(dialogs => dialogs.DisplayPromptAsync(title, question, acceptText, dismissText, placeholder, maxLength ?? -1));

    public async Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null)
    {
        var clicked = false;        
        var snackbar = SB.Make(
            message,
            () => clicked = true,
            actionText ?? "OK",
            TimeSpan.FromMilliseconds(durationMillis),
            SnackbarOptions
        );
        await snackbar.Show();
        return clicked;
    }

    async Task<T> Run<T>(Func<IPageDialogService, Task<T>> func)
    {
        var window = this.application.Windows.OfType<Window>().First();
        var currentPage = MvvmHelpers.GetCurrentPage(window.Page);
        var container = currentPage.GetContainerProvider();

        var dialogs = container.Resolve<IPageDialogService>();
        var result = await this.platform.InvokeTaskOnMainThread(() => func(dialogs));
        return result;
    }


    public static SnackbarOptions SnackbarOptions = new()
    {
        BackgroundColor = Colors.Black,
        TextColor = Colors.White,
        ActionButtonTextColor = Colors.Yellow,
        CornerRadius = new CornerRadius(10),
        Font = Font.SystemFontOfSize(14),
        ActionButtonFont = Font.SystemFontOfSize(14)
    };
}
#endif