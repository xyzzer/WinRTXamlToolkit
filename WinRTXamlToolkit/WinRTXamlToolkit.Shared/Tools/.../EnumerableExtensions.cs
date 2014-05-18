using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Provides extensions to IEnumerable types.
    /// </summary>
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Non-optimized method that shuffles the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static List<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var copy = list.ToList();

            var ret = new List<T>(copy.Count);

            while (copy.Count > 0)
            {
                var i = _random.Next(copy.Count);
                ret.Add(copy[i]);
                copy.RemoveAt(i);
            }

            return ret;
        }

        /// <summary>
        /// Runs a specific action for each item in an enumeration.
        /// </summary>
        /// <typeparam name="T">Type of objects enumerated.</typeparam>
        /// <param name="list">The enumeration.</param>
        /// <param name="action">Action to invoke.</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }
    }
}