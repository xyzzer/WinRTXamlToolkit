using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
    public interface IToolStripElement
    {
        bool IsInDropDown { get; set; }
    }

    [ContentProperty(Name = "Elements")]
    [TemplatePart(Name = BarGridName, Type = typeof(Grid))]
    [TemplatePart(Name = BarPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = DropDownButtonName, Type = typeof(Button))]
    [TemplatePart(Name = DropDownPopupName, Type = typeof(Popup))]
    [TemplatePart(Name = DropDownPanelName, Type = typeof(StackPanel))]
    public sealed class ToolStrip : Control
    {
        #region Template part names
        private const string BarGridName = "BarGrid";
        private const string BarPanelName = "BarPanel";
        private const string DropDownButtonName = "DropDownButton";
        private const string DropDownPopupName = "DropDownPopup";
        private const string DropDownPanelName = "DropDownPanel"; 
        #endregion

        #region Template parts
        private Grid _barGrid;
        private StackPanel _barPanel;
        private Button _dropDownButton;
        private Popup _dropDownPopup;
        private StackPanel _dropDownPanel; 
        #endregion

        private List<IToolStripElement> _barElements = new List<IToolStripElement>();
        private List<IToolStripElement> _overflowElements = new List<IToolStripElement>();

        public ObservableCollection<IToolStripElement> Elements { get; private set; }

        #region CTOR
        public ToolStrip()
        {
            this.DefaultStyleKey = typeof(ToolStrip);
            this.Elements = new ObservableCollection<IToolStripElement>();
            this.Elements.CollectionChanged += this.OnElementsCollectionChanged;
            this.SizeChanged += this.OnSizeChanged;
        }
        #endregion

        #region OnSizeChanged()
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ReflowElements();
        }
        #endregion

        #region OnElementsCollectionChanged()
        private void OnElementsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ReflowElements();

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<ButtonBase>())
                {
                    item.KeyDown += this.OnButtonKeyDown;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<ButtonBase>())
                {
                    item.KeyDown += this.OnButtonKeyDown;
                }
            }
        }
        #endregion

        #region OnButtonKeyDown()
        private void OnButtonKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (_dropDownPanel == null ||
                _barPanel == null ||
                _dropDownButton == null ||
                _dropDownPopup == null)
            {
                return;
            }

            IToolStripElement element;
            ButtonBase button;
            CastAndVerifyItem(sender, out element, out button);

            if (e.Key == VirtualKey.Down || e.Key == VirtualKey.Right)
            {
                if (element.IsInDropDown)
                {
                    var index = _dropDownPanel.Children.IndexOf(button);

                    if (_dropDownPanel.Children.Count > index)
                    {
                        var nextDropDownButton = _dropDownPanel.Children[index + 1] as ButtonBase;

                        if (nextDropDownButton != null &&
                            nextDropDownButton.IsTabStop)
                        {
                            nextDropDownButton.Focus(FocusState.Keyboard);
                        }
                    }
                }
                else
                {
                    var index = _barPanel.Children.IndexOf(button);

                    if (index < _barPanel.Children.Count - 2)
                    {
                        var nextBarButton = _barPanel.Children[index + 1] as ButtonBase;

                        if (nextBarButton != null &&
                            nextBarButton.IsTabStop)
                        {
                            nextBarButton.Focus(FocusState.Keyboard);
                        }
                    }
                    else if (_dropDownPanel.Children.Count > 0)
                    {
                        this.PositionDropDownPopup();
                        _dropDownPopup.IsOpen = true;
                        var firstButton = _dropDownPanel.Children[0] as ButtonBase;

                        if (firstButton != null &&
                            firstButton.IsTabStop)
                        {
                            firstButton.Focus(FocusState.Keyboard);
                        }
                    }
                }
            }
            else if (e.Key == VirtualKey.Up || e.Key == VirtualKey.Left)
            {
                if (element.IsInDropDown)
                {
                    var index = _dropDownPanel.Children.IndexOf(button);

                    if (index > 0)
                    {
                        var previousDropDownButton = _dropDownPanel.Children[index - 1] as ButtonBase;

                        if (previousDropDownButton != null &&
                            previousDropDownButton.IsTabStop)
                        {
                            previousDropDownButton.Focus(FocusState.Keyboard);
                        }
                    }
                    else if (_barPanel.Children.Count > 1)
                    {
                        var lastBarButton = _barPanel.Children[_barPanel.Children.Count - 2] as ButtonBase;

                        if (lastBarButton != null &&
                            lastBarButton.IsTabStop)
                        {
                            lastBarButton.Focus(FocusState.Keyboard);
                        }
                    }
                }
                else
                {
                    var index = _barPanel.Children.IndexOf(button);

                    if (index > 0)
                    {
                        var previousBarButton = _barPanel.Children[index - 1] as ButtonBase;

                        if (previousBarButton != null &&
                            previousBarButton.IsTabStop)
                        {
                            previousBarButton.Focus(FocusState.Keyboard);
                        }
                    }
                }
            }
        }
        #endregion

        #region ReflowElements()
        private void ReflowElements()
        {
            if (_barPanel == null ||
                _dropDownPanel == null ||
                _barGrid == null ||
                _dropDownButton == null)
            {
                return;
            }

            #region ClearElements
            _barElements.Clear();
            _overflowElements.Clear();

            for (int i = 0; i < _barPanel.Children.Count; )
            {
                if (_barPanel.Children[i] != _dropDownButton)
                {
                    _barPanel.Children.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            _dropDownPanel.Children.Clear(); 
            #endregion

            bool addingToBar = true;

            int insertIndex = 0;
            _dropDownButton.Visibility = Visibility.Collapsed;

            foreach (var item in this.Elements)
            {
                IToolStripElement element;
                ButtonBase button;
                CastAndVerifyItem(item, out element, out button);

                if (addingToBar)
                {
                    element.IsInDropDown = false;
                    _barPanel.Children.Insert(insertIndex, button);
                    _barPanel.Measure(new Size(10000, 10000));

                    if (_barPanel.DesiredSize.Width < _barGrid.ActualWidth)
                    {
                        insertIndex++;
                        continue;
                    }
                    else
                    {
                        addingToBar = false;
                        _barPanel.Children.Remove(button);
                        _dropDownButton.Visibility = Visibility.Visible;
                        insertIndex = 0;

                        _barPanel.Measure(new Size(10000, 10000));

                        while (
                            _barPanel.DesiredSize.Width > _barGrid.ActualWidth &&
                            _barPanel.Children.Count > 1)
                        {
                            var switchedElement = (IToolStripElement)_barPanel.Children[_barPanel.Children.Count - 2];
                            var switchedButton = (ButtonBase)switchedElement;
                            _barPanel.Children.RemoveAt(_barPanel.Children.Count - 2);
                            switchedElement.IsInDropDown = true;
                            _dropDownPanel.Children.Insert(0, switchedButton);
                            _barPanel.Measure(new Size(10000, 10000));
                        }
                    }
                }

                if (!addingToBar)
                {
                    element.IsInDropDown = true;
                    _dropDownPanel.Children.Add(button);
                }
            }
        }
        #endregion

        #region OnElementAdded()
        private void OnElementAdded(object newItem)
        {
            IToolStripElement element;
            ButtonBase button;
            CastAndVerifyItem(newItem, out element, out button);
        }

        private static void CastAndVerifyItem(object item, out IToolStripElement element, out ButtonBase button)
        {
            if (item == null)
            {
                throw new ArgumentNullException("element", "ToolStrip.Elements doesn't accept null elements.");
            }

            element = item as IToolStripElement;

            if (element == null)
            {
                throw new ArgumentException("ToolStrip.Elements only accepts ButtonBase, IToolStripElement elements.", "element");
            }

            button = item as ButtonBase;

            if (button == null)
            {
                throw new ArgumentException("ToolStrip.Elements only accepts ButtonBase, IToolStripElement elements.", "element");
            }
        }

        #endregion

        #region OnElementRemoved()
        private void OnElementRemoved(object oldItem)
        {
        } 
        #endregion

        #region OnApplyTemplate()
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.UnregisterTemplateParts();
            this.RegisterTemplateParts();
            this.ReflowElements();
        } 
        #endregion

        #region RegisterTemplateParts()
        private void RegisterTemplateParts()
        {
            _barGrid = this.GetTemplateChild(BarGridName) as Grid;
            _barPanel = this.GetTemplateChild(BarPanelName) as StackPanel;
            _dropDownButton = this.GetTemplateChild(DropDownButtonName) as Button;
            _dropDownPopup = this.GetTemplateChild(DropDownPopupName) as Popup;
            _dropDownPanel = this.GetTemplateChild(DropDownPanelName) as StackPanel;

            if (_barGrid != null &&
                _dropDownPopup != null &&
                _barGrid.Children.Contains(_dropDownPopup))
            {
                _barGrid.Children.Remove(_dropDownPopup);
            }

            if (_dropDownButton != null)
            {
                _dropDownButton.Click += this.OnDropDownButtonClick;
            }

            if (_dropDownPanel != null)
            {
                _dropDownPanel.SizeChanged += this.OnDropDownPanelSizeChanged;
            }
        }
        #endregion

        #region UnregisterTemplateParts()
        private void UnregisterTemplateParts()
        {
            if (_dropDownButton != null)
            {
                _dropDownButton.Click -= this.OnDropDownButtonClick;
            }

            if (_dropDownPanel != null)
            {
                _dropDownPanel.SizeChanged -= this.OnDropDownPanelSizeChanged;
            }
        } 
        #endregion

        #region OnDropDownPanelSizeChanged()
        private void OnDropDownPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_dropDownPopup.IsOpen)
            {
                this.PositionDropDownPopup();
            }
        } 
        #endregion

        #region PositionDropDownPopup()
        private void PositionDropDownPopup()
        {
            var dropDownButtonBounds = _dropDownButton.GetBoundingRect();
            var windowBounds = Window.Current.Bounds;
            var panelHeight = _dropDownPanel.ActualHeight;
            var panelWidth = _dropDownPanel.ActualWidth;
            var spaceBelow = windowBounds.Height - dropDownButtonBounds.Bottom;
            var spaceAbove = dropDownButtonBounds.Top;

            _dropDownPopup.Width = panelWidth;
            _dropDownPopup.Height = panelHeight;

            if (spaceBelow >= panelHeight ||
                spaceAbove < spaceBelow)
            {
                _dropDownPopup.VerticalOffset = dropDownButtonBounds.Bottom;

                if (spaceBelow < panelHeight &&
                    spaceAbove < spaceBelow)
                {
                    _dropDownPopup.Height = spaceBelow;
                }
            }
            else
            {
                _dropDownPopup.VerticalOffset = Math.Max(0, dropDownButtonBounds.Top - panelHeight);

                if (spaceAbove < panelHeight &&
                    spaceAbove > spaceBelow)
                {
                    _dropDownPopup.Height = spaceAbove;
                }
            }

            if (dropDownButtonBounds.Right > panelWidth)
            {
                _dropDownPopup.HorizontalOffset = dropDownButtonBounds.Right - panelWidth;
            }
            else
            {
                _dropDownPopup.HorizontalOffset = 0;
            }
        } 
        #endregion

        #region OnDropDownButtonClick()
        private void OnDropDownButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_dropDownPopup != null &&
                _dropDownPanel != null &&
                _dropDownPanel.Children.Count > 0)
            {
                if (_dropDownPopup.IsOpen)
                {
                    _dropDownPopup.IsOpen = false;
                }
                else
                {
                    this.PositionDropDownPopup();
                    _dropDownPopup.IsOpen = true;
                    var firstButton = _dropDownPanel.Children[0] as ButtonBase;

                    if (firstButton != null &&
                        firstButton.IsTabStop)
                    {
                        firstButton.Focus(FocusState.Programmatic);
                    }
                }
            }
        } 
        #endregion
    }
}
