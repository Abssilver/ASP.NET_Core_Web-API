using System;

namespace MetricsManagerClient.Requests
{
    public class GetDotNetMetricsFromAgentRequest
    {
        public int AgentId { get; set; }
        
        public DateTimeOffset FromTime { get; set; }
        
        public DateTimeOffset ToTime { get; set; }
    }
}