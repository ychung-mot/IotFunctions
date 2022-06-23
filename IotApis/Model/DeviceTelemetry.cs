namespace IotApis.Model
{
    public class DeviceTelemetry
    {
        public string deviceId { get; set; }
        public IDictionary<string, string> measurements { get; set; }
        public long timestamp { get; set; }
    }
}
