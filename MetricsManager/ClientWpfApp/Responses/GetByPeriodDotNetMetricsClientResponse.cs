using System.Collections.Generic;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.Responses
{
    public class GetByPeriodDotNetMetricsClientResponse
    {
        public List<DotNetMetricClientDto> Metrics { get; set; }
    }
}