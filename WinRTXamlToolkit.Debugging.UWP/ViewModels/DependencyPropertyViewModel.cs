using System;
using System.Reflection;
using System.Text;
using WinRTXamlToolkit.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Common;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public partial class DependencyPropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyPropertyInfo _dpi;
        private readonly PropertyInfo _propertyInfo;
        private readonly Type _propertyType;

        #region CoercionHelper
        private IValueCoercionHelper _coercionHelper;
        internal IValueCoercionHelper CoercionHelper
        {
            get
            {
                return _coercionHelper;
            }
            set
            {
                _coercionHelper = value;
            }
        }
        #endregion

        #region DependencyProperty
        private readonly DependencyProperty _dependencyProperty;
        public DependencyProperty DependencyProperty
        {
            get
            {
                return _dependencyProperty;
            }
        }
        #endregion

        #region Category
        public override string Category
        {
            get
            {
                #region Appearance
                if (_dependencyProperty == UIElement.VisibilityProperty ||
                    _dependencyProperty == UIElement.OpacityProperty ||
                    _dependencyProperty == Control.BorderThicknessProperty ||
                    _dependencyProperty == Border.CornerRadiusProperty ||
                    _dependencyProperty == Rectangle.RadiusXProperty ||
                    _dependencyProperty == Rectangle.RadiusYProperty ||
                    _dependencyProperty == Viewbox.StretchProperty ||
                    _dependencyProperty == Viewbox.StretchDirectionProperty ||
                    _dependencyProperty == Shape.StretchProperty ||
                    _dependencyProperty == Shape.StrokeDashArrayProperty ||
                    _dependencyProperty == Shape.StrokeDashCapProperty ||
                    _dependencyProperty == Shape.StrokeDashOffsetProperty ||
                    _dependencyProperty == Shape.StrokeEndLineCapProperty ||
                    _dependencyProperty == Shape.StrokeLineJoinProperty ||
                    _dependencyProperty == Shape.StrokeMiterLimitProperty ||
                    _dependencyProperty == Shape.StrokeStartLineCapProperty ||
                    _dependencyProperty == Shape.StrokeThicknessProperty ||
                    _dependencyProperty == FrameworkElement.RequestedThemeProperty
                    )
                {
                    return AppearanceCategoryName;
                }
                #endregion

                #region Brush
                if (_dependencyProperty == TextBlock.ForegroundProperty ||
                    _dependencyProperty == Control.ForegroundProperty ||
                    _dependencyProperty == RichTextBlock.ForegroundProperty ||
                    _dependencyProperty == ContentPresenter.ForegroundProperty ||
                    _dependencyProperty == Control.BackgroundProperty ||
                    _dependencyProperty == Control.BorderBrushProperty ||
                    _dependencyProperty == Shape.FillProperty ||
                    _dependencyProperty == Shape.StrokeProperty ||
                    _dependencyProperty == IconElement.ForegroundProperty ||
                    _dependencyProperty == TextBlock.SelectionHighlightColorProperty ||
                    _dependencyProperty == TextBox.SelectionHighlightColorProperty ||
                    _dependencyProperty == RichTextBlock.SelectionHighlightColorProperty ||
                    _dependencyProperty == RichEditBox.SelectionHighlightColorProperty ||
                    _dependencyProperty == PasswordBox.SelectionHighlightColorProperty
)
                {
                    return BrushCategoryName;
                }
                #endregion

                #region Common
                if (_dependencyProperty == Control.IsEnabledProperty ||
                    _dependencyProperty == FrameworkElement.AllowDropProperty ||
                    _dependencyProperty == FrameworkElement.DataContextProperty ||
                    _dependencyProperty == FrameworkElement.TagProperty ||
                    _dependencyProperty == UIElement.IsHitTestVisibleProperty ||
                    _dependencyProperty == Path.DataProperty ||
                    _dependencyProperty == ToolTipService.ToolTipProperty ||
                    _dependencyProperty == TextBlock.TextProperty ||
                    _dependencyProperty == TextBlock.IsTextSelectionEnabledProperty ||
                    _dependencyProperty == TextBox.TextProperty ||
                    _dependencyProperty == RichTextBlock.SelectedTextProperty ||
                    _dependencyProperty == RichTextBlock.IsTextSelectionEnabledProperty ||
                    _dependencyProperty == PasswordBox.PasswordProperty ||
                    _dependencyProperty == PasswordBox.PasswordCharProperty ||
                    _dependencyProperty == Image.SourceProperty ||
                    _dependencyProperty == Panel.ChildrenTransitionsProperty ||
                    _dependencyProperty == ItemsControl.ItemContainerTransitionsProperty ||
                    _dependencyProperty == UIElement.TransitionsProperty ||
                    _dependencyProperty == ContentControl.ContentProperty ||
                    _dependencyProperty == ContentPresenter.ContentProperty ||
                    _dependencyProperty == ContentControl.ContentTransitionsProperty ||
                    _dependencyProperty == ContentPresenter.ContentTransitionsProperty ||
                    _dependencyProperty == ScrollViewer.HorizontalScrollModeProperty ||
                    _dependencyProperty == ScrollViewer.VerticalScrollModeProperty ||
                    _dependencyProperty == ScrollViewer.MinZoomFactorProperty ||
                    _dependencyProperty == ScrollViewer.MaxZoomFactorProperty ||
                    _dependencyProperty == ScrollViewer.ZoomModeProperty ||
                    _dependencyProperty == Control.IsTabStopProperty ||
                    _dependencyProperty == Control.TabIndexProperty ||
                    _dependencyProperty == Control.TabNavigationProperty ||
                    _dependencyProperty == ScrollViewer.VerticalSnapPointsAlignmentProperty ||
                    _dependencyProperty == ScrollViewer.VerticalSnapPointsTypeProperty ||
                    _dependencyProperty == ScrollViewer.HorizontalSnapPointsAlignmentProperty ||
                    _dependencyProperty == ScrollViewer.HorizontalSnapPointsTypeProperty ||
                    _dependencyProperty == ScrollViewer.ZoomSnapPointsProperty ||
                    _dependencyProperty == ScrollViewer.ZoomSnapPointsTypeProperty ||
                    (_dpi.OwnerType == typeof(ItemsControl) && Name == "Items") ||
                    _dependencyProperty == ItemsControl.ItemsSourceProperty ||
                    _dependencyProperty == ItemsControl.DisplayMemberPathProperty ||
                    _dependencyProperty == Selector.IsSynchronizedWithCurrentItemProperty ||
                    _dependencyProperty == Selector.SelectedIndexProperty ||
                    _dependencyProperty == Selector.SelectedItemProperty ||
                    _dependencyProperty == Selector.SelectedValueProperty ||
                    _dependencyProperty == Selector.SelectedValuePathProperty ||
                    _dependencyProperty == ListBox.SelectionModeProperty ||
                    _dependencyProperty == ListViewBase.CanDragItemsProperty ||
                    //_dependencyProperty == ListViewBase.CanReorderItemsProperty || // deprecated
                    _dependencyProperty == ListViewBase.IsItemClickEnabledProperty ||
                    _dependencyProperty == ListViewBase.IsSwipeEnabledProperty ||
                    _dependencyProperty == RichEditBox.AcceptsReturnProperty ||
                    _dependencyProperty == SymbolIcon.SymbolProperty ||
                    _dependencyProperty == Flyout.PlacementProperty ||
                    _dependencyProperty == TextBox.PlaceholderTextProperty ||
                    _dependencyProperty == RichEditBox.PlaceholderTextProperty ||
                    _dependencyProperty == ComboBox.PlaceholderTextProperty ||
                    _dependencyProperty == ComboBox.HeaderProperty ||
                    _dependencyProperty == ListViewBase.HeaderProperty ||
                    _dependencyProperty == TextBox.HeaderProperty ||
                    _dependencyProperty == RichEditBox.HeaderProperty
)
                {
                    return CommonCategoryName;
                }
                #endregion

                #region Interactions
                if (_dependencyProperty == UIElement.IsDoubleTapEnabledProperty ||
                    _dependencyProperty == UIElement.IsHoldingEnabledProperty ||
                    _dependencyProperty == UIElement.IsTapEnabledProperty ||
                    _dependencyProperty == UIElement.IsRightTapEnabledProperty ||
                    _dependencyProperty == UIElement.ManipulationModeProperty
                    )
                {
                    return InteractionsCategoryName;
                }
                #endregion

                #region Layout
                if (_dependencyProperty == FrameworkElement.WidthProperty ||
                    _dependencyProperty == FrameworkElement.HeightProperty ||
                    _dependencyProperty == FrameworkElement.MaxWidthProperty ||
                    _dependencyProperty == FrameworkElement.MinWidthProperty ||
                    _dependencyProperty == FrameworkElement.MaxHeightProperty ||
                    _dependencyProperty == FrameworkElement.MinHeightProperty ||
                    _dependencyProperty == FrameworkElement.ActualWidthProperty ||
                    _dependencyProperty == FrameworkElement.ActualHeightProperty ||
                    _dependencyProperty == FrameworkElement.MarginProperty ||
                    _dependencyProperty == FrameworkElement.HorizontalAlignmentProperty ||
                    _dependencyProperty == FrameworkElement.VerticalAlignmentProperty ||
                    _dependencyProperty == Control.PaddingProperty ||
                    _dependencyProperty == FrameworkElement.FlowDirectionProperty ||
                    _dependencyProperty == Control.HorizontalContentAlignmentProperty ||
                    _dependencyProperty == Control.VerticalContentAlignmentProperty ||
                    _dependencyProperty == UIElement.ClipProperty ||
                    _dependencyProperty == UIElement.UseLayoutRoundingProperty ||
                    _dependencyProperty == StackPanel.OrientationProperty ||
                    _dependencyProperty == Grid.ColumnProperty ||
                    _dependencyProperty == Grid.RowProperty ||
                    _dependencyProperty == Grid.ColumnSpanProperty ||
                    _dependencyProperty == Grid.RowSpanProperty ||
                    _dependencyProperty == Canvas.LeftProperty ||
                    _dependencyProperty == Canvas.TopProperty ||
                    _dependencyProperty == Canvas.ZIndexProperty ||
                    _dependencyProperty == WrapGrid.ItemWidthProperty ||
                    _dependencyProperty == WrapGrid.ItemHeightProperty ||
                    _dependencyProperty == WrapGrid.OrientationProperty ||
                    _dependencyProperty == VariableSizedWrapGrid.ItemWidthProperty ||
                    _dependencyProperty == VariableSizedWrapGrid.ItemHeightProperty ||
                    _dependencyProperty == VariableSizedWrapGrid.ColumnSpanProperty ||
                    _dependencyProperty == VariableSizedWrapGrid.RowSpanProperty ||
                    _dependencyProperty == VariableSizedWrapGrid.MaximumRowsOrColumnsProperty ||
                    _dependencyProperty == VariableSizedWrapGrid.OrientationProperty ||
                    _dependencyProperty == ScrollViewer.HorizontalScrollBarVisibilityProperty ||
                    _dependencyProperty == ScrollViewer.VerticalScrollBarVisibilityProperty ||
                    _dependencyProperty == WrapPanel.ItemWidthProperty ||
                    _dependencyProperty == WrapPanel.ItemHeightProperty)
                {
                    return LayoutCategoryName;
                }
                #endregion

                #region Text
                if (_dependencyProperty == TextBlock.FontFamilyProperty ||
                    _dependencyProperty == TextBlock.FontSizeProperty ||
                    _dependencyProperty == TextBlock.FontStretchProperty ||
                    _dependencyProperty == TextBlock.FontStyleProperty ||
                    _dependencyProperty == TextBlock.FontWeightProperty ||
                    _dependencyProperty == TextBlock.LineHeightProperty ||
                    _dependencyProperty == TextBlock.LineStackingStrategyProperty ||
                    _dependencyProperty == TextBlock.TextAlignmentProperty ||
                    _dependencyProperty == TextBlock.TextTrimmingProperty ||
                    _dependencyProperty == TextBlock.TextWrappingProperty ||
                    _dependencyProperty == TextBox.FontFamilyProperty ||
                    _dependencyProperty == TextBox.FontSizeProperty ||
                    _dependencyProperty == TextBox.FontStretchProperty ||
                    _dependencyProperty == TextBox.FontStyleProperty ||
                    _dependencyProperty == TextBox.FontWeightProperty ||
                    _dependencyProperty == TextBox.TextAlignmentProperty ||
                    _dependencyProperty == TextBox.TextWrappingProperty ||
                    _dependencyProperty == TextBox.IsSpellCheckEnabledProperty ||
                    _dependencyProperty == RichTextBlock.FontFamilyProperty ||
                    _dependencyProperty == RichTextBlock.FontSizeProperty ||
                    _dependencyProperty == RichTextBlock.FontStretchProperty ||
                    _dependencyProperty == RichTextBlock.FontStyleProperty ||
                    _dependencyProperty == RichTextBlock.FontWeightProperty ||
                    _dependencyProperty == RichTextBlock.LineHeightProperty ||
                    _dependencyProperty == RichTextBlock.LineStackingStrategyProperty ||
                    _dependencyProperty == RichTextBlock.TextAlignmentProperty ||
                    _dependencyProperty == RichTextBlock.TextTrimmingProperty ||
                    _dependencyProperty == RichTextBlock.TextWrappingProperty ||
                    _dependencyProperty == RichTextBlock.TextIndentProperty ||
                    _dependencyProperty == RichEditBox.FontFamilyProperty ||
                    _dependencyProperty == RichEditBox.FontSizeProperty ||
                    _dependencyProperty == RichEditBox.FontStretchProperty ||
                    _dependencyProperty == RichEditBox.FontStyleProperty ||
                    _dependencyProperty == RichEditBox.FontWeightProperty ||
                    _dependencyProperty == RichEditBox.TextAlignmentProperty ||
                    _dependencyProperty == RichEditBox.TextWrappingProperty ||
                    _dependencyProperty == TextBlock.CharacterSpacingProperty ||
                    _dependencyProperty == PasswordBox.CharacterSpacingProperty ||
                    _dependencyProperty == RichTextBlock.CharacterSpacingProperty ||
                    _dependencyProperty == Control.FontFamilyProperty ||
                    _dependencyProperty == Control.FontSizeProperty ||
                    _dependencyProperty == Control.FontStretchProperty ||
                    _dependencyProperty == Control.FontStyleProperty ||
                    _dependencyProperty == Control.FontWeightProperty ||
                    _dependencyProperty == ContentPresenter.FontFamilyProperty ||
                    _dependencyProperty == ContentPresenter.FontSizeProperty ||
                    _dependencyProperty == ContentPresenter.FontStretchProperty ||
                    _dependencyProperty == ContentPresenter.FontStyleProperty ||
                    _dependencyProperty == ContentPresenter.FontWeightProperty ||
                    _dependencyProperty == TextBlock.TextLineBoundsProperty ||
                    _dependencyProperty == RichTextBlock.TextLineBoundsProperty ||
                    _dependencyProperty == RichTextBlock.TextReadingOrderProperty ||
                    _dependencyProperty == TextBlock.TextReadingOrderProperty
                    )
                {
                    return TextCategoryName;
                }
                #endregion

                #region Transform
                if (_dependencyProperty == UIElement.RenderTransformProperty ||
                    _dependencyProperty == UIElement.RenderTransformOriginProperty ||
                    _dependencyProperty == UIElement.ProjectionProperty
                    )
                {
                    return TransformCategoryName;
                }
                #endregion

                #region WinRTXamlToolkitExtensions
                var propertyOwnerAssembly = _dpi.OwnerType.GetTypeInfo().Assembly;
                var propertyOwnerNamespace = _dpi.OwnerType.Namespace;

                if (propertyOwnerNamespace.StartsWith(typeof(ListBoxExtensions).Namespace))
                {
                    return WinRTXamlToolkitExtensionsCategoryName;
                }
                #endregion

                #region WinRTXamlToolkitControl
                if (propertyOwnerNamespace.StartsWith(typeof(WrapPanel).Namespace))
                {
                    return WinRTXamlToolkitControlCategoryName;
                }
                #endregion

                #region WinRTXamlToolkitDebugging
                if (propertyOwnerAssembly.Equals(
                            typeof(DependencyPropertyViewModel).GetTypeInfo().Assembly))
                {
                    return WinRTXamlToolkitDebuggingCategoryName;
                }
                #endregion

                return MiscCategoryName;
            }
        }
        #endregion

        public object DefaultValue { get; private set; }

        #region AsObjectViewModel
        private DependencyObjectViewModel _asObjectViewModel;
        public DependencyObjectViewModel AsObjectViewModel
        {
            get
            {
                if (_asObjectViewModel == null)
                {
                    _asObjectViewModel = new DependencyObjectViewModel(this.ElementModel.TreeModel, null, (DependencyObject)this.Value);
#pragma warning disable 4014
                    _asObjectViewModel.LoadPropertiesAsync();
#pragma warning restore 4014
                }

                return _asObjectViewModel;
            }
        }
        #endregion

        #region CTOR
        public DependencyPropertyViewModel(
            DependencyObjectViewModel elementModel,
            DependencyPropertyInfo dpi)
            : base(elementModel)
        {
            _dpi = dpi;
            _dependencyProperty = dpi.Property;
            this.Name = dpi.DisplayName;
            _propertyInfo = elementModel.Model.GetType().GetRuntimeProperty(dpi.DisplayName);

            if (_propertyInfo != null)
            {
                var accessor = _propertyInfo.GetMethod ?? _propertyInfo.SetMethod;

                _propertyType = accessor.ReturnType;
            }

            if (_propertyType == null)
            {
                this.DefaultValue =
                    _dpi.Property.GetMetadata(this.ElementModel.Model.GetType())
                        .DefaultValue;

                if (this.DefaultValue != null)
                {
                    _propertyType = this.DefaultValue.GetType();
                }
                else
                {
                    _propertyType = typeof(object);
                }
            }

            CoercionHelper =
                ValueCoercionHelperFactory.GetValueCoercionHelper(DependencyProperty);
        }
        #endregion

        #region Value
        public override object Value
        {
            get
            {
                object val;

                if (this.TryGetValue(this.ElementModel.Model, out val))
                {
                    return val;
                }

                return 0;
            }
            set
            {
                try
                {
                    if (CoercionHelper != null)
                    {
                        CoercionHelper.CoerceValue(ref value);
                    }

                    this.ElementModel.Model.SetValue(DependencyProperty, value);
                    _isDefault = null;
                    OnPropertyChanged();
                    // ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged("CanResetValue");
                    OnPropertyChanged("IsDefault");
                    // ReSharper restore ExplicitCallerInfoArgument
                }
                catch
                {
                }
            }
        }

        public override Type PropertyType
        {
            get
            {
                return _propertyType;
            }
        }
        #endregion

        #region IsDefault
        public override bool IsDefault
        {
            get
            {
                try
                {
                    if (!(this.ElementModel.Model is UIElement) &&
                        (this.DependencyProperty == Grid.RowProperty
                        || this.DependencyProperty == Grid.ColumnProperty
                        || this.DependencyProperty == Grid.RowSpanProperty
                        || this.DependencyProperty == Grid.ColumnSpanProperty
                        || this.DependencyProperty == Canvas.LeftProperty
                        || this.DependencyProperty == Canvas.TopProperty
                        || this.DependencyProperty == Canvas.ZIndexProperty))
                    {
                        return true;
                    }

                    if (_isDefault == null)
                    {
                        var localValue = this.ElementModel.Model.ReadLocalValue(this.DependencyProperty);
                        _isDefault = localValue == DependencyProperty.UnsetValue;
                    }

                    return _isDefault.Value;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                    return true;
                }
            }
        }
        #endregion

        #region IsReadOnly
        public override bool IsReadOnly
        {
            get
            {
                if (DependencyPropertyCache.AttachedProperties.Contains(_dpi))
                {
                    return false;
                }

                if (_propertyInfo == null)
                {
                    return true;
                }

                var sm = _propertyInfo.SetMethod;

                return sm == null;
            }
        }
        #endregion

        #region CanResetValue
        public override bool CanResetValue
        {
            get
            {
                return !this.IsReadOnly && !this.IsDefault;
            }
        }
        #endregion

        #region IsAttached
        public bool IsAttached
        {
            get
            {
                return _dpi.IsAttached;
            }
        }
        #endregion

        #region ResetValue()
        public override void ResetValue()
        {
            this.ElementModel.Model.ClearValue(_dependencyProperty);
            _isDefault = null;

            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Value");
            OnPropertyChanged("ValueString");
            OnPropertyChanged("CanResetValue");
            OnPropertyChanged("IsDefault");
            // ReSharper restore ExplicitCallerInfoArgument
        }
        #endregion

        #region CanAnalyze
        public override bool CanAnalyze
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Analyze()
        /// <summary>
        /// Analyzes the property value source and displays the report in a dialog.
        /// </summary>
        public override void Analyze()
        {
            var sb = new StringBuilder();
            var dependencyObject = this.ElementModel.Model;
            var dp = _dependencyProperty;

            // Value
            sb.AppendFormat("Value: {0}\r\n", dependencyObject.GetValue(dp) ?? "<null>");

            // Animation base value
            sb.AppendFormat("Animation base value: {0}\r\n", dependencyObject.GetAnimationBaseValue(dp) ?? "<null>");
            var localValue = dependencyObject.ReadLocalValue(dp);

            // Local value
            if (localValue != DependencyProperty.UnsetValue)
            {
                sb.AppendFormat(
                    "Local Value: {0}\r\n", dependencyObject.ReadLocalValue(dp) ?? "<null>");
            }

            // Style value
            var fe = dependencyObject as FrameworkElement;
            if (fe != null &&
                fe.Style != null)
            {
                var styleValue = fe.Style.GetPropertyValue(dp);

                if (styleValue != DependencyProperty.UnsetValue)
                {
                    sb.AppendFormat("Style Value: {0}\r\n", styleValue ?? "<null>");
                }
            }

            // Default value
            sb.AppendFormat("Default Value: {0}\r\n", this.DefaultValue ?? "<null>");

#pragma warning disable 4014
            new MessageDialog(sb.ToString()).ShowAsync();
#pragma warning restore 4014
        }
        #endregion

        #region TryGetValue()
        public override bool TryGetValue(object model, out object val)
        {
            var dob = model as DependencyObject;

            if (dob == null)
            {
                val = 0;
                return false;
            }

            if (!(this.ElementModel.Model is UIElement) &&
                (this.DependencyProperty == Grid.RowProperty
                || this.DependencyProperty == Grid.ColumnProperty
                || this.DependencyProperty == Grid.RowSpanProperty
                || this.DependencyProperty == Grid.ColumnSpanProperty
                || this.DependencyProperty == Canvas.LeftProperty
                || this.DependencyProperty == Canvas.TopProperty
                || this.DependencyProperty == Canvas.ZIndexProperty))
            {
                val = 0;
                return false;
            }

            try
            {
                val = dob.GetValue(this.DependencyProperty);
            }
            catch
            {
                val = 0;
                return false;
            }

            return true;
        } 
        #endregion
    }
}