using System;

namespace MetricsManagerClient.DataLayer.Models
{
    public class NetworkMetricClient
    {
        public int Id { get; set; } 
        public int Value { get; set; } 
        public DateTimeOffset Time { get; set; }
    }
}