using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;

namespace Shiny;


public static class LoggingExtensions
{
    /// <summary>
    /// Logs a warning if enabled and method scope time >= passed slowTime
    /// </summary>
    /// <param name="logger">This logger to use</param>
    /// <param name="getMsg">Build the message with the time taken</param>
    /// <param name="slowTime">Defaults to 5 seconds</param>
    /// <returns></returns>
    public static IDisposable LogWarningIfSlow(this ILogger logger, Func<TimeSpan, string> getMsg, TimeSpan? slowTime = null)
    {
        if (!logger.IsEnabled(LogLevel.Warning))
            return Disposable.Empty;

        slowTime ??= TimeSpan.FromSeconds(5);
        var sw = new Stopwatch();
        sw.Start();

        return Disposable.Create(() =>
        {
            sw.Stop();
            if (sw.Elapsed >= slowTime)
            {
                var msg = getMsg.Invoke(sw.Elapsed);
                logger.LogWarning(msg);
            }
        });
    }
}