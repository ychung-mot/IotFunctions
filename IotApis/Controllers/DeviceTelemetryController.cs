using IotApis.Model;
using IotApis.Service;
using IotCommon;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace IotApis.Controllers
{
    [ApiController]
    [Route("api/weather/device")]
    public class DeviceTelemetryController : ControllerBase
    {
        private readonly IDeviceTelemetryService _deviceTelemetryService;
        private readonly ILogger<DeviceTelemetryController> _logger;

        public DeviceTelemetryController(IDeviceTelemetryService deviceTelemetryService, ILogger<DeviceTelemetryController> logger)
        {
            _deviceTelemetryService = deviceTelemetryService;
            _logger = logger;
        }

        [HttpGet("{deviceId}/telemetry", Name = "GetDeviceTelemetries")]
        public async Task<ActionResult<IEnumerable<DeviceTelemetry>>> GetDeviceTelemetries(string deviceId, string dateFrom, string dateTo)
        {
            if (!ValidateDate(dateFrom) || !ValidateDate(dateTo))
                return BadRequest();

            var dateFromTs = DateUtils.ConvertPacificToUtcTotalMilliseconds(DateTime.Parse(dateFrom));
            var dateToTs = DateUtils.ConvertPacificToUtcTotalMilliseconds(DateTime.Parse(dateTo).AddDays(1).AddSeconds(-1));

            return await _deviceTelemetryService.GetDeviceTelemetries(deviceId, dateFromTs, dateToTs);
        }

        private bool ValidateDate(string dateStr)
        {
            var regex = new Regex(@"\d\d\d\d-\d\d-\d\d");

            return regex.Match(dateStr).Success;
        }     


    }
}