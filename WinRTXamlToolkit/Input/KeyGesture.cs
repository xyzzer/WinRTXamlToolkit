using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Input
{
    public class KeyGesture : List<KeyCombination>
    {
        private enum ParserStates
        {
            Key,
            Combine,
            Separator
        }

        public override string ToString()
        {
            return string.Join(",", this.Select(c => c.ToString()));
        }

        public static KeyGesture Parse(string keyGestureString)
        {
            if (keyGestureString == null)
            {
                throw new ArgumentNullException("keyGestureString", "Key gesture string not specified");
            }

            keyGestureString = keyGestureString.Trim();

            if (keyGestureString.Length == 0)
            {
                throw new FormatException("Key gesture string empty");
            }

            var gesture = new KeyGesture();

            try
            {
                var state = ParserStates.Key;
                KeyCombination combination = null;
                int start = 0;

                for (int i = 0; i < keyGestureString.Length; i++)
                {
                    var c = keyGestureString[i];

                    if (c == '+')
                    {
                        if (combination == null)
                        {
                            combination = new KeyCombination();
                        }

                        combination.Add(Key.Parse(keyGestureString.Substring(start, i - start)));

                        start = i + 1;
                    }
                    else if (c == ',')
                    {
                        if (combination == null)
                        {
                            combination = new KeyCombination();
                        }

                        combination.Add(Key.Parse(keyGestureString.Substring(start, i - start)));

                        start = i + 1;

                        gesture.Add(combination);
                        combination = null;
                    }
                }

                if (start < keyGestureString.Length)
                {
                    if (combination == null)
                    {
                        combination = new KeyCombination();
                    }

                    combination.Add(Key.Parse(keyGestureString.Substring(start)));
                    gesture.Add(combination);
                }
            }
            catch (Exception ex)
            {
                throw new FormatException(string.Format("Key gesture string \"{0}\" not recognized", keyGestureString), ex);
            }

            return gesture;
        }
    }
}