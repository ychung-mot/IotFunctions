using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace IotFuncs
{
    public static class Extensions
    {
        public static IImageProcessingContext ApplyWaterMark(this IImageProcessingContext context, string text, Font font, Color color)
        {
            var imgSize = context.GetCurrentSize();
            var fontSize = TextMeasurer.Measure(text, new TextOptions(font));

            var x = imgSize.Width - fontSize.Width - 10;
            var y = imgSize.Height - fontSize.Height - 10;

            return context.DrawText(text, font, color, new PointF(x, y));
        }
    }
}
