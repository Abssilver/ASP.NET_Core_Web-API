using System;

namespace MetricsManagerClient.Requests
{
    public class GetCpuMetricsFromAgentRequest
    {
        public int AgentId { get; set; }
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}