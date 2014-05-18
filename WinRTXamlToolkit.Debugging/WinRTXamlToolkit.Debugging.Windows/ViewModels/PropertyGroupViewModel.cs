using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WinRTXamlToolkit.Debugging.Commands;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class PropertyGroupViewModel : BindableBase
    {
        private ObservableCollection<BindableBase> _parentList;
        public ObservableCollection<BindableBase> ParentList
        {
            get
            {
                return _parentList;
            }
            set
            {
                if (_parentList == value)
                {
                    return;
                }

                _parentList = value;

                if (_isExpanded == true)
                {
                    Expand();
                }
                else
                {
                    Collapse();
                }
            }
        }

        private readonly List<BasePropertyViewModel> _properties;
        public string Category { get; private set; }

        #region IsExpanded
        private bool? _isExpanded = false;

        public bool? IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (this.SetProperty(ref _isExpanded, value))
                {
                    if ((bool)value)
                    {
                        Expand();
                    }
                    else
                    {
                        Collapse();
                    }
                }
            }
        }

        private void Collapse()
        {
            this.IsExpanded = false;

            if (this.ParentList == null)
            {
                return;
            }

            foreach (var sampleButtonViewModel in _properties)
            {
                this.ParentList.Remove(sampleButtonViewModel);
            }
        }

        private void Expand()
        {
            this.IsExpanded = true;

            if (this.ParentList == null)
            {
                return;
            }

            var groupHeaderIndex = this.ParentList.IndexOf(this);
            var insertIndex = groupHeaderIndex + 1;

            for (int i = 0; i < _properties.Count; i++)
            {
                this.ParentList.Insert(insertIndex, _properties[i]);
                insertIndex++;
            }
        }
        #endregion

        public PropertyGroupViewModel(
            string category,
            IEnumerable<BasePropertyViewModel> properties)
        {
            this.Category = category;
            _properties = properties.ToList();
        }
    }
}
