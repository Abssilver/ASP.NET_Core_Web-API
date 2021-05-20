using System.Collections.Generic;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.Responses
{
    public class GetByPeriodNetworkMetricsApiResponse
    {
        public List<NetworkMetricDto> Metrics { get; set; }
    }
}