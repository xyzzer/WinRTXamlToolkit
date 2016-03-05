using System;
using System.Collections.Generic;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Manages instantiation, retrieval and caching of AlternativePage instances used in AlternativeFrame
    /// in a manner similar to the original Frame class in the framework.
    /// AlternativeFrame should always get page instances by calling FrameCache.Get()
    /// when navigating to a page or preloading one
    /// and store a page by calling FrameCache.Store() when navigating away from it.
    /// FrameCache decides whether a page should actually be cached when calling Store() or whether to ignore the call.
    /// </summary>
    internal class FrameCache
    {
        private readonly Dictionary<Type, List<AlternativePage>> _typeToPageListMap = new Dictionary<Type, List<AlternativePage>>();
        private readonly List<AlternativePage> _limitedCache = new List<AlternativePage>();

        #region CTOR
        internal FrameCache(int cacheSize)
        {
            _cacheSize = cacheSize;
        } 
        #endregion

        #region CacheSize
        private int _cacheSize;
        internal int CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                _cacheSize = value;
                TrimLimitedCache();
            }
        }
        #endregion CacheSize

        #region Get()
        /// <summary>
        /// Gets the page of specified type.
        /// If one is already cached - it removes it from the cache and returns it.
        /// If one isn't cached - it returns a new instance.
        /// </summary>
        /// <remarks>
        /// There is no guarantee of returning a specific page from the cache
        /// if there are multiple pages of the same type stored.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <returns>An AlternativePage of a given type.</returns>
        internal AlternativePage Get(Type type)
        {
            List<AlternativePage> pageList;

            if (_typeToPageListMap.TryGetValue(type, out pageList))
            {
                if (pageList.Count > 0)
                {
                    var page = pageList[pageList.Count - 1];
                    pageList.RemoveAt(pageList.Count - 1);

                    if (page.NavigationCacheMode == NavigationCacheMode.Enabled)
                    {
                        _limitedCache.Remove(page);
                    }

                    return page;
                }
            }

            var newPage = Activator.CreateInstance(type) as AlternativePage;

            return newPage;
        } 
        #endregion

        #region Store()
        /// <summary>
        /// Stores the specified page in cache.
        /// </summary>
        /// <remarks>
        /// Multiple pages of same type may be stored.
        /// </remarks>
        /// <param name="page">The page.</param>
        public void Store(AlternativePage page)
        {
            if (page.NavigationCacheMode == NavigationCacheMode.Disabled)
            {
                return;
            }

            var pageType = page.GetType();
            List<AlternativePage> pageList;

            if (!_typeToPageListMap.TryGetValue(pageType, out pageList))
            {
                _typeToPageListMap.Add(pageType, pageList = new List<AlternativePage>());
            }

            pageList.Add(page);

            _limitedCache.Add(page);

            TrimLimitedCache();
        } 
        #endregion

        #region TrimLimitedCache()
        private void TrimLimitedCache()
        {
            while (_limitedCache.Count > _cacheSize)
            {
                var page = _limitedCache[0];
                _typeToPageListMap[page.GetType()].Remove(page);
                _limitedCache.RemoveAt(0);
            }
        }
        #endregion
    }
}