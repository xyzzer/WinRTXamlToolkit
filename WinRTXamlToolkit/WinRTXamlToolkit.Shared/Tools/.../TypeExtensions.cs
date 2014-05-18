using System;
using System.Reflection;

namespace WinRTXamlToolkit.Tools
{
    /// <summary>
    /// Provides some reflection extension for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsGenericType &&
                             type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }
    }
}
