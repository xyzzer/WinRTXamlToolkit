using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WinRTXamlToolkit.Debugging.Commands;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public abstract class BasePropertyViewModel : BindableBase
    {
        public DependencyObjectViewModel ElementModel { get; private set; }

        public const string AppearanceCategoryName = "Appearance";
        public const string BrushCategoryName = "Brush";
        public const string CommonCategoryName = "Common";
        public const string InteractionsCategoryName = "Interactions";
        public const string LayoutCategoryName = "Layout";
        public const string MiscCategoryName = "Miscellaneous";
        public const string TextCategoryName = "Text";
        public const string TransformCategoryName = "Transform";
        public const string WinRTXamlToolkitExtensionsCategoryName = "Toolkit Extensions";
        public const string WinRTXamlToolkitControlCategoryName = "Toolkit Control";
        public const string WinRTXamlToolkitDebuggingCategoryName = "Debugging";

        protected bool? _isDefault;

        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            protected set { this.SetProperty(ref _name, value); }
        }
        #endregion

        public virtual string ValueString
        {
            get { return (this.Value ?? "<null>").ToString(); }
        }

        public abstract object Value { get; set; }

        public abstract Type PropertyType { get; }

        public abstract string Category { get; }

        public abstract bool IsDefault { get; }

        public abstract bool IsReadOnly { get; }

        public abstract bool CanResetValue { get; }

        public abstract void ResetValue();

        public RelayCommand ResetValueCommand { get; private set; }

        public abstract bool CanAnalyze { get; }

        public abstract void Analyze();

        public RelayCommand AnalyzeCommand { get; private set; }

        #region CanFindSimilar
        public bool CanFindSimilar
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region FindSimilar()
        public async void FindSimilar()
        {
            var elements = new List<object>();
            var baseValue = this.Value;

            foreach (var root in DebugConsoleViewModel.Instance.VisualTreeView.RootElements)
            {
                await this.AddSimilarElementsAsync(elements, baseValue, root);
            }

            var vm = new ElementListToolWindowViewModel(elements, this.Name + " == " + this.ValueString);
            DebugConsoleViewModel.Instance.ToolWindows.Add(vm);
        }

        private async Task AddSimilarElementsAsync(List<object> elements, object baseValue, TreeItemViewModel item)
        {
            var dobvm = item as DependencyObjectViewModel;

            if (dobvm != null)
            {
                object itemValue;

                if (TryGetValue(dobvm.Model, out itemValue) &&
                    object.Equals(itemValue, baseValue))
                {
                    elements.Add(item);
                }
            }

            if (item.Children == null ||
                item.Children.Count == 0 ||
                (item.Children.Count == 1 && item.Children[0] is StubTreeItemViewModel))
            {
                await item.LoadChildrenAsync();
            }

            if (item.Children == null)
            {
                return;
            }

            foreach (var child in item.Children)
            {
                await this.AddSimilarElementsAsync(elements, baseValue, child);
            }
        }
        #endregion

        public RelayCommand FindSimilarCommand { get; private set; }

        public BasePropertyViewModel(DependencyObjectViewModel elementModel)
        {
            this.ElementModel = elementModel;
            this.ResetValueCommand = new RelayCommand(
                this.ResetValue,
                () => this.CanResetValue);
            this.AnalyzeCommand = new RelayCommand(
                this.Analyze,
                () => this.CanAnalyze);
            this.FindSimilarCommand = new RelayCommand(
                this.FindSimilar,
                () => this.CanFindSimilar);
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CanResetValue")
            {
                this.ResetValueCommand.RaiseCanExecuteChanged();
            }
        }

        public void Refresh()
        {
            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Value");
            OnPropertyChanged("ValueString");
            OnPropertyChanged("IsDefault");
            // ReSharper restore ExplicitCallerInfoArgument
        }

        /// <summary>
        /// Gets the value of the property on the specified object.
        /// </summary>
        public abstract bool TryGetValue(object model, out object value);
    }
}