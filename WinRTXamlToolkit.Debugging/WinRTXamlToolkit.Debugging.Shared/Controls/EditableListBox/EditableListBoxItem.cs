using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using WinRTXamlToolkit.Controls.Common;

namespace WinRTXamlToolkit.Debugging.Controls
{
    public class EditableListBoxItem : ListBoxItem
    {
        private bool _isEditable;
        private bool _changingTemplates;

        #region EditableContentTemplate
        /// <summary>
        /// EditableContentTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty EditableContentTemplateProperty =
            DependencyProperty.Register(
                "EditableContentTemplate",
                typeof(DataTemplate),
                typeof(EditableListBoxItem),
                new PropertyMetadata(null, OnEditableContentTemplateChanged));

        /// <summary>
        /// Gets or sets the EditableContentTemplate property. This dependency property 
        /// indicates the template to use for the control when it is selected.
        /// </summary>
        public DataTemplate EditableContentTemplate
        {
            get { return (DataTemplate)GetValue(EditableContentTemplateProperty); }
            set { SetValue(EditableContentTemplateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the EditableContentTemplate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnEditableContentTemplateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EditableListBoxItem)d;
            DataTemplate oldEditableContentTemplate = (DataTemplate)e.OldValue;
            DataTemplate newEditableContentTemplate = target.EditableContentTemplate;
            target.OnEditableContentTemplateChanged(oldEditableContentTemplate, newEditableContentTemplate);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the EditableContentTemplate property.
        /// </summary>
        /// <param name="oldEditableContentTemplate">The old EditableContentTemplate value</param>
        /// <param name="newEditableContentTemplate">The new EditableContentTemplate value</param>
        private void OnEditableContentTemplateChanged(
            DataTemplate oldEditableContentTemplate, DataTemplate newEditableContentTemplate)
        {
            if (this.IsSelected)
            {
                _changingTemplates = true;
                this.ContentTemplate = newEditableContentTemplate;
                _changingTemplates = false;
            }

            _isEditable = true;
        }
        #endregion

        #region EditableContentTemplateSelector
        /// <summary>
        /// EditableContentTemplateSelector Dependency Property
        /// </summary>
        public static readonly DependencyProperty EditableContentTemplateSelectorProperty =
            DependencyProperty.Register(
                "EditableContentTemplateSelector",
                typeof(DataTemplateSelector),
                typeof(EditableListBoxItem),
                new PropertyMetadata(null, OnEditableContentTemplateSelectorChanged));

        /// <summary>
        /// Gets or sets the EditableContentTemplateSelector property. This dependency property 
        /// indicates the DataTemplateSelector to use when the control is selected.
        /// </summary>
        public DataTemplateSelector EditableContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(EditableContentTemplateSelectorProperty); }
            set { SetValue(EditableContentTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the EditableContentTemplateSelector property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnEditableContentTemplateSelectorChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (EditableListBoxItem)d;
            DataTemplateSelector oldEditableContentTemplateSelector = (DataTemplateSelector)e.OldValue;
            DataTemplateSelector newEditableContentTemplateSelector = target.EditableContentTemplateSelector;
            target.OnEditableContentTemplateSelectorChanged(oldEditableContentTemplateSelector, newEditableContentTemplateSelector);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the EditableContentTemplateSelector property.
        /// </summary>
        /// <param name="oldEditableContentTemplateSelector">The old EditableContentTemplateSelector value</param>
        /// <param name="newEditableContentTemplateSelector">The new EditableContentTemplateSelector value</param>
        private void OnEditableContentTemplateSelectorChanged(
            DataTemplateSelector oldEditableContentTemplateSelector, DataTemplateSelector newEditableContentTemplateSelector)
        {
            if (this.IsSelected)
            {
                _changingTemplates = true;
                this.ContentTemplateSelector = newEditableContentTemplateSelector;
                _changingTemplates = false;
            }

            _isEditable = true;
        }
        #endregion

        #region SlimContentTemplate
        /// <summary>
        /// SlimContentTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty SlimContentTemplateProperty =
            DependencyProperty.Register(
                "SlimContentTemplate",
                typeof(DataTemplate),
                typeof(EditableListBoxItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the SlimContentTemplate property. This dependency property 
        /// indicates the default content template to use when the item is not selected.
        /// </summary>
        public DataTemplate SlimContentTemplate
        {
            get { return (DataTemplate)GetValue(SlimContentTemplateProperty); }
            set { SetValue(SlimContentTemplateProperty, value); }
        }
        #endregion

        #region SlimContentTemplateSelector
        /// <summary>
        /// SlimContentTemplateSelector Dependency Property
        /// </summary>
        public static readonly DependencyProperty SlimContentTemplateSelectorProperty =
            DependencyProperty.Register(
                "SlimContentTemplateSelector",
                typeof(DataTemplateSelector),
                typeof(EditableListBoxItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the SlimContentTemplateSelector property. This dependency property 
        /// indicates the default template selector to use when the item is not selected.
        /// </summary>
        public DataTemplateSelector SlimContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SlimContentTemplateSelectorProperty); }
            set { SetValue(SlimContentTemplateSelectorProperty, value); }
        }
        #endregion

        private readonly PropertyChangeEventSource<bool> _isSelectedChangeHandler;
        //private readonly PropertyChangeEventSource<DataTemplate> _contentTemplateChangeHandler;
        //private readonly PropertyChangeEventSource<DataTemplateSelector> _contentTemplateSelectorChangeHandler;

        public EditableListBoxItem()
        {
            DefaultStyleKey = typeof (EditableListBoxItem);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
            _isSelectedChangeHandler =
                new PropertyChangeEventSource<bool>(
                    this,
                    "IsSelected",
                    BindingMode.OneWay);
            //_contentTemplateChangeHandler =
            //    new PropertyChangeEventSource<DataTemplate>(
            //        this,
            //        "ContentTemplate",
            //        BindingMode.OneWay);
            //_contentTemplateSelectorChangeHandler =
            //    new PropertyChangeEventSource<DataTemplateSelector>(
            //        this,
            //        "ContentTemplateSelector",
            //        BindingMode.OneWay);
        }

        protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
        {
            base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);

            if (!_changingTemplates)
            {
                SlimContentTemplate = this.ContentTemplate;
            }
        }

        protected override void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
        {
            base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);

            if (!_changingTemplates)
            {
                this.SlimContentTemplateSelector = this.ContentTemplateSelector;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _isSelectedChangeHandler.ValueChanged += OnIsSelectedChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _isSelectedChangeHandler.ValueChanged -= OnIsSelectedChanged;
        }


        private void OnIsSelectedChanged(object sender, bool isSelected)
        {
            if (!_isEditable)
            {
                return;
            }

            _changingTemplates = true;

            if (isSelected)
            {
                this.ContentTemplate = this.EditableContentTemplate;
                this.ContentTemplateSelector = this.EditableContentTemplateSelector;
            }
            else
            {
                this.ContentTemplate = this.SlimContentTemplate;
                this.ContentTemplateSelector = this.SlimContentTemplateSelector;
            }

            _changingTemplates = false;
        }
    }
}
