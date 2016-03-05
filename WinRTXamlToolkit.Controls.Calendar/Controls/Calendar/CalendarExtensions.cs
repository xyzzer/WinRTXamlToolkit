// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Inherited code: Requires comment.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    internal static class CalendarExtensions
    {
        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        private static Dictionary<DependencyObject, Dictionary<DependencyProperty, bool>> _suspendedHandlers = new Dictionary<DependencyObject, Dictionary<DependencyProperty, bool>>();

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <param name="obj">Inherited code: Requires comment 1.</param>
        /// <param name="dependencyProperty">Inherited code: Requires comment 2.</param>
        /// <returns>Inherited code: Requires comment 3.</returns>
        public static bool IsHandlerSuspended(this DependencyObject obj, DependencyProperty dependencyProperty)
        {
            if (_suspendedHandlers.ContainsKey(obj))
            {
                return _suspendedHandlers[obj].ContainsKey(dependencyProperty);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <param name="obj">Inherited code: Requires comment 1.</param>
        /// <param name="property">Inherited code: Requires comment 2.</param>
        /// <param name="value">Inherited code: Requires comment 3.</param>
        public static void SetValueNoCallback(this DependencyObject obj, DependencyProperty property, object value)
        {
            obj.SuspendHandler(property, true);
            try
            {
                obj.SetValue(property, value);
            }
            finally
            {
                obj.SuspendHandler(property, false);
            }
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <param name="obj">Inherited code: Requires comment 1.</param>
        /// <param name="dependencyProperty">Inherited code: Requires comment 2.</param>
        /// <param name="suspend">Inherited code: Requires comment 3.</param>
        private static void SuspendHandler(this DependencyObject obj, DependencyProperty dependencyProperty, bool suspend)
        {
            if (_suspendedHandlers.ContainsKey(obj))
            {
                Dictionary<DependencyProperty, bool> suspensions = _suspendedHandlers[obj];

                if (suspend)
                {
                    Debug.Assert(!suspensions.ContainsKey(dependencyProperty), "Suspensions should not contain the property!");

                    // true = dummy value
                    suspensions[dependencyProperty] = true;
                }
                else
                {
                    Debug.Assert(suspensions.ContainsKey(dependencyProperty), "Suspensions should contain the property!");
                    suspensions.Remove(dependencyProperty);
                    if (suspensions.Count == 0)
                    {
                        _suspendedHandlers.Remove(obj);
                    }
                }
            }
            else
            {
                Debug.Assert(suspend, "suspend should be true!");
                _suspendedHandlers[obj] = new Dictionary<DependencyProperty, bool>();
                _suspendedHandlers[obj][dependencyProperty] = true;
            }
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <param name="ctrl">Inherited code: Requires comment 1.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Inherited mature control.")]
        public static void GetMetaKeyState(out bool ctrl)
        {
            //ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control) == CoreVirtualKeyStates.Down;
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <param name="ctrl">Inherited code: Requires comment 2.</param>
        /// <param name="shift">Inherited code: Requires comment 3.</param>
        public static void GetMetaKeyState(out bool ctrl, out bool shift)
        {
            //ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control) == CoreVirtualKeyStates.Down;
            //shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            shift = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) == CoreVirtualKeyStates.Down;
        }
    }
}