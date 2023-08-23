using System;
using Microsoft.Extensions.DependencyInjection;
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