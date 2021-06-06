using System;

namespace MetricsManagerClient.Requests
{
    public class GetAllNetworkMetricsRequest
    {
        public DateTimeOffset FromTime { get; set; }
        
        public DateTimeOffset ToTime { get; set; }
    }
}