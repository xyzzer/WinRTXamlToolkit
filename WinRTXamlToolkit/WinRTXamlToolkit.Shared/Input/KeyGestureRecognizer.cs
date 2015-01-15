using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Input
{
    public static class VirtualKeyExtensions
    {
        public static bool IsModifier(this VirtualKey key)
        {
            return
                key == VirtualKey.Control ||
                key == VirtualKey.Menu ||
                key == VirtualKey.Shift ||
                key == VirtualKey.LeftControl ||
                key == VirtualKey.LeftMenu ||
                key == VirtualKey.LeftShift ||
                key == VirtualKey.RightControl ||
                key == VirtualKey.RightMenu ||
                key == VirtualKey.RightShift ||
                key == VirtualKey.LeftWindows ||
                key == VirtualKey.RightWindows ||
                key == VirtualKey.Application;
        }
    }

    public class KeyGestureRecognizer : IDisposable
    {
        private Window window;
        private KeyGesture gesture;
        private int combinationsMatched;

        #region GestureRecognized event
        /// <summary>
        /// GestureRecognized event property.
        /// </summary>
        public event EventHandler GestureRecognized;

        /// <summary>
        /// Raises GestureRecognized event.
        /// </summary>
        private void RaiseGestureRecognized()
        {
            var handler = this.GestureRecognized;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGestureRecognizer"/> class.
        /// </summary>
        /// <param name="gesture">The gesture to recognized.</param>
        /// <exception cref="System.ArgumentNullException">gesture</exception>
        /// <exception cref="System.ArgumentException">The gesture needs to consist of at least one key or key combination.;gesture</exception>
        public KeyGestureRecognizer(KeyGesture gesture)
        {
            if (gesture == null)
            {
                throw new ArgumentNullException("gesture");
            }

            if (gesture.Count == 0)
            {
                throw new ArgumentException("The gesture needs to consist of at least one key or key combination.", "gesture");
            }

            this.gesture = gesture;
            this.window = Window.Current;
            this.window.CoreWindow.KeyDown += this.CoreWindowOnKeyDown;
        }

        public void Dispose()
        {
            if (!this.window.Dispatcher.HasThreadAccess)
            {
                this.window.Dispatcher.RunAsync(
                    CoreDispatcherPriority.High,
                    Dispose);
                return;
            }

            this.window.CoreWindow.KeyDown -= this.CoreWindowOnKeyDown;
            this.window = null;
            this.gesture = null;
        }

        enum MatchKind
        {
            Mismatch,
            Incomplete,
            Match
        }

        private MatchKind CheckKeyCombination(KeyCombination combination, VirtualKey keyAdded, KeyCombination precedingCombination = null)
        {
            // A key gesture is defined by either explicit modifier keys in both combinations
            // or no modifiers specified for second combination,
            // but then the second combination works with either no modifiers or same modifiers as the first.

            // A gesture is rejected if a combination with unrecognized non-modifier key is used.
            var downState = CoreVirtualKeyStates.Down;
            var ctrl = (this.window.CoreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
            var alt = (this.window.CoreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
            var shift = (this.window.CoreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;

            if (!keyAdded.IsModifier())
            {
                if (combination.Contains(keyAdded))
                {
                    foreach (var key in combination)
                    {
                        if (key == keyAdded)
                        {
                            continue;
                        }


                        if ((this.window.CoreWindow.GetKeyState(key) & downState) != downState)
                        {
                            // Missing some modifier key
                            return MatchKind.Mismatch;
                        }
                    }

                    // All the keys matched!

                    // Reject if found additional modifiers pressed
                    if (ctrl &&
                        (!combination.Contains(VirtualKey.Control) &&
                        (precedingCombination == null || !precedingCombination.Contains(VirtualKey.Control))) ||
                        alt &&
                        (!combination.Contains(VirtualKey.Menu) ||
                        (precedingCombination == null || !precedingCombination.Contains(VirtualKey.Menu))) ||
                        shift &&
                        (!combination.Contains(VirtualKey.Shift) ||
                        (precedingCombination == null || !precedingCombination.Contains(VirtualKey.Shift))))
                    {
                        return MatchKind.Mismatch;
                    }

                    return MatchKind.Match;
                }
                else
                {
                    // An invalid non-modifier key was pressed
                    return MatchKind.Mismatch;
                }
            }
            else
            {
                // Only recognizing combinations when a non-modifier key is pressed
                return MatchKind.Incomplete;
            }
        }

        private void CoreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            KeyCombination precedingCombination = null;

            if (this.combinationsMatched > 0)
            {
                precedingCombination = this.gesture[0];
            }

            switch (CheckKeyCombination(this.gesture[this.combinationsMatched], args.VirtualKey, precedingCombination))
            {
                case MatchKind.Incomplete:
                    break;
                case MatchKind.Mismatch:
                    this.combinationsMatched = 0;
                    break;
                case MatchKind.Match:
                    this.combinationsMatched++;

                    if (this.combinationsMatched == this.gesture.Count)
                    {
                        this.RaiseGestureRecognized();
                        this.combinationsMatched = 0;
                    }

                    break;
            }
        }
    }
}
