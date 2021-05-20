using System;

namespace MetricsManager.Requests
{
    public class HddMetricsApiGetRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
        public string ClientBaseAddress { get; set; }
    }
}