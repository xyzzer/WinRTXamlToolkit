using System;
using System.Linq;

namespace WinRTXamlToolkit.Tools
{
    public static class EnumExtensions
    {
        public static TEnumType[] GetValues<TEnumType>(Func<TEnumType,bool> condition = null)
        {
            if (condition == null)
            {
                return Enum.GetValues(typeof (TEnumType)).Cast<TEnumType>().ToArray();
            }
            else
            {
                return Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>().Where(condition).ToArray();
            }
        }
    }
}
