using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Controls.Extensions.Forms;
using WinRTXamlToolkit.Debugging.Views;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class FocusTrackerToolWindowViewModel : ToolWindowViewModel
    {
        #region class FocusEvent
        public class FocusEvent : BindableBase
        {
            public object Element { get; }
            public string TimeStamp { get; private set; }
            #region FocusLossPossibleReason
            private string focusLossPossibleReason;
            /// <summary>
            /// Gets or sets a value indicating possible reason for loss of focus.
            /// </summary>
            public string FocusLossPossibleReason
            {
                get { return this.focusLossPossibleReason; }
                set { this.SetProperty(ref this.focusLossPossibleReason, value); }
            }
            #endregion

            public FocusEvent(object element)
            {
                this.Element = element ?? new { FontWeight = FontWeights.Normal, DisplayName = "<null>" };
                this.TimeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
                this.FocusLossPossibleReason = "Focused";
            }
        }
        #endregion

        private bool _ignoreFocusChange;
        public ObservableCollection<FocusEvent> FocusEvents { get; }

        #region SelectedEvent
        private FocusEvent _selectedEvent;

        public FocusEvent SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (this.SetProperty(ref _selectedEvent, value))
                {
                    var dobvm = _selectedEvent.Element as DependencyObjectViewModel;

                    var uiElement = dobvm?.Model as UIElement;

                    if (uiElement != null)
                    {
                        _ignoreFocusChange = true;
#pragma warning disable 4014
                        DebugConsoleViewModel.Instance.VisualTreeView.SelectItemAsync(uiElement);
#pragma warning restore 4014
                        _ignoreFocusChange = false;
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

        #region FocusTrackerToolWindowViewModel()
        public FocusTrackerToolWindowViewModel()
        {
            this.FocusEvents = new ObservableCollection<FocusEvent>();
            //DebugConsoleViewModel.Instance.ToolWindows.Add(this);
#pragma warning disable 4014
            this.AddFocusEventAsync(FocusManager.GetFocusedElement() as UIElement);
#pragma warning restore 4014
        } 
        #endregion

        #region OnFocusChanged()
        private async void OnFocusChanged(object sender, UIElement e)
        {
            if (!_ignoreFocusChange)
            {
                if (e == null)
                {
                    await this.AddFocusEventAsync(null);
                }
                else if (
                    !(e is DebugConsoleView) &&
                    !(e.GetAncestorsOfType<DebugConsoleView>().Any()))
                {
                    await this.AddFocusEventAsync(e);
                }
            }
        } 
        #endregion

        #region AddFocusEventAsync()
        private async Task AddFocusEventAsync(UIElement uiElement)
        {
            this.AnalyzePossibleLossOfFocusReason();

            if (uiElement != null)
            {
                await DebugConsoleViewModel.Instance.VisualTreeView.SelectItemAsync(
                    uiElement,
                    refreshOnFail: true // need to refresh because otherwise if the tree is not up to date - selection won't change for elements in the tree already loaded
                    );
                var fe = new FocusEvent(DebugConsoleViewModel.Instance.VisualTreeView.SelectedItem);
                this.FocusEvents.Add(fe);
                this.SelectedEvent = fe;
            }
            else
            {
                var fe = new FocusEvent(null);
                this.FocusEvents.Add(fe);
                this.SelectedEvent = fe;
            }
        }

        private void AnalyzePossibleLossOfFocusReason()
        {
            var lastFocused = this.FocusEvents.LastOrDefault();

            if (lastFocused == null)
            {
                return;
            }

            var element = (lastFocused.Element as DependencyObjectViewModel)?.Model as UIElement;

            if (element != null)
            {
                string reasons = string.Empty;

                if (!element.IsInVisualTree())
                {
                    reasons = "Removed from tree";
                }
                if (element.Visibility == Visibility.Collapsed)
                {
                    reasons = string.IsNullOrEmpty(reasons) ? "Collapsed" : reasons + ", Collapsed";
                }
                if ((element as Control)?.IsEnabled == false)
                {
                    reasons = string.IsNullOrEmpty(reasons) ? "Disabled" : reasons + ", Disabled";
                }

                lastFocused.FocusLossPossibleReason = string.IsNullOrEmpty(reasons) ? $"Lost focus ({element.GetType().Name})" : reasons;
            }
            else
            {
                lastFocused.FocusLossPossibleReason = "Lost focus";
            }
        }
        #endregion

        #region Remove()
        internal override void Remove()
        {
            if (_focusTracker != null)
            {
                _focusTracker.FocusChanged -= this.OnFocusChanged;
            }

            base.Remove();
        }
        #endregion
    }
}