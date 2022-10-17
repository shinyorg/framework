using Microsoft.Extensions.Logging;
using ReactiveUI;
using Shiny.Extensions.Localization;
using Shiny.Hosting;

namespace Shiny.Impl;


public class GlobalExceptionHandler : IObserver<Exception>
{
    public void OnCompleted() { }
    public void OnError(Exception error) { }
    public async void OnNext(Exception value) => await Host
        .Current
        .Services
        .GetRequiredService<GlobalExceptionAction>()
        .Process(value);
}