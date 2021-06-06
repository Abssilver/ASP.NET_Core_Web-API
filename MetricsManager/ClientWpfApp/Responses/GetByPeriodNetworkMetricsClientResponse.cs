using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.Responses
{
    public class GetByPeriodNetworkMetricsClientResponse
    {
        public List<NetworkMetricClientDto> Metrics { get; set; }
    }
}