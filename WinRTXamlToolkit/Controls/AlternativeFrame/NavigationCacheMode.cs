using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Specifies how the page is cached when used within a frame.
    /// </summary>
    /// <remarks>
    /// You use the NavigationCacheMode enumeration when setting the NavigationCacheMode property
    /// of the <see cref="AlternativePage"/> class.
    /// You specify whether a new instance of the page is created for each visit to the page or whether
    /// a previously constructed instance of the page that has been saved in the cache is used for each visit.
    /// <p/>
    /// The default value for the NavigationCacheMode property is Disabled.
    /// Set the NavigationCacheMode property to Enabled or Required when a new instance of the page is not essential for each visit.
    /// By using a cached instance of the page, you can improve the performance of your application and
    /// reduce the load on your server. Set the NavigationCacheMode property to Disabled
    /// if a new instance must be created for each visit. For example, you should not cache a page that
    /// displays information that is unique to each customer.
    /// <p/>
    /// The OnNavigatedTo method is called for each request, even when the page is retrieved from the cache.
    /// You should include in this method code that must be executed for each request rather than placing
    /// that code in the Page constructor.
    /// </remarks>
    public enum NavigationCacheMode
    {
        /// <summary>
        /// The page is never cached and a new instance of the page is created on each visit.
        /// </summary>
        Disabled,
        /// <summary>
        /// The page is cached and the cached instance is reused for every visit regardless of the cache size for the frame.
        /// </summary>
        Required,
        /// <summary>
        /// The page is cached, but the cached instance is discarded when the size of the cache for the frame is exceeded.
        /// </summary>
        Enabled
    }
}
