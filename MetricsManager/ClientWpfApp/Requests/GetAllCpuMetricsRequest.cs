using System;

namespace MetricsManagerClient.Requests
{
    public class GetAllCpuMetricsRequest
    {
        public DateTimeOffset FromTime { get; set; }
        
        public DateTimeOffset ToTime { get; set; }
    }
}