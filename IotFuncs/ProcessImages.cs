using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace IotFuncs
{
    public static class ProcessImages
    {
        [FunctionName("ResizeAndAddWatermark")]
        public static async Task Run(
            [BlobTrigger("images-src/{name}")] Stream source,
            [Blob("images-dest/{name}", FileAccess.Write)] Stream destination,
            string name,
            ILogger log)
        {
            var font = SystemFonts.CreateFont("Arial", 10);
            var text = "Ministry of Transportation and Infrastructure";

            using var input = Image.Load<Rgba32>(source, out IImageFormat format);

            input.Mutate(x => x.Resize(400, 0));
            input.Mutate(x => x.ApplyWaterMark(text, font, Color.White));

            await input.SaveAsync(destination, format);

            log.LogInformation($"{name} has been processed.");
        }
    }   
}
