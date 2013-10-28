using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.System;

namespace WinRTXamlToolkit.Input
{
    public class KeyCommand : ICommand
    {
        public string KeyGestureString { get; set; }

        public string KeyGesture { get; set; }

        public bool IsFocusRequired { get; set; }

        #region Invoked event
        public event EventHandler Invoked;

        private void RaiseInvoked()
        {
            var handler = this.Invoked;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        } 
        #endregion

        #region ICommand implementation
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged; 
        #endregion
    }

    public class KeyCombination : List<VirtualKey>
    {
    }

    public class KeyGesture : List<KeyCombination>
    {
        private enum ParserStates
        {
            Key,
            Combine,
            Separator
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

                    switch (state)
                    {
                        case ParserStates.Key:
                            if (c == '+')
                            {
                                if (combination == null)
                                {
                                    combination = new KeyCombination();
                                }

                                combination.Add(Key.Parse(keyGestureString.Substring(start, i - start)));

                                start = i + 1;
                                state = ParserStates.Combine;
                            }
                            else if (c == ',')
                            {
                                gesture.Add(combination);
                                combination = null;
                                state = ParserStates.Separator;
                            }
                            break;
                        case ParserStates.Combine:
                            state = ParserStates.Key;
                            break;
                        case ParserStates.Separator:
                            state = ParserStates.Key;
                            break;
                    }
                }

                if (state == ParserStates.Key)
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