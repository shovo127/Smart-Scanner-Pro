namespace SmartScannerPro.Scanner.WIA.Helpers;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides utility methods to execute synchronous delegates on a Single-Threaded Apartment (STA) thread.
/// This is required for interacting with Windows Image Acquisition (WIA) COM interfaces.
/// </summary>
public static class StaThread
{
    /// <summary>
    /// Runs a synchronous function that returns a value on a new STA thread and returns a task that completes when the thread finishes.
    /// </summary>
    /// <typeparam name="T">The type of the result returned by the function.</typeparam>
    /// <param name="func">The synchronous function to execute.</param>
    /// <returns>A task representing the asynchronous execution of the function.</returns>
    public static Task<T> RunAsync<T>(Func<T> func)
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        var tcs = new TaskCompletionSource<T>();
        var thread = new Thread(() =>
        {
            try
            {
                var result = func();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }

    /// <summary>
    /// Runs a synchronous action on a new STA thread and returns a task that completes when the thread finishes.
    /// </summary>
    /// <param name="action">The synchronous action to execute.</param>
    /// <returns>A task representing the asynchronous execution of the action.</returns>
    public static Task RunAsync(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var tcs = new TaskCompletionSource();
        var thread = new Thread(() =>
        {
            try
            {
                action();
                tcs.SetResult();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }
}
