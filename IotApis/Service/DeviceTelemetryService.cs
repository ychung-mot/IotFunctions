using IotApis.Data;
using IotApis.Model;
using Microsoft.EntityFrameworkCore;

namespace IotApis.Service
{
    public interface IDeviceTelemetryService
    {
        public Task<List<DeviceTelemetry>> GetDeviceTelemetries(string deviceId, long dateFrom, long dateTo);
    }

    public class DeviceTelemetryService : IDeviceTelemetryService
    {
        private IDbContextFactory<WeatherContext> _contextFactory;

        public DeviceTelemetryService(IDbContextFactory<WeatherContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<List<DeviceTelemetry>> GetDeviceTelemetries(string deviceId, long dateFrom, long dateTo)
        {
            using var dbContext = await _contextFactory.CreateDbContextAsync();

            var telemetries = await dbContext.DeviceTelemetry
                .Where(x => x.deviceId == deviceId && x.timestamp >= dateFrom && x.timestamp <= dateTo)
                .ToListAsync();

            return telemetries;
        }
    }
}
