using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Contains extension methods for TextBlock.
    /// </summary>
    public static class TextBlockExtensions
    {
        /// <summary>
        /// Gets the rectangle boundaries of a character in a TextBlock.
        /// </summary>
        /// <param name="tb">The TextBlock.</param>
        /// <param name="characterIndex">Index of the character.</param>
        /// <returns></returns>
        public static Rect GetCharacterRect(this TextBlock tb, int characterIndex)
        {
            var aw = tb.ActualWidth;
            var ah = tb.ActualHeight;

            int offset = 0;
            bool headerPrinted = false;

            var previousCharacterRect = new Rect(-100000, 0, 0, 0);
            var pointer = tb.ContentStart.GetPositionAtOffset(offset, LogicalDirection.Forward);
            var rect = pointer.GetCharacterRect(LogicalDirection.Forward);

            for (var i = 0; i <= characterIndex; i++)
            {
                while (
                    rect == previousCharacterRect ||
                    rect.X - previousCharacterRect.X < 4)
                {
                    offset++;
                    if (offset > tb.ContentEnd.Offset)
                        break;
                    pointer = tb.ContentStart.GetPositionAtOffset(offset, LogicalDirection.Forward);
                    rect = pointer.GetCharacterRect(LogicalDirection.Forward);
                }

                previousCharacterRect = rect;

                if (i == characterIndex)
                {
                    //Debug.WriteLine(rect.ToString());

                    while (
                        rect == previousCharacterRect ||
                        rect.X - previousCharacterRect.X < 4)
                    {
                        offset++;
                        if (offset > tb.ContentEnd.Offset)
                            break;
                        pointer = tb.ContentStart.GetPositionAtOffset(offset, LogicalDirection.Forward);
                        rect = pointer.GetCharacterRect(LogicalDirection.Forward);
                        //Debug.WriteLine(rect.ToString());
                    }

                    offset++;

                    var x = previousCharacterRect.X;
                    var y = previousCharacterRect.Y;
                    // If the text gets trimmed - trimmed rects will be 0,0,0,0
                    var w = Math.Max(rect.X - previousCharacterRect.X, 0);
                    var h = previousCharacterRect.Height;

                    var pox = (x + (w / 2)) / aw;

                    if (!headerPrinted)
                    {
                        //Debug.WriteLine("ActualWidth: {0}", aw);
                        //Debug.WriteLine("ActualHeight: {0}\r\n", ah);
                        //Debug.WriteLine("po\ti\tx\ty\tw\th\tpox");
                        headerPrinted = true;
                    }

                    //Debug.WriteLine(
                    //    "{0:F0}\t{1:F0}\t{2:F0}\t{3:F0}\t{4:F0}\t{5:F0}\t{6:F3}",
                    //    pointer.Offset, characterIndex, x, y, w, h, pox);

                    return new Rect(x, y, w, h);
                }
            }

            return new Rect();
        }
    }
}
