using System;

namespace MetricsManager.Requests
{
    public class NetworkMetricApiGetRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
        public string ClientBaseAddress { get; set; }
    }
}