using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls.Extensions.Forms;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class FocusTrackerToolWindowViewModel : ToolWindowViewModel
    {
        public class FocusEvent
        {
            public object Element { get; private set; }
            public string TimeStamp { get; private set; }

            public FocusEvent(object element)
            {
                Element = element;
                this.TimeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
            }
        }

        public ObservableCollection<FocusEvent> FocusEvents { get; private set; }

        private bool ignoreFocusChange;

        #region SelectedEvent
        private FocusEvent selectedEvent;

        public FocusEvent SelectedEvent
        {
            get { return this.selectedEvent; }
            set
            {
                if (this.SetProperty(ref this.selectedEvent, value))
                {
                    var dobvm = this.selectedEvent.Element as DependencyObjectViewModel;

                    if (dobvm != null)
                    {
                        var uiElement = dobvm.Model as UIElement;

                        if (uiElement != null)
                        {
                            ignoreFocusChange = true;
#pragma warning disable 4014
                            DebugConsoleViewModel.Instance.VisualTreeView.SelectItem(uiElement);
#pragma warning restore 4014
                            ignoreFocusChange = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region FocusTracker
        private FocusTracker _focusTracker;
        public FocusTracker FocusTracker
        {
            get
            {
                return _focusTracker;
            }
            set
            {
                if (_focusTracker != value)
                {
                    if (_focusTracker != null)
                    {
                        _focusTracker.FocusChanged -= this.OnFocusChanged;
                    }

                    _focusTracker = value;

                    if (_focusTracker != null)
                    {
                        _focusTracker.FocusChanged += this.OnFocusChanged;
                    }
                }

            }
        } 
        #endregion

        public FocusTrackerToolWindowViewModel()
        {
            this.FocusEvents = new ObservableCollection<FocusEvent>();
            //DebugConsoleViewModel.Instance.ToolWindows.Add(this);
            AddFocusEvent(FocusManager.GetFocusedElement() as UIElement);
        }

        private void OnFocusChanged(object sender, UIElement e)
        {
            if (!ignoreFocusChange)
            {
                AddFocusEvent(e);
            }
        }

        private async Task AddFocusEvent(UIElement uiElement)
        {
            await DebugConsoleViewModel.Instance.VisualTreeView.SelectItem(uiElement);
            var fe = new FocusEvent(DebugConsoleViewModel.Instance.VisualTreeView.SelectedItem);
            this.FocusEvents.Add(fe);
            this.SelectedEvent = fe;
        }

        internal void Remove()
        {
            if (_focusTracker != null)
            {
                _focusTracker.FocusChanged -= this.OnFocusChanged;
            }

            DebugConsoleViewModel.Instance.ToolWindows.Remove(this);
        }
    }
}