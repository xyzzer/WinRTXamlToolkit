// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.DataVisualization
{
    /// <summary>
    /// Represents the title of a data visualization control.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public partial class Title : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the Title class.
        /// </summary>
        public Title()
        {
            DefaultStyleKey = typeof(Title);
        }
    }
}