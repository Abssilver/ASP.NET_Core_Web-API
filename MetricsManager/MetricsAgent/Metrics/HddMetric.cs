using System;

namespace MetricsAgent.Metrics
{
    public class HddMetric
    {
        public int Id { get; set; } 
        public int Value { get; set; } 
        public DateTimeOffset Time { get; set; }
    }
}