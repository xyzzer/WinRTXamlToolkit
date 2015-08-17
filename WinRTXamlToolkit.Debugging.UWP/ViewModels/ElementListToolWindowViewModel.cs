using System.Collections.Generic;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class ElementListToolWindowViewModel : ToolWindowViewModel
    {
        public List<object> Elements { get; private set; }

        #region SelectedElement
        private object selectedElement;
        public object SelectedElement
        {
            get { return this.selectedElement; }
            set
            {
                if (this.SetProperty(ref this.selectedElement, value))
                {
                    var dobvm = this.selectedElement as DependencyObjectViewModel;

                    if (dobvm != null)
                    {
                        var uiElement = dobvm.Model as UIElement;

                        if (uiElement != null)
                        {
#pragma warning disable 4014
                            DebugConsoleViewModel.Instance.VisualTreeView.SelectItem(uiElement);
#pragma warning restore 4014
                        }
                    }
                }
            }
        }
        #endregion

        public string Header { get; private set; }

        public ElementListToolWindowViewModel(List<object> elements, string header)
        {
            this.Elements = elements;
            this.Header = header;
            //DebugConsoleViewModel.Instance.ToolWindows.Add(this);
        }

        internal void Remove()
        {
            DebugConsoleViewModel.Instance.ToolWindows.Remove(this);
        }
    }
}