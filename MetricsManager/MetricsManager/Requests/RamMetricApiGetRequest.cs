using System;

namespace MetricsManager.Requests
{
    public class RamMetricApiGetRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
        public string ClientBaseAddress { get; set; }
    }
}