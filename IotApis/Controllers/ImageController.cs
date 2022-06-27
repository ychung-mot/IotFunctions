using Azure.Storage.Blobs;
using IotCommon;
using Microsoft.AspNetCore.Mvc;

namespace IotApis.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : ControllerBase
    {
        private IConfiguration _config;

        public ImageController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet()]
        public async Task<IActionResult> GetImage(string imagePath)
        {
            var connString = _config.GetConnectionString(Constants.BlobConnString);

            var client = new BlobServiceClient(connString);

            var parsed = ParseImagePath(imagePath);
            if (!parsed.valid) return BadRequest();

            var container = client.GetBlobContainerClient(parsed.containerName);
            var blobClient = container.GetBlobClient(parsed.blobName);

            if (!blobClient.Exists())
                return NotFound();

            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            return File(stream, "image/jpeg");
        }

        private (bool valid, string containerName, string blobName) ParseImagePath(string imagePath)
        {
            Uri uri;

            try
            {
                uri = new Uri(imagePath);
            }
            catch
            {
                return (false, "", "");
            }

            return (true, uri.Segments[1], uri.LocalPath.Replace(uri.Segments[1], ""));
        }
    }
}
