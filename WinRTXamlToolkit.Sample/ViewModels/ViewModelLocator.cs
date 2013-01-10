using System.ComponentModel;
using System.Diagnostics;

namespace WinRTXamlToolkit.Sample.ViewModels
{
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; }

        #region MainPage
        private MainPageViewModel _mainPage;
        public MainPageViewModel MainPage
        {
            get
            {
                return _mainPage ?? (_mainPage = new MainPageViewModel());
            }
        }
        #endregion

        public ViewModelLocator()
        {
            Debug.Assert(Instance == null);

            Instance = this;
        }
    }
}
