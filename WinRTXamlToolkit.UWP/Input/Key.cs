using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Input
{
    /// <summary>
    /// Defines a keyboard key.
    /// </summary>
    public static class Key
    {
        private static readonly Dictionary<string, VirtualKey> KeyMap;

        static Key()
        {
            KeyMap = new Dictionary<string, VirtualKey>();
            EnumExtensions.GetValues<VirtualKey>().Where(vk => !KeyMap.ContainsKey(vk.ToString().ToLower())).ForEach(vk => KeyMap.Add(vk.ToString().ToLower(), vk));
            KeyMap.Add("alt", VirtualKey.Menu);
            KeyMap.Add("ctrl", VirtualKey.Control);
            KeyMap.Add(" ", VirtualKey.Space);
            KeyMap.Add("+", VirtualKey.Add);
            KeyMap.Add("-", VirtualKey.Subtract);
            KeyMap.Add("/", VirtualKey.Divide);
            KeyMap.Add("*", VirtualKey.Multiply);
            KeyMap.Add(".", VirtualKey.Decimal);

            // OEM codes for American keyboards from http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
            KeyMap.Add("`", (VirtualKey)0xC0);
            KeyMap.Add("~", (VirtualKey)0xC0);
            KeyMap.Add(":", (VirtualKey)0xBA);
            KeyMap.Add(";", (VirtualKey)0xBA);
            KeyMap.Add(",", (VirtualKey)0xBC);
            KeyMap.Add("?", (VirtualKey)0xBF);
            KeyMap.Add("[", (VirtualKey)0xDB);
            KeyMap.Add("{", (VirtualKey)0xDB);
            KeyMap.Add("\\", (VirtualKey)0xDC);
            KeyMap.Add("|", (VirtualKey)0xDC);
            KeyMap.Add("]", (VirtualKey)0xDD);
            KeyMap.Add("}", (VirtualKey)0xDD);
            KeyMap.Add("'", (VirtualKey)0xDE);
            KeyMap.Add("\"", (VirtualKey)0xDE);
        }

        /// <summary>
        /// Parses a key from its name.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>A VirtualKey value on success. Throws <see cref="FormatException"/> otherwise.</returns>
        /// <exception cref="System.FormatException"></exception>
        public static VirtualKey Parse(string keyName)
        {
            VirtualKey key;

            if (!KeyMap.TryGetValue(keyName.ToLower(), out key))
            {
                throw new FormatException(string.Format("\"{0}\" is not a recognized key name. Check VirtualKey enumeration for known key names."));
            }

            return key;
        }
    }
}