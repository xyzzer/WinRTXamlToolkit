using System.Linq;
using Jupiter = Windows.UI.Xaml;
using D2D = SharpDX.Direct2D1;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class ShapeExtensions
    {
        public static D2D.StrokeStyle GetStrokeStyle(this Jupiter.Shapes.Shape shape, D2D.Factory factory)
        {
            var properties = new D2D.StrokeStyleProperties();

            properties.StartCap = shape.StrokeStartLineCap.ToSharpDX();
            properties.EndCap = shape.StrokeEndLineCap.ToSharpDX();
            properties.LineJoin = shape.StrokeLineJoin.ToSharpDX();
            properties.MiterLimit = (float)shape.StrokeMiterLimit;

            if (shape.StrokeDashArray.Count > 0)
            {
                properties.DashCap = shape.StrokeDashCap.ToSharpDX();
                properties.DashOffset = (float)shape.StrokeDashOffset;
                properties.DashStyle = D2D.DashStyle.Custom;
                return new D2D.StrokeStyle(factory, properties, shape.StrokeDashArray.Select(d => (float)d).ToArray());
            }

            properties.DashStyle = D2D.DashStyle.Solid;
            return new D2D.StrokeStyle(factory, properties);
        }
    }
}
