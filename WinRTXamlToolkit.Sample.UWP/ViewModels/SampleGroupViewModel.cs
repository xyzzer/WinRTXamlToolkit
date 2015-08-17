using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WinRTXamlToolkit.Sample.Commands;

namespace WinRTXamlToolkit.Sample.ViewModels
{
    public class SampleGroupViewModel : ButtonViewModel
    {
        private readonly SampleTypes _sampleType;
        private readonly List<ButtonViewModel> _samplesList;
        private bool _isExpanded;
        public ObservableCollection<ButtonViewModel> ParentList { get; set; }

        public SampleTypes SampleType
        {
            get
            {
                return _sampleType;
            }
        }

        public SampleGroupViewModel(
            SampleTypes sampleType,
            IEnumerable<ButtonViewModel> samplesList)
        {
            _sampleType = sampleType;
            _samplesList = samplesList.ToList();

            switch (sampleType)
            {
                case SampleTypes.AwaitableUI:
                    this.Caption = "Awaitable UI";
                    break;
                case SampleTypes.Controls:
                    this.Caption = "Controls";
                    break;
                case SampleTypes.Debugging:
                    this.Caption = "Debugging";
                    break;
                case SampleTypes.Extensions:
                    this.Caption = "Control Extensions";
                    break;
                case SampleTypes.Imaging:
                    this.Caption = "Imaging helpers";
                    break;
                case SampleTypes.Miscellaneous:
                    this.Caption = "Miscellaneous";
                    break;
                default:
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    this.Caption = _sampleType.ToString();
                    break;
            }
            
            this.Command = new RelayCommand(ToggleIsExpanded);;
        }

        private void ToggleIsExpanded()
        {
            _isExpanded = !_isExpanded;

            if (_isExpanded)
            {
                var groupHeaderIndex = this.ParentList.IndexOf(this);
                var insertIndex = groupHeaderIndex + 1;

                for (int i = 0; i < _samplesList.Count; i++)
                {
                    this.ParentList.Insert(insertIndex, _samplesList[i]);
                    insertIndex++;
                }
            }
            else
            {
                foreach (var sampleButtonViewModel in _samplesList)
                {
                    this.ParentList.Remove(sampleButtonViewModel);
                }
            }
        }
    }
}