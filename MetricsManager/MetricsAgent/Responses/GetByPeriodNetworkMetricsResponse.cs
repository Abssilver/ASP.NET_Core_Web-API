using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class GetByPeriodNetworkMetricsResponse
    {
        public List<NetworkMetricDto> Metrics { get; set; }
    }
}