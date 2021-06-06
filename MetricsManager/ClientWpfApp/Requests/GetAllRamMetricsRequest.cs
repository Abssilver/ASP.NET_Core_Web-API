using System;

namespace MetricsManagerClient.Requests
{
    public class GetAllRamMetricsRequest
    {
        public DateTimeOffset FromTime { get; set; }
        
        public DateTimeOffset ToTime { get; set; }
    }
}