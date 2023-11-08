using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading.Tasks;
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


    /// <summary>
    /// Execute a task wrapped in the warning timer logic
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="taskFunc"></param>
    /// <param name="getMsg"></param>
    /// <param name="slowTime"></param>
    /// <returns></returns>
    public static async Task LogWarningIfSlow(this ILogger logger, Func<Task> taskFunc, Func<TimeSpan, string> getMsg, TimeSpan? slowTime = null)
    {
        using (logger.LogWarningIfSlow(getMsg, slowTime))
            await taskFunc.Invoke().ConfigureAwait(false);
    }
}