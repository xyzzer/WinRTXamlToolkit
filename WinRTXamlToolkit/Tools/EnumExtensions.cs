using System;
using System.Linq;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Enum type extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the values declared by the given enum.
        /// </summary>
        /// <typeparam name="TEnumType">The enum type.</typeparam>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public static TEnumType[] GetValues<TEnumType>(Func<TEnumType,bool> condition = null)
        {
            if (condition == null)
            {
                return Enum.GetValues(typeof (TEnumType)).Cast<TEnumType>().ToArray();
            }

            return Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>().Where(condition).ToArray();
        }
    }
}
