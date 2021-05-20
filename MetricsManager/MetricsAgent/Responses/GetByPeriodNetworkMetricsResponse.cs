using System.Collections.Generic;
using MetricsAgent.Responses.DataTransferObjects;

namespace MetricsAgent.Responses
{
    public class GetByPeriodNetworkMetricsResponse
    {
        public List<NetworkMetricDto> Metrics { get; set; }
    }
}