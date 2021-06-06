using System;

namespace MetricsManagerClient.DataLayer.Models
{
    public class RamMetricClient
    {
        public int Id { get; set; } 
        public int Value { get; set; } 
        public DateTimeOffset Time { get; set; }
    }
}