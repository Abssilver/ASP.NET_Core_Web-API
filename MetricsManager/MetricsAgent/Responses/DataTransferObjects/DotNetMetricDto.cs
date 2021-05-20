
using System;

namespace MetricsAgent.Responses.DataTransferObjects
{
    public class DotNetMetricDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }

}