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
        public override string ToString()
        {
            return string.Join("+", this.Select(vk => vk.ToString()));
        }
    }
}