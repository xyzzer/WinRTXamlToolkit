using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Debugging.Controls
{
    public class EditableListBox : ListBox
    {
        #region EditableItemTemplate
        /// <summary>
        /// EditableItemTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty EditableItemTemplateProperty =
            DependencyProperty.Register(
                "EditableItemTemplate",
                typeof(DataTemplate),
                typeof(EditableListBox),
                new PropertyMetadata(null, OnEditableItemTemplateChanged));

        /// <summary>
        /// Gets or sets the EditableItemTemplate property. This dependency property 
        /// indicates the data template to use for selected items.
        /// </summary>
        public DataTemplate EditableItemTemplate
        {
            get { return (DataTemplate)GetValue(EditableItemTemplateProperty); }
            set { SetValue(EditableItemTemplateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the EditableItemTemplate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnEditableItemTemplateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EditableListBox)d;
            DataTemplate oldEditableItemTemplate = (DataTemplate)e.OldValue;
            DataTemplate newEditableItemTemplate = target.EditableItemTemplate;
            target.OnEditableItemTemplateChanged(oldEditableItemTemplate, newEditableItemTemplate);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the EditableItemTemplate property.
        /// </summary>
        /// <param name="oldEditableItemTemplate">The old EditableItemTemplate value</param>
        /// <param name="newEditableItemTemplate">The new EditableItemTemplate value</param>
        private void OnEditableItemTemplateChanged(
            DataTemplate oldEditableItemTemplate, DataTemplate newEditableItemTemplate)
        {
        }
        #endregion

        #region EditableItemTemplateSelector
        /// <summary>
        /// EditableItemTemplateSelector Dependency Property
        /// </summary>
        public static readonly DependencyProperty EditableItemTemplateSelectorProperty =
            DependencyProperty.Register(
                "EditableItemTemplateSelector",
                typeof(DataTemplateSelector),
                typeof(EditableListBox),
                new PropertyMetadata(null, OnEditableItemTemplateSelectorChanged));

        /// <summary>
        /// Gets or sets the EditableItemTemplateSelector property. This dependency property 
        /// indicates the data template selector to use for selected items.
        /// </summary>
        public DataTemplateSelector EditableItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(EditableItemTemplateSelectorProperty); }
            set { SetValue(EditableItemTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the EditableItemTemplateSelector property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnEditableItemTemplateSelectorChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EditableListBox)d;
            DataTemplateSelector oldEditableItemTemplateSelector = (DataTemplateSelector)e.OldValue;
            DataTemplateSelector newEditableItemTemplateSelector = target.EditableItemTemplateSelector;
            target.OnEditableItemTemplateSelectorChanged(oldEditableItemTemplateSelector, newEditableItemTemplateSelector);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the EditableItemTemplateSelector property.
        /// </summary>
        /// <param name="oldEditableItemTemplateSelector">The old EditableItemTemplateSelector value</param>
        /// <param name="newEditableItemTemplateSelector">The new EditableItemTemplateSelector value</param>
        private void OnEditableItemTemplateSelectorChanged(
            DataTemplateSelector oldEditableItemTemplateSelector, DataTemplateSelector newEditableItemTemplateSelector)
        {
        }
        #endregion
        
        public EditableListBox()
        {
            DefaultStyleKey = typeof(EditableListBox);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is EditableListBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new EditableListBoxItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var editableListBoxItem = (EditableListBoxItem)element;

            if (this.EditableItemTemplate != null &&
                !editableListBoxItem.HasNonDefaultValue(
                    EditableListBoxItem.EditableContentTemplateProperty))
            {
                editableListBoxItem.SetBinding(
                    EditableListBoxItem.EditableContentTemplateProperty,
                    new Binding
                    {
                        Path = new PropertyPath("EditableItemTemplate"),
                        Source = this
                    });
            }

            if (this.EditableItemTemplateSelector != null &&
                !editableListBoxItem.HasNonDefaultValue(
                    EditableListBoxItem.EditableContentTemplateSelectorProperty))
            {
                //editableListBoxItem.EditableContentTemplateSelector =
                //    this.EditableItemTemplateSelector;
                editableListBoxItem.SetBinding(
                    EditableListBoxItem.EditableContentTemplateSelectorProperty,
                    new Binding
                    {
                        Path = new PropertyPath("EditableItemTemplateSelector"),
                        Source = this
                    });
            }

            if (this.ItemTemplate != null &&
                !editableListBoxItem.HasNonDefaultValue(
                    EditableListBoxItem.SlimContentTemplateProperty))
            {
                //editableListBoxItem.SlimContentTemplate = this.ItemTemplate;
                editableListBoxItem.SetBinding(
                    EditableListBoxItem.SlimContentTemplateProperty,
                    new Binding
                    {
                        Path = new PropertyPath("ItemTemplate"),
                        Source = this
                    });
            }

            if (this.ItemTemplateSelector != null &&
                !editableListBoxItem.HasNonDefaultValue(
                    EditableListBoxItem.SlimContentTemplateSelectorProperty))
            {
                //editableListBoxItem.SlimContentTemplateSelector =
                //    this.ItemTemplateSelector;
                editableListBoxItem.SetBinding(
                    EditableListBoxItem.SlimContentTemplateSelectorProperty,
                    new Binding
                    {
                        Path = new PropertyPath("ItemTemplateSelector"),
                        Source = this
                    });
            }
            //var contentControl = element as ContentControl;

            //if (contentControl != null)
            //{
            //    contentControl.PrepareContentControl(item, this.ItemTemplate, this.ItemTemplateSelector);

            //    var editableListBoxItem = contentControl as EditableListBoxItem;

            //    if (editableListBoxItem != null &&
            //        this.EditableItemTemplate != null &&
            //        !editableListBoxItem.HasNonDefaultValue(
            //            EditableListBoxItem.EditableContentTemplateProperty))
            //    {
            //        editableListBoxItem.SetBinding(
            //            EditableListBoxItem.EditableContentTemplateProperty,
            //            new Binding
            //            {
            //                Path = new PropertyPath("EditableItemTemplate"),
            //                Source = this
            //            });
            //    }

            //    if (editableListBoxItem != null &&
            //        this.EditableItemTemplateSelector != null &&
            //        !editableListBoxItem.HasNonDefaultValue(
            //            EditableListBoxItem.EditableContentTemplateSelectorProperty))
            //    {
            //        editableListBoxItem.SetBinding(
            //            EditableListBoxItem.EditableContentTemplateSelectorProperty,
            //            new Binding
            //            {
            //                Path = new PropertyPath("EditableItemTemplateSelector"),
            //                Source = this
            //            });
            //    }
            //}
            //else
            //{
            //    base.PrepareContainerForItemOverride(element, item);
            //}
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            //var contentControl = element as ContentControl;

            //if (contentControl != null)
            //{
            //    contentControl.ClearContentControl(item);
            //}
            //else
            //{
            //    base.ClearContainerForItemOverride(element, item);
            //}
        }
    }
}
