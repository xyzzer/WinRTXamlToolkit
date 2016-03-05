// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Specifies values for the different modes of operation of a
    /// <see cref="T:WinRTXamlToolkit.Controls.Calendar" />.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    public enum CalendarMode
    {
        /// <summary>
        /// The <see cref="T:WinRTXamlToolkit.Controls.Calendar" /> displays a
        /// month at a time.
        /// </summary>
        Month = 0,

        /// <summary>
        /// The <see cref="T:WinRTXamlToolkit.Controls.Calendar" /> displays a
        /// year at a time.
        /// </summary>
        Year = 1,

        /// <summary>
        /// The <see cref="T:WinRTXamlToolkit.Controls.Calendar" /> displays a
        /// decade at a time.
        /// </summary>
        Decade = 2,
    }
}