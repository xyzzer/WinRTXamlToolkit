using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Composition
{
    // Thanks to Christoph Wille and his WinRT-snippets
    // https://github.com/christophwille/winrt-snippets/tree/master/RenderTextToBitmap
    // http://stackoverflow.com/questions/9151615/how-does-one-use-a-memory-stream-instead-of-files-when-rendering-direct2d-images

    public static class WriteableBitmapRenderExtensions
    {
        //Do not use the below extension as it is slower than the one that follows
        //public static async Task Render(this WriteableBitmap wb, FrameworkElement fe)
        //{
        //    var ms = await RenderToPngStream(fe);

        //    using (var msrandom = new MemoryRandomAccessStream(ms))
        //    {
        //        await wb.SetSourceAsync(msrandom);
        //    }
        //}

        public static async Task<WriteableBitmap> Render(FrameworkElement fe)
        {
            using (var engine = new CompositionEngine())
            {
                return await engine.RenderToWriteableBitmap(fe);
            }
        }

        public static async Task<MemoryStream> RenderToPngStream(FrameworkElement fe)
        {
            using (var engine = new CompositionEngine())
            {
                return await engine.RenderToPngStream(fe);
            }
        }
    }
}
