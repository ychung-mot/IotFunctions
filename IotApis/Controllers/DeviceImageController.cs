using IotApis.Model;
using IotApis.Service;
using IotCommon;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace IotApis.Controllers
{
    [ApiController]
    [Route("api/camera/devices")]
    public class DeviceImageController : ControllerBase
    {
        private readonly IDeviceImageService _deviceImageService;
        private readonly ILogger<DeviceImageController> _logger;

        public DeviceImageController(IDeviceImageService deviceImageService, ILogger<DeviceImageController> logger)
        {
            _deviceImageService = deviceImageService;
            _logger = logger;
        }

        [HttpGet("{deviceId}/images", Name = "GetDeviceImages")]
        public async Task<ActionResult<IEnumerable<DeviceImage>>> GetDeviceImages(string deviceId, string dateFrom, string dateTo, string? preset)
        {
            if (!ValidateDate(dateFrom) || !ValidateDate(dateTo))
                return BadRequest();

            var dateFromTs = DateUtils.ConvertPacificToUtcTotalMilliseconds(DateTime.Parse(dateFrom));
            var dateToTs = DateUtils.ConvertPacificToUtcTotalMilliseconds(DateTime.Parse(dateTo).AddDays(1).AddSeconds(-1));

            return await _deviceImageService.GetDeviceImages(deviceId, dateFromTs, dateToTs, preset);
        }

        private bool ValidateDate(string dateStr)
        {
            var regex = new Regex(@"\d\d\d\d-\d\d-\d\d");

            return regex.Match(dateStr).Success;
        }
    }
}