//#define DUMPTYPES
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
#if DUMPTYPES
using System.Text;
#endif
using System.Threading.Tasks;
using WinRTXamlToolkit.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Automation;

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

            var someType = typeof(FrameworkElement);
            var someTypeInfo = someType.GetTypeInfo();
            var someAssembly = someTypeInfo.Assembly;
            IEnumerable<Type> platformTypes = null;

            try
            {
                platformTypes = someAssembly.ExportedTypes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                platformTypes = KnownTypes.Types;
            }

#if DUMPTYPES
            var sb = new StringBuilder();
            foreach (var type in platformTypes)
            {
                sb.AppendFormat("typeof({0}),\r\n", type.FullName);
                FindDependencyProperties(type);
            }
            var str = sb.ToString();
            Debug.WriteLine(str);
#else
            foreach (var type in platformTypes)
            {
                FindDependencyProperties(type);
            }
#endif

            try
            {
                var userAssemblies = await PackageHelper.GetPackageAssembliesAsync();

                foreach (var userAssembly in userAssemblies)
                {
                    try
                    {
                        foreach (var type in userAssembly.ExportedTypes)
                        {
                            FindDependencyProperties(type);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                    // Uncomment to printf-debug app-crashing type scanning issues
                    //System.Diagnostics.Debug.WriteLine("S{2}>{0}.{1}", type.ToString(), dpPropertyInfo.Name, type.GetTypeInfo().Assembly.FullName);
                    //if (type.FullName == "Windows.UI.Xaml.Controls.Primitives.LoopingSelector" &&
                    //    dpPropertyInfo.Name == "ShouldLoopProperty")
                    //{
                    //    Debugger.Break();
                    //}

                    if (type == typeof (AutomationProperties) &&
                        dpPropertyInfo.Name == "AccessibilityViewProperty")
                    {
                        continue;
                    }

                    if (type.FullName == "Windows.UI.Xaml.Controls.Maps.MapControl")
                    {
                        continue;
                    }

                    if (type.FullName == "Windows.UI.Xaml.Controls.AutoSuggestBoxTextChangedEventArgs" &&
                        dpPropertyInfo.Name == "ReasonProperty" )
                    {
                        continue;
                    }

                    if (type.FullName == "Windows.UI.Xaml.Controls.Maps.MapIcon" &&
                        (dpPropertyInfo.Name == "LocationProperty" ||
                        dpPropertyInfo.Name == "NormalizedAnchorPointProperty" ||
                        dpPropertyInfo.Name == "TitleProperty"))
                    {
                        continue;
                    }

                    if (type.FullName == "Windows.UI.Xaml.Controls.Maps.MapItemsControl" &&
                        (dpPropertyInfo.Name == "ItemTemplateProperty" ||
                        dpPropertyInfo.Name == "ItemsProperty" ||
                        dpPropertyInfo.Name == "ItemsSourceProperty"))
                    {
                        continue;
                    }

                    if (type.FullName == "Windows.UI.Xaml.Controls.Maps.MapPolygon" &&
                        (dpPropertyInfo.Name == "PathProperty" ||
                        dpPropertyInfo.Name == "StrokeDashedProperty" ||
                        dpPropertyInfo.Name == "StrokeThicknessProperty"))
                    {
                        continue;
                    }

                    if (type.FullName == "Windows.UI.Xaml.Controls.Maps.MapPolyline" &&
                        (dpPropertyInfo.Name == "PathProperty" ||
                        dpPropertyInfo.Name == "StrokeDashedProperty"))
                    {
                        continue;
                    }

                    var dependencyProperty = (DependencyProperty)dpPropertyInfo.GetValue(type);
                    var propertyName =
                        dpPropertyInfo.Name.Substring(
                            0,
                            dpPropertyInfo.Name.Length - "Property".Length);
                    AddDependencyPropertyInfo(type, typeInfo, dependencyProperty, propertyName, ref propertyList);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed>{0}.{1}", type.ToString(), dpPropertyInfo.Name);
                }
            }

            if (!typeInfo.ContainsGenericParameters)
                foreach (var dpFieldInfo in
                    typeInfo
                        .DeclaredFields
                        .Where(df => df.IsStatic && df.FieldType == typeof(DependencyProperty)))
                {
                    var dependencyProperty = (DependencyProperty)dpFieldInfo.GetValue(type);
                    var propertyName =
                        dpFieldInfo.Name.Substring(
                            0,
                            dpFieldInfo.Name.Length - "Property".Length);
                    AddDependencyPropertyInfo(type, typeInfo, dependencyProperty, propertyName, ref propertyList);
                }
        }

        /// <summary>
        /// Adds the DependencyPropertyInfo object for the given.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="typeInfo">The type info.</param>
        /// <param name="dependencyProperty">The dependency property.</param>
        /// <param name="propertyName">Name of the dependency property.</param>
        /// <param name="propertyList">The non-attached dependency property list for the given type.</param>
        private static void AddDependencyPropertyInfo(
            Type type,
            TypeInfo typeInfo,
            DependencyProperty dependencyProperty,
            string propertyName,
            ref List<DependencyPropertyInfo> propertyList)
        {
            try
            {
                bool? isAttached = null;

                // Check for plain property matching the dependency property.
                if (typeInfo.GetDeclaredProperty(propertyName) == null)
                {
                    isAttached = true;
                }
                else
                {
                    // Check for the Get method typically only specified for attached properties
                    var getMethodName = string.Format("Get{0}", propertyName);
                    var getMethod = typeInfo.GetDeclaredMethod(getMethodName);

                    if (getMethod != null)
                    {
                        isAttached = true;
                    }
                    else
                    {
                        isAttached = false;
                    }
                }

                if (isAttached == true)
                {
                    // Attached property
                    var displayName = string.Format("{0}.{1}", type.Name, propertyName);

                    AttachedProperties.Add(
                        new DependencyPropertyInfo(
                            dependencyProperty,
                            propertyName,
                            type,
                            displayName,
                            true));
                }
                else
                {
                    // non-attached property
                    if (propertyList == null)
                    {
                        propertyList = new List<DependencyPropertyInfo>();
                        DependencyProperties.Add(type, propertyList);
                    }

                    propertyList.Add(
                        new DependencyPropertyInfo(
                            dependencyProperty,
                            propertyName,
                            type,
                            propertyName,
                            false));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
