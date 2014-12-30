using System.Collections.Generic;
using System.Linq;
using Windows.System;

namespace WinRTXamlToolkit.Input
{
    /// <summary>
    /// Specifies a combination of keys.
    /// </summary>
    public class KeyCombination : List<VirtualKey>
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Join("+", this.Select(vk => vk.ToString()));
        }
    }
}