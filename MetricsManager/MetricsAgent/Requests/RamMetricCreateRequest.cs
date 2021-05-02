using System;

namespace MetricsAgent.Requests
{
    public class RamMetricCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
    }
}