using System;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.Common
{
    public class DependencyPropertyInfo
    {
        public DependencyProperty Property { get; private set; }
        public string Name { get; private set; }
        public Type OwnerType { get; private set; }
        public string DisplayName { get; private set; }
        public bool IsAttached { get; private set; }

        public DependencyPropertyInfo(
            DependencyProperty property,
            string name,
            Type ownerType,
            string displayName,
            bool isAttached)
        {
            Property = property;
            Name = name;
            OwnerType = ownerType;
            DisplayName = displayName;
            IsAttached = isAttached;
        }
    }
}