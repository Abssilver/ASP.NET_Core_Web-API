
using System;

namespace MetricsAgent.Responses.DataTransferObjects
{
    public class NetworkMetricDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }

}