using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace omni_multitool.Helpers
{
    public static class SingleInstanceGuard
    {
        private const string MutexName = @"Global\omni_multitool_single_instance_mutex";
        private const string EventName = @"Global\omni_multitool_show_window_event";

        private static Mutex? _mutex;
        private static EventWaitHandle? _eventWaitHandle;
        private static CancellationTokenSource? _cts;

        public static bool CheckAndRegister()
        {
            try
            {
                _mutex = new Mutex(true, MutexName, out bool createdNew);

                if (!createdNew)
                {
                    // Instance already running. Signal existing instance to restore window and return false.
                    try
                    {
                        using var eventWaitHandle = EventWaitHandle.OpenExisting(EventName);
                        eventWaitHandle.Set();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to signal existing instance: {ex.Message}");
                    }

                    return false;
                }

                // Primary instance: create event wait handle and start listening for restore signals.
                _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EventName);
                _cts = new CancellationTokenSource();

                Task.Run(() => ListenForRestoreSignal(_cts.Token));

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SingleInstanceGuard error: {ex.Message}");
                return true; // Fallback to allowing execution if mutex fails
            }
        }

        private static void ListenForRestoreSignal(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _eventWaitHandle != null)
            {
                try
                {
                    if (_eventWaitHandle.WaitOne(1000))
                    {
                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            if (Application.Current is App app)
                            {
                                app.RestoreMainWindow();
                            }
                        });
                    }
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error waiting for restore signal: {ex.Message}");
                }
            }
        }

        public static void Cleanup()
        {
            try
            {
                _cts?.Cancel();
                _eventWaitHandle?.Dispose();
                _mutex?.ReleaseMutex();
                _mutex?.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cleaning up SingleInstanceGuard: {ex.Message}");
            }
        }
    }
}
