namespace IotApis.HttpClients
{
    public interface IIotCentralApi
    {
        Task<HttpContent> GetWeatherTelemetry(string deviceId, string dateFrom, string dateTo);
        Task<HttpContent> GetCameraTelemetry(string deviceId, string dateFrom, string dateTo);
    }
    public class IotCentralApi : IIotCentralApi
    {
        private HttpClient _client;
        private IApi _api;
        private IConfiguration _config;
        private ILogger<IotCentralApi> _logger;

        public IotCentralApi(HttpClient client, IApi api, IConfiguration config, ILogger<IotCentralApi> logger)
        {
            _client = client;
            _api = api;
            _config = config;
            _logger = logger;
        }

        public async Task<HttpContent> GetWeatherTelemetry(string deviceId, string dateFrom, string dateTo)
        {
            var path = _config.GetValue<string>("IotCentral:TelemetryPath") ?? "";

            var query = "SELECT $id, $ts, deviceId, measurements FROM dtmi:modelDefinition:h9cornd8k:segb9agyn3" +
                $" WHERE $ts >= '{dateFrom}' AND $ts <= '{dateTo}' AND $id = '{deviceId}'";

            var body = $"{{ \"query\": \"{query}\" }}";

            var response = await _api.Post(_client, path, body);

            return response.Content;
        }

        public async Task<HttpContent> GetCameraTelemetry(string deviceId, string dateFrom, string dateTo)
        {
            var path = _config.GetValue<string>("IotCentral:TelemetryPath") ?? "";

            var query = "SELECT $id, $ts, CameraDatas FROM dtmi:modelDefinition:jipkbe6hr:g8voncqowt" +
                $" WHERE $ts >= '{dateFrom}' AND $ts <= '{dateTo}' AND $id = '{deviceId}'";

            var body = $"{{ \"query\": \"{query}\" }}";

            var response = await _api.Post(_client, path, body);

            return response.Content;
        }
    }
}
