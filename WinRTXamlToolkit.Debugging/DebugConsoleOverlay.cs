using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.Debugging
{
    public class DebugConsoleOverlay
    {
        #region Instance
        private static DebugConsoleOverlay _instance;
        private static DebugConsoleOverlay Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DebugConsoleOverlay();
                    _instance.Show();
                }

                return _instance;
            }
        }
        #endregion

        private DebugConsole _debugConsole;
        private Popup _popup;

        [Conditional("DEBUG")]
        private void Show()
        {
            _debugConsole = new DebugConsole
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
            };

            _popup = new Popup
            {
                Child = _debugConsole,
                IsOpen = true
            };

            Window.Current.SizeChanged += OnWindowSizeChanged;
        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            _debugConsole.Width = Window.Current.Bounds.Width;
            _debugConsole.Height = Window.Current.Bounds.Height;
        }

        [Conditional("DEBUG")]
        public static void Trace(string format, params object[] args)
        {
            Instance.TraceInternal(format, args);
        }

        [Conditional("DEBUG")]
        public static void Trace(string message)
        {
            Instance.TraceInternal(message);
        }

        [Conditional("DEBUG")]
        public static void Clear()
        {
            Instance.ClearInternal();
        }

        [Conditional("DEBUG")]
        private void TraceInternal(string format, object[] args)
        {
            try
            {
                var line =
                    string.Format(
                        "{0} - {1}\n",
                        DateTime.Now.ToString("HH:mm:ss"),
                        string.Format(
                            format,
                            args));
                _debugConsole.Append(line);
            }
            catch(FormatException)
            {
                var line =
                    string.Format(
                        "{0} - {1}\n",
                        DateTime.Now.ToString("HH:mm:ss"),
                        string.Format(
                            format.Replace("{", "{{").Replace("}", "}}"),
                            args));
                _debugConsole.Append(line);
            }
        }

        [Conditional("DEBUG")]
        private void TraceInternal(string message)
        {
            var line =
                string.Format(
                    "{0} - {1}\n",
                    DateTime.Now.ToString("HH:mm:ss"),
                    message);
            _debugConsole.Append(line);
        }

        [Conditional("DEBUG")]
        private void ClearInternal()
        {
            _debugConsole.Clear();
        }
    }

    public static class DC
    {
        [Conditional("DEBUG")]
        public static void TraceLocalized(
            string message = "Checkpoint", 
            [CallerMemberName] string methodName = "", 
            [CallerFilePath] string filePath = "", 
            [CallerLineNumber] int lineNumber = -1)
        {
            DebugConsoleOverlay.Trace(
                string.Format(
                    "File: {0},\tMethod: {1},\tLine: {2} - {3}",
                    filePath,
                    methodName,
                    lineNumber,
                    message));
        }

        [Conditional("DEBUG")]
        public static void TraceLocalized(
            object value,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            DebugConsoleOverlay.Trace(
                string.Format(
                    "File: {0},\tMethod: {1},\tLine: {2} - {3}",
                    filePath,
                    methodName,
                    lineNumber,
                    (value ?? "<null>").ToString()));
        }

        [Conditional("DEBUG")]
        public static void TraceMemoryCheckPoint(
            string message,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            GC.Collect();
            string memoryUsage = System.GC.GetTotalMemory(true).GetSizeString();
            DebugConsoleOverlay.Trace(
                string.Format(
                    "{4} - Memory: {3} at File: {0},\tMethod: {1},\tLine: {2}",
                    filePath,
                    methodName,
                    lineNumber,
                    memoryUsage,
                    message));
        }

        [Conditional("DEBUG")]
        public static void Trace(string format, params object[] args)
        {
            DebugConsoleOverlay.Trace(format, args);
        }

        [Conditional("DEBUG")]
        public static void Trace(string message)
        {
            DebugConsoleOverlay.Trace(message);
        }

        [Conditional("DEBUG")]
        public static void Trace(object value)
        {
            DebugConsoleOverlay.Trace((value ?? "<null>").ToString());
        }

        private static DateTime _previousTraceIntervalTimeStamp = DateTime.Now;

        [Conditional("DEBUG")]
        public static void TraceInterval(string message = null)
        {
            var now = DateTime.Now;
            var delay = now - _previousTraceIntervalTimeStamp;
            _previousTraceIntervalTimeStamp = now;

            if (message == null)
            {
                Trace(
                    string.Format(
                        "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                        delay.Hours,
                        delay.Minutes,
                        delay.Seconds,
                        delay.Milliseconds));
            }
            else
            {
                Trace(
                    "{0} - {1}",
                    string.Format(
                        "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                        delay.Hours,
                        delay.Minutes,
                        delay.Seconds,
                        delay.Milliseconds),
                    message);
            }
        }

        [Conditional("DEBUG")]
        public static void Clear()
        {
            DebugConsoleOverlay.Clear();
        }
    }
}
