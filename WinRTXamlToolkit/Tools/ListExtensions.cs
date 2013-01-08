using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Tools
{
    public static class ListExtensions
    {
        private static readonly Random _random = new Random();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            var copy = list.ToList();

            var ret = new List<T>(list.Count);

            while (copy.Count > 0)
            {
                var i = _random.Next(copy.Count);
                ret.Add(copy[i]);
                copy.RemoveAt(i);
            }

            return ret;
        }
    }
}