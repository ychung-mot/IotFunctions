using IotApis.HttpClients;
using IotApis.Model;
using IotApis.Service;
using IotCommon;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IotApis.Controllers
{
    [ApiController]
    [Route("api/camera/devices")]
    public class DeviceImageDataController : IotControllerBase
    {
        private readonly IDeviceImageDataService _deviceImageService;
        private readonly IIotCentralApi _iotCentralApi;
        private readonly ILogger<DeviceImageDataController> _logger;

        public DeviceImageDataController(IDeviceImageDataService deviceImageService, IIotCentralApi iotCentralApi, ILogger<DeviceImageDataController> logger)
        {
            _deviceImageService = deviceImageService;
            _iotCentralApi = iotCentralApi;
            _logger = logger;
        }

        [HttpGet("{deviceId}/imagedata", Name = "GetDeviceImageData")]
        public async Task<ActionResult<IEnumerable<DeviceImage>>> GetDeviceImageData(string deviceId, string dateFrom, string dateTo, string? preset)
        {
            if (!ValidateDate(dateFrom) || !ValidateDate(dateTo))
                return BadRequest();

            var dateFromTs = DateUtils.ConvertPacificToUtcTotalMilliseconds(DateTime.Parse(dateFrom));
            var dateToTs = DateUtils.ConvertPacificToUtcTotalMilliseconds(DateTime.Parse(dateTo).AddDays(1).AddSeconds(-1));

            return await _deviceImageService.GetDeviceImageData(deviceId, dateFromTs, dateToTs, preset);
        }

        [HttpGet("iotcentral/{deviceId}/imagedata", Name = "GetIotCentralDeviceImageData")]
        public async Task<ActionResult> GetIotCentralDeviceImageData(string deviceId, string dateFrom, string dateTo)
        {
            if (!ValidateDate(dateFrom) || !ValidateDate(dateTo))
                return BadRequest();

            var dateFromTs = DateTime.Parse(dateFrom).ToString("o", CultureInfo.InvariantCulture);
            var dateToTs = DateTime.Parse(dateTo).AddDays(1).AddSeconds(-1).ToString("o", CultureInfo.InvariantCulture);

            var responseMessage = await _iotCentralApi.GetCameraTelemetry(deviceId, dateFromTs, dateToTs);

            return await HandleResponseMessage(responseMessage);
        }

        [HttpGet("iotcentral/{deviceId}/property")]
        public async Task<ActionResult> GetIotCentralDeviceProperty(string deviceId)
        {
            var responseMessage = await _iotCentralApi.GetDeviceProperty(deviceId);

            return await HandleResponseMessage(responseMessage);
        }

        private bool ValidateDate(string dateStr)
        {
            var regex = new Regex(@"\d\d\d\d-\d\d-\d\d");

            return regex.Match(dateStr).Success;
        }


    }
}