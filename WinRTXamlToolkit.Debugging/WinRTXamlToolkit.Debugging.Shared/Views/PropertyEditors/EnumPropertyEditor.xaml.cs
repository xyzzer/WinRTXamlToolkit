using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class EnumPropertyEditor : UserControl
    {
        private bool _updatingSelection;
        private bool _isFlags;
        private IEnumerable<EnumValueInfo> _valueList;

        public class EnumValueInfo
        {
            public string DisplayName { get; set; }
            public object Value { get; set; }
        }

        #region EnumType
        /// <summary>
        /// EnumType Dependency Property
        /// </summary>
        public static readonly DependencyProperty EnumTypeProperty =
            DependencyProperty.Register(
                "EnumType",
                typeof(Type),
                typeof(EnumPropertyEditor),
                new PropertyMetadata(null, OnEnumTypeChanged));

        /// <summary>
        /// Gets or sets the EnumType property. This dependency property 
        /// indicates the type of the enum.
        /// </summary>
        public Type EnumType
        {
            get { return (Type)GetValue(EnumTypeProperty); }
            set { SetValue(EnumTypeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the EnumType property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnEnumTypeChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EnumPropertyEditor)d;
            Type oldEnumType = (Type)e.OldValue;
            Type newEnumType = target.EnumType;
            target.OnEnumTypeChanged(oldEnumType, newEnumType);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the EnumType property.
        /// </summary>
        /// <param name="oldEnumType">The old EnumType value</param>
        /// <param name="newEnumType">The new EnumType value</param>
        private void OnEnumTypeChanged(
            Type oldEnumType, Type newEnumType)
        {
            if (newEnumType == null)
            {
                _valueList = null;
                combo.ItemsSource = null;

                return;
            }

            var typeInfo = newEnumType.GetTypeInfo();

            _isFlags = typeInfo.GetCustomAttribute(typeof (FlagsAttribute)) != null;
            combo.SelectionMode = _isFlags
                                        ? SelectionMode.Multiple
                                        : SelectionMode.Single;

            _valueList = null;

            if (typeInfo.IsEnum)
            {
                combo.ItemsSource =
                    _valueList = 
                        Enum.GetValues(newEnumType)
                            .Cast<object>()
                            .Select(ev => new EnumValueInfo { DisplayName = ev.ToString(), Value = ev})
                            .ToList();
            }
            else if (
                typeInfo.IsGenericType &&
                newEnumType.GetGenericTypeDefinition() == typeof (Nullable<>) &&
                newEnumType.GetTypeInfo().GenericTypeArguments[0].GetTypeInfo().IsEnum)
            {
                newEnumType = newEnumType.GetGenericTypeDefinition().GenericTypeArguments[0];
                combo.ItemsSource =
                    new [] {new EnumValueInfo { DisplayName = string.Empty, Value = null}}
                        .Concat(
                            Enum.GetValues(newEnumType)
                                .Cast<object>()
                                .Select(ev => new EnumValueInfo { DisplayName = ev.ToString(), Value = ev }))
                                .ToList();
            }
            else
            {
                throw new InvalidOperationException("Error in EnumPropertyEditor logic");
            }

            if (this.Model != null)
            {
                UpdateSelection();
            }
        }
        #endregion

        #region Model
        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(
                "Model",
                typeof(object),
                typeof(EnumPropertyEditor),
                new PropertyMetadata(null, OnModelChanged));

        /// <summary>
        /// Gets or sets the Model property. This dependency property 
        /// indicates the property view model to update with the values.
        /// </summary>
        public object Model
        {
            get { return (object)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Model property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnModelChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EnumPropertyEditor)d;
            BasePropertyViewModel oldModel = (BasePropertyViewModel)e.OldValue;
            BasePropertyViewModel newModel = (BasePropertyViewModel)target.Model;
            target.OnModelChanged(oldModel, newModel);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Model property.
        /// </summary>
        /// <param name="oldModel">The old Model value</param>
        /// <param name="newModel">The new Model value</param>
        private void OnModelChanged(
            BasePropertyViewModel oldModel, BasePropertyViewModel newModel)
        {
            if (this.EnumType != null)
            {
                UpdateSelection();
            }
        }
        #endregion

        #region Value
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(object),
                typeof(EnumPropertyEditor),
                new PropertyMetadata(false, OnValueChanged));

        /// <summary>
        /// Gets or sets the Value property. This dependency property 
        /// indicates the value.
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnValueChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EnumPropertyEditor)d;
            object oldValue = (object)e.OldValue;
            object newValue = target.Value;
            target.OnValueChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Value property.
        /// </summary>
        /// <param name="oldValue">The old Value value</param>
        /// <param name="newValue">The new Value value</param>
        private void OnValueChanged(
            object oldValue, object newValue)
        {
            if (this.EnumType != null &&
                this.Model != null)
            {
                UpdateSelection();
            }
        }
        #endregion

        private void UpdateSelection()
        {
            _updatingSelection = true;

            if (!_isFlags)
            {
                var valueModel = _valueList.First(ev => ev.Value.Equals(((BasePropertyViewModel)this.Model).Value));
                combo.SelectedItem = valueModel;
            }
            else
            {
                combo.SelectedItems.Clear();
                var pvm = (BasePropertyViewModel)this.Model;
                var intValue = Convert.ToInt32(pvm.Value);

                foreach (var enumValueInfo in _valueList
                    .Where(
                        ev =>
                            ev.Value != null &&
                            (Convert.ToInt32(ev.Value) | intValue) == intValue))
                {
                    combo.SelectedItems.Add(enumValueInfo);
                }
            }

            _updatingSelection = false;
        }

        public EnumPropertyEditor()
        {
            this.InitializeComponent();
        }

        private void OnComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_updatingSelection)
            {
                if (!_isFlags && combo.SelectedItem != null)
                {
                    ((BasePropertyViewModel)this.Model).Value = ((EnumValueInfo)combo.SelectedItem).Value;
                }
                else
                {
                    int value = 0;

                    foreach (var ev in combo.SelectedItems.Cast<EnumValueInfo>())
                    {
                        value = value | Convert.ToInt32(ev.Value);
                    }

                    ((BasePropertyViewModel)this.Model).Value = value;
                }
            }
        }
    }
}
