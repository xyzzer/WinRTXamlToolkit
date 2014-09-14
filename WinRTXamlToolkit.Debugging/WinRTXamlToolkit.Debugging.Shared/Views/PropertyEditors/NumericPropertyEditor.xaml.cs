using System;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class NumericPropertyEditor : UserControl
    {
        #region NumericType
        /// <summary>
        /// NumericType Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericTypeProperty =
            DependencyProperty.Register(
                "NumericType",
                typeof(Type),
                typeof(NumericPropertyEditor),
                new PropertyMetadata(null, OnNumericTypeChanged));

        /// <summary>
        /// Gets or sets the NumericType property. This dependency property 
        /// indicates the type.
        /// </summary>
        public Type NumericType
        {
            get { return (Type)GetValue(NumericTypeProperty); }
            set { SetValue(NumericTypeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NumericType property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnNumericTypeChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (NumericPropertyEditor)d;
            Type oldNumericType = (Type)e.OldValue;
            Type newNumericType = target.NumericType;
            target.OnNumericTypeChanged(oldNumericType, newNumericType);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the NumericType property.
        /// </summary>
        /// <param name="oldNumericType">The old NumericType value</param>
        /// <param name="newNumericType">The new NumericType value</param>
        private void OnNumericTypeChanged(
            Type oldNumericType, Type newNumericType)
        {
            if (this.Model != null)
            {
                ReadModelValue();
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
                typeof(NumericPropertyEditor),
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
            var target = (NumericPropertyEditor)d;
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
            if (oldModel != null)
            {
                oldModel.PropertyChanged -= OnModelPropertyChanged;
            }

            if (this.NumericType != null)
            {
                ReadModelValue();
            }

            if (newModel != null)
            {
                newModel.PropertyChanged += OnModelPropertyChanged;
            }
        }
        #endregion
        
        public NumericPropertyEditor()
        {
            this.InitializeComponent();
        }

        private void OnModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                ReadModelValue();
            }
        }

        /// <summary>
        /// Reads the model value and based on the type and known dependency property details determines the
        /// minimum and maximum values as well as comfortable change speeds.
        /// </summary>
        private void ReadModelValue()
        {
            var model = (BasePropertyViewModel)this.Model;
            var autoCheckBoxVisibility = Visibility.Collapsed;

            double smallChange = 1d;
            double minimum = double.MinValue;
            double maximum = double.MaxValue;
            double dragSpeed = 100;
            var valueBarVisibility = NumericUpDownValueBarVisibility.Collapsed;
            var valueFormat = "F0";

            if (this.NumericType == typeof(byte))
            {
                minimum = byte.MinValue;
                maximum = byte.MaxValue;
                this.NumericUpDown.Value = (byte)model.Value;
            }
            else if (this.NumericType == typeof(Int16))
            {
                minimum = Int16.MinValue;
                maximum = Int16.MaxValue;
                this.NumericUpDown.Value = (Int16)model.Value;
            }
            else if (this.NumericType == typeof(UInt16))
            {
                minimum = UInt16.MinValue;
                maximum = UInt16.MaxValue;
                this.NumericUpDown.Value = (UInt16)model.Value;
            }
            else if (this.NumericType == typeof(int))
            {
                minimum = int.MinValue;
                maximum = int.MaxValue;
                this.NumericUpDown.Value = (int)model.Value;
            }
            else if (this.NumericType == typeof(uint))
            {
                minimum = uint.MinValue;
                maximum = uint.MaxValue;
                this.NumericUpDown.Value = (uint)model.Value;
            }
            else if (this.NumericType == typeof(Int64))
            {
                minimum = Int64.MinValue;
                maximum = Int64.MaxValue;
                this.NumericUpDown.Value = (Int64)model.Value;
            }
            else if (this.NumericType == typeof(UInt64))
            {
                minimum = UInt64.MinValue;
                maximum = UInt64.MaxValue;
                this.NumericUpDown.Value = (UInt64)model.Value;
            }
            else if (this.NumericType == typeof(float))
            {
                minimum = float.MinValue;
                maximum = float.MaxValue;
                var value = (float)model.Value;
                
                if (float.IsNaN(value))
                {
                    AutoCheckBox.IsChecked = true;
                }
                else
                {
                    this.NumericUpDown.Value = value;
                    AutoCheckBox.IsChecked = false;
                }
            }
            else if (this.NumericType == typeof(double))
            {
                minimum = double.MinValue;
                maximum = double.MaxValue;
                var value = (double)model.Value;
                
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    this.AutoCheckBox.IsChecked = true;
                }
                else
                {
                    this.NumericUpDown.Value = value;
                    this.AutoCheckBox.IsChecked = false;
                }
            }

            var dpm = model as DependencyPropertyViewModel;

            if (dpm != null)
            {
                var dp = dpm.DependencyProperty;

                if (dp == FrameworkElement.WidthProperty ||
                    dp == FrameworkElement.HeightProperty ||
                    dp == FrameworkElement.MaxWidthProperty ||
                    dp == FrameworkElement.MaxHeightProperty)
                {
                    autoCheckBoxVisibility = Visibility.Visible;
                }

                if (dp == FrameworkElement.WidthProperty ||
                    dp == FrameworkElement.HeightProperty ||
                    dp == FrameworkElement.MinWidthProperty ||
                    dp == FrameworkElement.MaxWidthProperty ||
                    dp == FrameworkElement.MinHeightProperty ||
                    dp == FrameworkElement.MaxHeightProperty)
                {
                    minimum = 0;
                    dragSpeed = 100;
                }

                if (dp == TextBlock.LineHeightProperty ||
                    dp == RichTextBlock.LineHeightProperty)
                {
                    minimum = 0;
                    dragSpeed = 30;
                }

                if (dp == TextBlock.CharacterSpacingProperty ||
                    dp == Control.CharacterSpacingProperty ||
                    dp == RichTextBlock.CharacterSpacingProperty)
                {
                    dragSpeed = 100;
                }

                if (dp == Shape.StrokeThicknessProperty)
                {
                    minimum = 0;
                    maximum = 1000000;
                    dragSpeed = 10;
                }

                if (dp == FrameworkElement.OpacityProperty)
                {
                    smallChange = 0.001;
                    valueFormat = "F3";
                    minimum = 0;
                    maximum = 1;
                    dragSpeed = 0.99;
                    valueBarVisibility = NumericUpDownValueBarVisibility.Visible;
                }

                if (dp == Canvas.ZIndexProperty)
                {
                    minimum = int.MinValue;
                    maximum = 1000000;
                    dragSpeed = 20;
                }

                if (dp == Grid.ColumnProperty ||
                    dp == Grid.RowProperty ||
                    dp == Grid.ColumnSpanProperty ||
                    dp == Grid.RowSpanProperty)
                {
                    minimum = 0;
                    maximum = 1000;
                    dragSpeed = 10;
                }

                if (dp == Control.FontSizeProperty ||
                    dp == TextBlock.FontSizeProperty ||
                    dp == RichTextBlock.FontSizeProperty)
                {
                    minimum = 1;
                    maximum = 1000;
                    dragSpeed = 20;
                }
            }

            this.AutoCheckBox.Visibility = autoCheckBoxVisibility;
            this.NumericUpDown.SmallChange = smallChange;
            this.NumericUpDown.Minimum = minimum;
            this.NumericUpDown.Maximum = maximum;
            this.NumericUpDown.DragSpeed = dragSpeed;
            this.NumericUpDown.ValueFormat = valueFormat;
            this.NumericUpDown.ValueBarVisibility = valueBarVisibility;
        }

        private void OnNumericUpDownValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var model = (BasePropertyViewModel)this.Model;
            AutoCheckBox.IsChecked = false;

            UpdateModelNumericValue(model);
        }

        private void UpdateModelNumericValue(BasePropertyViewModel model)
        {
            if (this.NumericType == typeof (byte))
            {
                model.Value = (byte)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (Int16))
            {
                model.Value = (Int16)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (UInt16))
            {
                model.Value = (UInt16)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (int))
            {
                model.Value = (int)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (uint))
            {
                model.Value = (uint)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (Int64))
            {
                model.Value = (Int64)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (UInt64))
            {
                model.Value = (UInt64)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (float))
            {
                model.Value = (float)this.NumericUpDown.Value;
            }
            else if (this.NumericType == typeof (double))
            {
                model.Value = this.NumericUpDown.Value;
            }
        }

        private void AutoCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            var model = (BasePropertyViewModel)this.Model;

            var dpm = model as DependencyPropertyViewModel;

            if (dpm != null)
            {
                var dp = dpm.DependencyProperty;

                if (dp == FrameworkElement.WidthProperty ||
                    dp == FrameworkElement.HeightProperty)
                {
                    model.Value = double.NaN;

                    return;
                }

                if (dp == FrameworkElement.MaxWidthProperty ||
                    dp == FrameworkElement.MaxHeightProperty)
                {
                    model.Value = double.PositiveInfinity;

                    return;
                }
            }
            
            if (this.NumericType == typeof(float))
            {
                model.Value = float.NaN;
            }
            else if (this.NumericType == typeof(double))
            {
                model.Value = double.NaN;
            }
        }

        private void AutoCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var model = (BasePropertyViewModel)this.Model;

            if (this.NumericType == typeof(float))
            {
                this.NumericUpDown.Value = 0;
                UpdateModelNumericValue(model);
            }
            else if (this.NumericType == typeof(double))
            {
                this.NumericUpDown.Value = 0;
                UpdateModelNumericValue(model);
            }
        }
    }
}
