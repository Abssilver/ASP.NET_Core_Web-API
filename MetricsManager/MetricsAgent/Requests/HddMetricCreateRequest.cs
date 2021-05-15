using System;

namespace MetricsAgent.Requests
{
    public class HddMetricCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
    }
}