using System;
using System.Collections.Generic;
using System.Linq;
using WinRTXamlToolkit.Tools;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods for DependencyObjects
    /// used for walking the visual tree with
    /// LINQ expressions.
    /// These simplify using VisualTreeHelper to one line calls.
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        /// <summary>
        /// Gets the window root that is the top level ascendant of the window.Content.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns></returns>
        public static UIElement GetRealWindowRoot(Window window = null)
        {
            if (window == null)
            {
                window = Window.Current;
            }

            if (window == null)
            {
                return null;
            }

            var root = window.Content as FrameworkElement;

            if (root != null)
            {
                var ancestors = root.GetAncestors().ToList();

                if (ancestors.Count > 0)
                {
                    root = (FrameworkElement)ancestors[ancestors.Count - 1];
                }
            }

            return root;
        }

        /// <summary>
        /// Gets the first descendant that is of the given type.
        /// </summary>
        /// <remarks>
        /// Returns null if not found.
        /// </remarks>
        /// <typeparam name="T">Type of descendant to look for.</typeparam>
        /// <param name="start">The start object.</param>
        /// <returns></returns>
        public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendantsOfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the descendants of the given type.
        /// </summary>
        /// <typeparam name="T">Type of descendants to return.</typeparam>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendants().OfType<T>();
        }

        /// <summary>
        /// Gets the descendants.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
        {
            if (start == null)
            {
                yield break;
            }

            var queue = new Queue<DependencyObject>();

            var popup = start as Popup;

            if (popup != null)
            {
                if (popup.Child != null)
                {
                    queue.Enqueue(popup.Child);
                    yield return popup.Child;
                }
            }
            else
            {
                var count = VisualTreeHelper.GetChildrenCount(start);

                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(start, i);
                    queue.Enqueue(child);
                    yield return child;
                }
            }

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();

                popup = parent as Popup;

                if (popup != null)
                {
                    if (popup.Child != null)
                    {
                        queue.Enqueue(popup.Child);
                        yield return popup.Child;
                    }
                }
                else
                {
                    var count = VisualTreeHelper.GetChildrenCount(parent);

                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(parent, i);
                        yield return child;
                        queue.Enqueue(child);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the child elements.
        /// </summary>
        /// <param name="parent">The parent element.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent)
        {
            var popup = parent as Popup;

            if (popup != null)
            {
                if (popup.Child != null)
                {
                    yield return popup.Child;
                    yield break;
                }
            }

            var count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
            }
        }

        /// <summary>
        /// Gets the child elements sorted in render order (by ZIndex first, declaration order second).
        /// </summary>
        /// <param name="parent">The parent element.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetChildrenByZIndex(
            this DependencyObject parent)
        {
            int i = 0;
            var indexedChildren =
                parent.GetChildren().Cast<FrameworkElement>().Select(
                child => new { Index = i++, ZIndex = Canvas.GetZIndex(child), Child = child });

            return
                from indexedChild in indexedChildren
                orderby indexedChild.ZIndex, indexedChild.Index
                select indexedChild.Child;
        }

        /// <summary>
        /// Gets the first ancestor that is of the given type.
        /// </summary>
        /// <remarks>
        /// Returns null if not found.
        /// </remarks>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static T GetFirstAncestorOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetAncestorsOfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the ancestors of a given type, starting with parent and going towards the visual tree root.
        /// </summary>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <param name="start">The start.</param>
        /// <returns>The ancestors of a given type, starting with parent and going towards the visual tree root.</returns>
        public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetAncestors().OfType<T>();
        }

        /// <summary>
        /// Gets the ancestors, starting with parent and going towards the visual tree root.
        /// </summary>
        /// <param name="start">The starting element.</param>
        /// <returns>The ancestor elements, starting with parent and going towards the visual tree root.</returns>
        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start)
        {
            var parent = VisualTreeHelper.GetParent(start);

            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        /// <summary>
        /// Gets the siblings, including the start element.
        /// </summary>
        /// <param name="start">The start element.</param>
        /// <returns>The siblings, including the start element.</returns>
        public static IEnumerable<DependencyObject> GetSiblings(this DependencyObject start)
        {
            var parent = VisualTreeHelper.GetParent(start);

            if (parent == null)
            {
                yield return start;
            }
            else
            {
                var count = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified DependencyObject is in visual tree.
        /// </summary>
        /// <remarks>
        /// Note that this might not work as expected if the object is in a popup.
        /// </remarks>
        /// <param name="dob">The DependencyObject.</param>
        /// <returns>
        ///   <c>true</c> if the specified dob is in visual tree ; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInVisualTree(this DependencyObject dob)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return false;
            }

            if (Window.Current == null)
            {
                // This may happen when a picker or CameraCaptureUI etc. is open.
                return false;
            }

            var root = GetRealWindowRoot();

            return
                root != null && dob.GetAncestors().Contains(root) ||
                VisualTreeHelper.GetOpenPopups(Window.Current)
                    .Any(popup => popup.Child != null && dob.GetAncestors().Contains(popup.Child));
        }

        /// <summary>
        /// Gets the position of the element.
        /// </summary>
        /// <param name="dob">The element.</param>
        /// <param name="origin">The relative (0..1,0..1 range) position of a point within the element to evaluate. Defaults to 0,0 for top-left corner.</param>
        /// <param name="relativeTo">The element of reference. Defaults to visual tree root.</param>
        /// <returns>The position of origin point relative to specified element.</returns>
        public static Point GetPosition(this UIElement dob, Point origin = new Point(), UIElement relativeTo = null)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return new Point();
            }

            if (relativeTo == null)
            {
                relativeTo = Window.Current.Content;
            }

            if (relativeTo == null)
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            var fe = relativeTo as FrameworkElement;
            var aw = fe != null ? fe.ActualWidth : 0;
            var ah = fe != null ? fe.ActualHeight : 0;

            var absoluteOrigin = new Point(aw * origin.X, ah * origin.X);

            if (dob == relativeTo)
            {
                return absoluteOrigin;
            }

            var ancestors = dob.GetAncestors().ToArray();

            if (!ancestors.Contains(relativeTo))
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            return
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(absoluteOrigin);
        }

        /// <summary>
        /// Gets the bounding rectangle of a given element
        /// relative to a given other element or visual root
        /// if relativeTo is null or not specified.
        /// </summary>
        /// <remarks>
        /// Note that the bounding box is calculated based on the corners of the element relative to itself,
        /// so e.g. a bounding box of a rotated ellipse will be larger than necessary and in general
        /// bounding boxes of elements with transforms applied to them will often be calculated incorrectly.
        /// </remarks>
        /// <param name="dob">The starting element.</param>
        /// <param name="relativeTo">The relative to element.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Element not in visual tree.</exception>
        public static Rect GetBoundingRect(this UIElement dob, UIElement relativeTo = null)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return Rect.Empty;
            }

            if (relativeTo == null)
            {
                relativeTo = Window.Current.Content as FrameworkElement;
            }

            if (relativeTo == null)
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            if (dob == relativeTo)
            {
                var fe = relativeTo as FrameworkElement;
                var aw = fe != null ? fe.ActualWidth : 0;
                var ah = fe != null ? fe.ActualHeight : 0;

                return new Rect(0, 0, aw, ah);
            }

            //var ancestors = dob.GetAncestors().ToArray();

            //if (!ancestors.Contains(relativeTo))
            //{
            //    throw new InvalidOperationException("Element not in visual tree.");
            //}


            var fe2 = dob as FrameworkElement;
            var aw2 = fe2 != null ? fe2.ActualWidth : 0;
            var ah2 = fe2 != null ? fe2.ActualHeight : 0;

            var topLeft =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(new Point());
            var topRight =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            aw2,
                            0));
            var bottomLeft =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            0,
                            ah2));
            var bottomRight =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            aw2,
                            ah2));

            var minX = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Min();
            var maxX = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Max();
            var minY = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Min();
            var maxY = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Max();

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
