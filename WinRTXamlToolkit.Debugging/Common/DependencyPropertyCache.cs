using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Debugging.Common
{
    internal class DependencyPropertyCache
    {
        public List<DependencyPropertyInfo> AttachedProperties { get; private set; }
        public Dictionary<Type, List<DependencyPropertyInfo>> DependencyProperties { get; private set; }

        public DependencyPropertyCache(Assembly userAssembly)
        {
            var platformTypes = typeof(FrameworkElement).GetTypeInfo().Assembly.ExportedTypes;
            var userTypes = userAssembly.ExportedTypes;
            var types = platformTypes.Concat(userTypes);

            AttachedProperties = new List<DependencyPropertyInfo>();
            DependencyProperties = new Dictionary<Type, List<DependencyPropertyInfo>>();

            foreach (var type in types)
            {
                var typeInfo = type.GetTypeInfo();

                List<DependencyPropertyInfo> propertyList = null;

                foreach (var dpPropertyInfo in
                    typeInfo
                        .DeclaredProperties
                        .Where(
                            pi =>
                            pi.GetMethod.IsStatic &&
                            pi.PropertyType == typeof (DependencyProperty)))
                {
                    var propertyName = dpPropertyInfo.Name.Substring(0, dpPropertyInfo.Name.Length - "Property".Length);

                    if (typeInfo.GetDeclaredProperty(propertyName) == null)
                    {
                        var name = dpPropertyInfo.Name.Substring(0, dpPropertyInfo.Name.Length - "Property".Length);
                        var displayName = string.Format("{0}.{1}", type.Name, name);
                        AttachedProperties.Add(
                            new DependencyPropertyInfo(
                                (DependencyProperty)dpPropertyInfo.GetValue(type),
                                name,
                                type,
                                displayName));
                    }
                    else
                    {
                        if (propertyList == null)
                        {
                            propertyList = new List<DependencyPropertyInfo>();
                            DependencyProperties.Add(type, propertyList);
                        }

                        var name = dpPropertyInfo.Name.Substring(0, dpPropertyInfo.Name.Length - "Property".Length);

                        propertyList.Add(
                            new DependencyPropertyInfo(
                                (DependencyProperty)dpPropertyInfo.GetValue(type),
                                name,
                                type,
                                name));
                    }
                }
            }
        }

        public IEnumerable<DependencyPropertyInfo> GetDependencyProperties(Type type)
        {
            var isTimeLine = typeof (Timeline).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());

            foreach (var attachedPropertyInfo in AttachedProperties)
            {
                if (isTimeLine || attachedPropertyInfo.OwnerType != typeof (Storyboard))
                {
                    yield return attachedPropertyInfo;
                }
            }

            while (type != typeof (object))
            {
                List<DependencyPropertyInfo> declaredDependencyProperties;

                if (DependencyProperties.TryGetValue(type, out declaredDependencyProperties))
                {
                    foreach (var declaredDependencyProperty in declaredDependencyProperties)
                    {
                        yield return declaredDependencyProperty;
                    }
                }

                type = type.GetTypeInfo().BaseType;
            }
        }
    }

    internal class DependencyPropertyInfo
    {
        public DependencyProperty Property { get; private set; }
        public string Name { get; private set; }
        public Type OwnerType { get; private set; }
        public string DisplayName { get; private set; }

        public DependencyPropertyInfo(
            DependencyProperty property,
            string name,
            Type ownerType,
            string displayName)
        {
            Property = property;
            Name = name;
            OwnerType = ownerType;
            DisplayName = displayName;
        }
    }
}
