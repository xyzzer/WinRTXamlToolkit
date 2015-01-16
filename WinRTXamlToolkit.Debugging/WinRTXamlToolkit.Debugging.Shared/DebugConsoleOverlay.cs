using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.Debugging
{
    /// <summary>
    /// Provides helpers for debugging WinRT XAML applications - for tracing and visual tree debugging.
    /// </summary>
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
                    _instance.Initialize();
                }

                return _instance;
            }
        }
        #endregion

        private DebugConsoleView _debugConsoleView;
        private Popup _popup;

        private void Initialize()
        {
            _debugConsoleView = new DebugConsoleView
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
            };

            _popup = new Popup
            {
                Child = _debugConsoleView,
            };

            var panel = Window.Current.Content.GetFirstDescendantOfType<Panel>();

            if (panel != null)
            {
                panel.Children.Add(_popup);
            }

            _popup.IsOpen = true;

            Window.Current.SizeChanged += this.OnWindowSizeChanged;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DebugConsoleOverlay"/> class from being created.
        /// </summary>
        private DebugConsoleOverlay()
        {
        }

        public static void Show()
        {
            Instance._popup.IsOpen = true;
        }

        public static void ShowLog()
        {
            Show();
            _instance._debugConsoleView.ShowLog();
        }

        public static void ShowVisualTree(UIElement element = null)
        {
            Show();
            _instance._debugConsoleView.ShowVisualTree(element);
        }

        public static void Hide()
        {
            Instance._popup.IsOpen = false;
        }

        public static void Expand()
        {
            Instance._debugConsoleView.Expand();
        }

        public static void Collapse()
        {
            Instance._debugConsoleView.Collapse();
        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            _debugConsoleView.Width = Window.Current.Bounds.Width;
            _debugConsoleView.Height = Window.Current.Bounds.Height;
        }

        public static void Trace(string format, params object[] args)
        {
            Instance.TraceInternal(format, args);
        }

        public static void Trace(string message)
        {
            Instance.TraceInternal(message);
        }

        public static void Clear()
        {
            Instance.ClearInternal();
        }

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
                _debugConsoleView.Append(line);
            }
            catch (FormatException)
            {
                var line =
                    string.Format(
                        "{0} - {1}\n",
                        DateTime.Now.ToString("HH:mm:ss"),
                        string.Format(
                            format.Replace("{", "{{").Replace("}", "}}"),
                            args));
                _debugConsoleView.Append(line);
            }
        }

        private void TraceInternal(string message)
        {
            var line =
                string.Format(
                    "{0} - {1}\n",
                    DateTime.Now.ToString("HH:mm:ss"),
                    message);
            _debugConsoleView.Append(line);
        }

        private void ClearInternal()
        {
            _debugConsoleView.Clear();
        }
    }

    /// <summary>
    /// Essentially an alias for DebugConsoleOverlay
    /// </summary>
    public static class DC
    {
        private static DateTime _previousTraceIntervalTimeStamp = DateTime.Now;

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

        public static void Trace(string format, params object[] args)
        {
            DebugConsoleOverlay.Trace(format, args);
        }

        public static void Trace(string message)
        {
            DebugConsoleOverlay.Trace(message);
        }

        public static void Trace(object value)
        {
            DebugConsoleOverlay.Trace((value ?? "<null>").ToString());
        }

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

        public static void Clear()
        {
            DebugConsoleOverlay.Clear();
        }

        public static void Hide()
        {
            DebugConsoleOverlay.Hide();
        }

        public static void Show()
        {
            DebugConsoleOverlay.Show();
        }

        public static void ShowLog()
        {
            DebugConsoleOverlay.ShowLog();
        }

        public static void ShowVisualTree(UIElement element = null)
        {
            DebugConsoleOverlay.ShowVisualTree(element);
        }

        public static void Expand()
        {
            DebugConsoleOverlay.Expand();
        }

        public static void Collapse()
        {
            DebugConsoleOverlay.Collapse();
        }
    }
}
