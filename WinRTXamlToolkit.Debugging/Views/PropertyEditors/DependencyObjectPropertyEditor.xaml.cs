using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Debugging.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class DependencyObjectPropertyEditor : UserControl
    {
        //#region Model
        ///// <summary>
        ///// Model Dependency Property
        ///// </summary>
        //public static readonly DependencyProperty ModelProperty =
        //    DependencyProperty.Register(
        //        "Model",
        //        typeof(object),
        //        typeof(DependencyObjectPropertyEditor),
        //        new PropertyMetadata(null, OnModelChanged));

        ///// <summary>
        ///// Gets or sets the Model property. This dependency property 
        ///// indicates the property view model to update with the values.
        ///// </summary>
        //public object Model
        //{
        //    get { return (object)GetValue(ModelProperty); }
        //    set { SetValue(ModelProperty, value); }
        //}

        ///// <summary>
        ///// Handles changes to the Model property.
        ///// </summary>
        ///// <param name="d">
        ///// The <see cref="DependencyObject"/> on which
        ///// the property has changed value.
        ///// </param>
        ///// <param name="e">
        ///// Event data that is issued by any event that
        ///// tracks changes to the effective value of this property.
        ///// </param>
        //private static void OnModelChanged(
        //    DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var target = (DependencyObjectPropertyEditor)d;
        //    BasePropertyViewModel oldModel = (BasePropertyViewModel)e.OldValue;
        //    BasePropertyViewModel newModel = (BasePropertyViewModel)target.Model;
        //    target.OnModelChanged(oldModel, newModel);
        //}

        ///// <summary>
        ///// Provides derived classes an opportunity to handle changes
        ///// to the Model property.
        ///// </summary>
        ///// <param name="oldModel">The old Model value</param>
        ///// <param name="newModel">The new Model value</param>
        //private void OnModelChanged(
        //    BasePropertyViewModel oldModel, BasePropertyViewModel newModel)
        //{
        //}
        //#endregion

        public DependencyObjectPropertyEditor()
        {
            this.InitializeComponent();
        }
    }
}
