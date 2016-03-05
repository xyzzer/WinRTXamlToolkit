using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Debugging.Converters
{
    public class BindingDebugConverter : IValueConverter
    {
        public bool BreaksAlways { get; set; }
        public bool Breaks { get; set; }
        public bool Traces { get; set; }
        public bool SavesTrace { get; set; }
        public ObservableCollection<string> TraceLines { get; set; }

        public BindingDebugConverter()
        {
            this.BreaksAlways = false;
            this.Breaks = true;
            this.SavesTrace = false;
            this.Traces = true;
            this.TraceLines = new ObservableCollection<string>();
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (this.Traces ||
                this.SavesTrace)
            {
                this.WriteLine(
                    "BindingDebugConverter.Convert(value:{0}, targetType:{1}, parameter:{2})",
                    value == null ? "<null>" : value.ToString(),
                    targetType,
                    parameter == null ? "<null>" : parameter.ToString());
            }

            if (this.Breaks && Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else if (this.BreaksAlways &&
                !DesignMode.DesignModeEnabled)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                else
                    Debugger.Launch();
            }

            return value;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in <c>TwoWay</c> bindings. 
        /// </summary>
        /// <param name="value">The target data being passed to the source..</param>
        /// <param name="targetType">The type of the target property, specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (this.Traces ||
                this.SavesTrace)
            {
                this.WriteLine(
                    "BindingDebugConverter.ConvertBack(value:{0}, targetType:{1}, parameter:{2})",
                    value == null ? "<null>" : value.ToString(),
                    targetType,
                    parameter == null ? "<null>" : parameter.ToString());
            }

            if (this.Breaks && Debugger.IsAttached)
            {
                Debugger.Break();
            } else if (this.BreaksAlways &&
                !DesignMode.DesignModeEnabled)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                else
                    Debugger.Launch();
            }

            return value;
        }

        private void WriteLine(string format, params object[] args)
        {
            if (!this.Traces &&
                !this.SavesTrace)
            {
                return;
            }

            var line = string.Format(format, args);

            if (this.Traces)
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

            if (this.SavesTrace)
            {
                this.TraceLines.Add(line);
            }
        }
    }
}
