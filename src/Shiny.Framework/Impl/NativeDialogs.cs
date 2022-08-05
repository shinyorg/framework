#if PLATFORM
namespace Shiny.Impl;


public class NativeDialogs : IDialogs
{
    readonly Lazy<IPageDialogService> prism;
    readonly IPlatform platform;


    public NativeDialogs(IServiceProvider services, IPlatform platform)
    {
        this.prism = new Lazy<IPageDialogService>(() => services.GetRequiredService<IPageDialogService>());
        this.platform = platform;
    }


    public Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options)
        => this.platform.InvokeTaskOnMainThread(() => this.prism.Value.DisplayActionSheetAsync(
            title,
            acceptText,
            dismissText,
            options
        ));

    public Task Alert(string message, string? title = null, string? dismissText = null)
        => this.platform.InvokeTaskOnMainThread(() => this.prism.Value.DisplayAlertAsync(title, message, dismissText ?? "OK"));

    public Task<bool> Confirm(string message, string? title = null, string? okText = null, string? cancelText = null)
        => this.platform.InvokeTaskOnMainThread(() => this.prism.Value.DisplayAlertAsync(title, message, okText, cancelText));

    public Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null)
        => this.platform.InvokeTaskOnMainThread(() => this.prism.Value.DisplayPromptAsync(title, question, acceptText, dismissText, placeholder, maxLength ?? -1));

    public Task<IAsyncDisposable> LoadingDialog(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
    public Task<IAsyncDisposable> LoadingSnackbar(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
    public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null) => Task.FromResult(false);
}
#endif