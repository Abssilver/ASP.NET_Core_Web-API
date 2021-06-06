using System;

namespace MetricsManagerClient.Requests
{
    public class GetAllHddMetricsRequest
    {
        public DateTimeOffset FromTime { get; set; }
        
        public DateTimeOffset ToTime { get; set; }
    }
}