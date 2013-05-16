using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WinRTXamlToolkit.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Debugging.Common
{
    /// <summary>
    /// Provides information about all dependency properties.
    /// </summary>
    internal static class DependencyPropertyCache
    {
        private static Task _initializationTask;

        public static HashSet<DependencyPropertyInfo> AttachedProperties { get; private set; }
        public static Dictionary<Type, List<DependencyPropertyInfo>> DependencyProperties { get; private set; }

        static DependencyPropertyCache()
        {
            Initialize();
        }

        private static async void Initialize()
        {
            var tcs = new TaskCompletionSource<bool>(false);
            _initializationTask = tcs.Task;

            AttachedProperties = new HashSet<DependencyPropertyInfo>();
            DependencyProperties = new Dictionary<Type, List<DependencyPropertyInfo>>();

            var platformTypes = typeof(FrameworkElement).GetTypeInfo().Assembly.ExportedTypes;

            foreach (var type in platformTypes)
            {
                FindDependencyProperties(type);
            }

            var userAssemblies = await PackageHelper.GetPackageAssembliesAsync();

            foreach (var userAssembly in userAssemblies)
            {
                foreach (var type in userAssembly.ExportedTypes)
                {
                    FindDependencyProperties(type);
                }
            }

            tcs.SetResult(true);
        }

        /// <summary>
        /// Finds and caches the info about all dependency properties.
        /// </summary>
        /// <param name="type">The type.</param>
        private static void FindDependencyProperties(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            List<DependencyPropertyInfo> propertyList = null;

            foreach (var dpPropertyInfo in
                typeInfo
                    .DeclaredProperties
                    .Where(
                        pi =>
                        pi.GetMethod != null &&
                        pi.GetMethod.IsStatic &&
                        pi.PropertyType == typeof (DependencyProperty)))
            {
                try
                {
                    var propertyName = dpPropertyInfo.Name.Substring(
                        0, dpPropertyInfo.Name.Length - "Property".Length);

                    if (typeInfo.GetDeclaredProperty(propertyName) == null)
                    {
                        var name = dpPropertyInfo.Name.Substring(
                            0, dpPropertyInfo.Name.Length - "Property".Length);
                        var displayName = string.Format("{0}.{1}", type.Name, name);
                        var dependencyProperty = (DependencyProperty)dpPropertyInfo.GetValue(type);
                        AttachedProperties.Add(
                            new DependencyPropertyInfo(
                                dependencyProperty,
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

                        var name = dpPropertyInfo.Name.Substring(
                            0, dpPropertyInfo.Name.Length - "Property".Length);

                        propertyList.Add(
                            new DependencyPropertyInfo(
                                (DependencyProperty)dpPropertyInfo.GetValue(type),
                                name,
                                type,
                                name));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public static async Task<List<DependencyPropertyInfo>> GetDependencyProperties(Type type)
        {
            await _initializationTask;

            return GetDependencyPropertiesCore(type).ToList();
        }

        private static IEnumerable<DependencyPropertyInfo> GetDependencyPropertiesCore(Type type)
        {
            var isTimeLine = typeof (Timeline).GetTypeInfo()
                                              .IsAssignableFrom(type.GetTypeInfo());

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
}
