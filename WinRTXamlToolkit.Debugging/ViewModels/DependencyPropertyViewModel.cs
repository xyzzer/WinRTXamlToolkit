using System;
using System.Reflection;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Common;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public partial class DependencyPropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyObjectViewModel _elementModel;
        private readonly DependencyPropertyInfo _dpi;
        private readonly PropertyInfo _propertyInfo;
        private readonly Type _propertyType;

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

        private readonly DependencyProperty _dependencyProperty;
        public DependencyProperty DependencyProperty
        {
            get
            {
                return _dependencyProperty;
            }
        }

        public object DefaultValue { get; private set; }

        private DependencyObjectViewModel _asObjectViewModel;
        public DependencyObjectViewModel AsObjectViewModel
        {
            get
            {
                if (_asObjectViewModel == null)
                {
                    _asObjectViewModel = new DependencyObjectViewModel(_elementModel.TreeModel, null, (DependencyObject)this.Value);
#pragma warning disable 4014
                    _asObjectViewModel.LoadProperties();
#pragma warning restore 4014
                }

                return _asObjectViewModel;
            }
        }

        #region CTOR
        public DependencyPropertyViewModel(
            DependencyObjectViewModel elementModel,
            DependencyPropertyInfo dpi)
        {
            _elementModel = elementModel;
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
                    _dpi.Property.GetMetadata(_elementModel.Model.GetType())
                        .DefaultValue;

                if (this.DefaultValue != null)
                {
                    _propertyType = this.DefaultValue.GetType();
                }
                else
                {
                    _propertyType = typeof (object);
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
                if (!(_elementModel.Model is UIElement) &&
                    (this.DependencyProperty == Grid.RowProperty
                    || this.DependencyProperty == Grid.ColumnProperty
                    || this.DependencyProperty == Grid.RowSpanProperty
                    || this.DependencyProperty == Grid.ColumnSpanProperty
                    || this.DependencyProperty == Canvas.LeftProperty
                    || this.DependencyProperty == Canvas.TopProperty
                    || this.DependencyProperty == Canvas.ZIndexProperty))
                {
                    return 0;
                }

                return _elementModel.Model.GetValue(this.DependencyProperty);
            }
            set
            {
                try
                {
                    if (CoercionHelper != null)
                    {
                        CoercionHelper.CoerceValue(ref value);
                    }

                    _elementModel.Model.SetValue(DependencyProperty, value);
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
                    if (!(this._elementModel.Model is UIElement) &&
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
                        var localValue = _elementModel.Model.ReadLocalValue(this.DependencyProperty);
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

        public override bool CanResetValue
        {
            get
            {
                return !this.IsReadOnly && !this.IsDefault;
            }
        }

        public override void ResetValue()
        {
            _elementModel.Model.ClearValue(_dependencyProperty);
            _isDefault = null;

 // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Value");
            OnPropertyChanged("ValueString");
            OnPropertyChanged("CanResetValue");
            OnPropertyChanged("IsDefault");
// ReSharper restore ExplicitCallerInfoArgument
       }

        public override bool CanAnalyze
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Analyzes the property value source and displays the report in a dialog.
        /// </summary>
        public override void Analyze()
        {
            var sb = new StringBuilder();
            var dependencyObject = _elementModel.Model;
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
    }
}