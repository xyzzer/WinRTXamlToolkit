using System;
using WinRTXamlToolkit.Sample.Commands;
using WinRTXamlToolkit.Sample.Views;

namespace WinRTXamlToolkit.Sample.ViewModels
{
    public class SampleButtonViewModel : ButtonViewModel
    {
        public SampleTypes SampleType { get; private set; }
        public Type ViewType { get; private set; }

        public SampleButtonViewModel(string caption, SampleTypes sampleType, Type viewType)
            : base(new RelayCommand(async () => await AppShell.Frame.NavigateAsync(typeof(TestPage), caption)), null, caption)
        {
            this.SampleType = sampleType;
            this.ViewType = viewType;
        }

        public SampleButtonViewModel(string caption, Type viewType, SampleTypes sampleType)
            : base(new RelayCommand(async () => await AppShell.Frame.NavigateAsync(typeof(TestPage), caption)), null, caption)
        {
            this.SampleType = sampleType;
            this.ViewType = viewType;
        }
    }
}