using System.Collections.ObjectModel;
using System.Linq;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    /// <summary>
    /// Specifies the list of properties to display.
    /// </summary>
    /// <seealso cref="WinRTXamlToolkit.Debugging.ViewModels.BindableBase" />
    public class PropertyList : BindableBase
    {
        #region Name
        private string _name;
        /// <summary>
        /// Gets or sets the name of the property list.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref _name, value); }
        }
        #endregion

        #region CommaSeparatedPropertyNames
        private string _commaSeparatedPropertyNames;
        /// <summary>
        /// Gets or sets the comma separated property names.
        /// </summary>
        public string CommaSeparatedPropertyNames
        {
            get { return _commaSeparatedPropertyNames; }
            set
            {
                if (this.SetProperty(ref _commaSeparatedPropertyNames, value))
                {
                    this.PropertyNames = new ObservableCollection<string>(_commaSeparatedPropertyNames.Split(',').Select(pn => pn.Trim()).Where(pn => !string.IsNullOrEmpty(pn)));
                    this.OnPropertyChanged(nameof(this.PropertyNames));
                }
            }
        }
        #endregion

        public ObservableCollection<string> PropertyNames { get; private set; }
    }
}