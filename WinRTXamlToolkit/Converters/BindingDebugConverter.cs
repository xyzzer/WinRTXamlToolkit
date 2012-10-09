using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
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
