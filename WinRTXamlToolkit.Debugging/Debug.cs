using System.Diagnostics;
using DiagnosticsDebug = System.Diagnostics.Debug;

namespace WinRTXamlToolkit.Debugging
{
    public static class Debug
    {
        public static bool TraceToDebugger = true;
        public static bool TraceToDebugConsoleOverlay = true;

        [Conditional("DEBUG")]
        public static void WriteLine(string format, params object[] args)
        {
            if (TraceToDebugger)
                DiagnosticsDebug.WriteLine(format, args);

            if (TraceToDebugConsoleOverlay)
                DC.Trace(format, args);
        }

        [Conditional("DEBUG")]
        public static void WriteLine(string message)
        {
            if (TraceToDebugger)
                DiagnosticsDebug.WriteLine(message);
            if (TraceToDebugConsoleOverlay)
                DC.Trace(message);
        }

        [Conditional("DEBUG")]
        public static void WriteLine(object value)
        {
            if (TraceToDebugger)
                DiagnosticsDebug.WriteLine(value);
            if (TraceToDebugConsoleOverlay)
                DC.Trace((value ?? "<null>").ToString());
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            DiagnosticsDebug.Assert(condition);
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
            DiagnosticsDebug.Assert(condition, message);
        }
    }
}
